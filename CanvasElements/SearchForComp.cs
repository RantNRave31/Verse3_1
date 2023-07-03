using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Verse3;
using Verse3.Components;
using Verse3.CorePresentation.Workspaces;
using Verse3.Elements;

namespace MathLibrary
{
    public class SearchForComp : BaseCompViewModel
    {

        public static SearchForComp Instance = null;

        #region Constructors
        
        public SearchForComp() : base()
        {
        }

        public SearchForComp(int x, int y) : base(x, y)
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                WorkspaceViewModel.StaticSelectedDataViewModel.Elements.Remove(Instance);
                Instance = this;
            }
        }

        #endregion

        public override CompInfo GetCompInfo() => new CompInfo(this, "Search", "_CanvasElements", "_CanvasElements");

        public override void Compute()
        {
        }
        
        private SearchBoxElementViewModel searchBlock = new SearchBoxElementViewModel();
        public override void Initialize()
        {
            base.titleTextBlock.TextRotation = 0;
            
            searchBlock = new SearchBoxElementViewModel();
            searchBlock.Width = 200;
            searchBlock.InputText = "";
            searchBlock.IsSelected = true;
            searchBlock.SearchStarted += SearchBlock_SearchStarted;
            this.ChildElementManager.AddElement(searchBlock);
        }

        private Dictionary<ButtonElementViewModel, CompInfo> buttonDictionary = new Dictionary<ButtonElementViewModel, CompInfo>();

        private void SearchBlock_SearchStarted(object? sender, HandyControl.Data.FunctionEventArgs<string> e)
        {
            try
            {
                if (buttonDictionary.Count > 0)
                {
                    foreach (ButtonElementViewModel renderable in buttonDictionary.Keys)
                    {
                        this.ChildElementManager.RemoveElement(renderable);
                    }
                }
                buttonDictionary.Clear();

                string query = e.Info;
                //Get a list of types that inherit BaseComp in loaded assemblies
                Type[] types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => typeof(BaseCompViewModel).IsAssignableFrom(p)).ToArray();
                if (types != null && types.Length > 0)
                {
                    //get an array of all the names of the types
                    string[] names = types.Select(t => t.Name).ToArray();
                    //search for the query in the names array
                    string[] results = names.Where(n => n.Contains(query)).ToArray();
                    //remove duplicate values from results
                    List<string> temp1 = new List<string>();
                    for (int i = 0; i < results.Length; i++)
                    {
                        if (!temp1.Contains(results[i])) temp1.Add(results[i]);
                    }
                    results = temp1.ToArray();
                    //reduce results to first 5
                    if (results.Length > 5)
                    {
                        string[] temp = new string[5];
                        for (int i = 0; i < 5; i++)
                        {
                            temp[i] = results[i];
                        }
                        results = temp;
                    }

                    if (results.Length > 0)
                    {
                        foreach (string result in results)
                        {
                            ButtonElementViewModel btn = new ButtonElementViewModel();
                            btn.DisplayedText = result;
                            btn.Width = 200;
                            btn.OnButtonClicked += Btn_OnButtonClicked;

                            //get the type from the types array
                            Type? type = types.Where(t => t.Name == result).FirstOrDefault();
                            if (type != null)
                            {
                                //create a new instance of the type
                                if (type == typeof(BaseCompViewModel)) continue;
                                if (type == typeof(SearchForComp)) continue;
                                object? comp = Activator.CreateInstance(type);
                                //get the CompInfo from the type
                                CompInfo ci = (CompInfo)type.GetMethod("GetCompInfo").Invoke(comp, null);
                                //add the button and the CompInfo to the dictionary
                                buttonDictionary.Add(btn, ci);
                            }
                        }
                    }

                    if (buttonDictionary.Count > 0)
                    {
                        foreach (ButtonElementViewModel button in buttonDictionary.Keys)
                        {
                            if (!this.ChildElementManager.BottomUIItems.Contains(button)) this.ChildElementManager.AddElement(button);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CoreConsole.Log(ex);
            }
        }

        private void Btn_OnButtonClicked(object? sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                if (btn.Content is string name)
                {
                    //find the buttonelement in the dictionary with compinfo.name
                    ButtonElementViewModel? btnElement = buttonDictionary.Where((kv) => kv.Value.Name == name).FirstOrDefault().Key;
                    if (btnElement is null) return;
                    if (buttonDictionary[btnElement].ConstructorInfo != null)
                    {
                        CompInfo ci = buttonDictionary[btnElement];
                        ////TODO: Invoke constructor based on <PluginName>.cfg json file
                        ////TODO: Allow user to place the comp with MousePosition
                        if (ci.ConstructorInfo.GetParameters().Length > 0)
                        {
                            ParameterInfo[] pi = ci.ConstructorInfo.GetParameters();
                            object[] args = new object[pi.Length];
                            for (int i = 0; i < pi.Length; i++)
                            {
                                if (!(pi[i].DefaultValue is DBNull)) args[i] = pi[i].DefaultValue;
                                else
                                {
                                    if (pi[i].ParameterType == typeof(int) && pi[i].Name.ToLower() == "x")
                                        args[i] = WorkspaceViewModel.StaticSelectedDataViewModel.DataModelView.GetMouseRelPosition().X;
                                    else if (pi[i].ParameterType == typeof(int) && pi[i].Name.ToLower() == "y")
                                        args[i] = WorkspaceViewModel.StaticSelectedDataViewModel.DataModelView.GetMouseRelPosition().Y;
                                }
                            }
                            WorkspaceViewModel.compsPendingInst.Add(ci, args);
                            WorkspaceViewModel.StaticWorkspaceViewModel.AddToCanvas_OnCall(this, new EventArgs());
                            //IElement? elInst = buttonDictionary[btnElement].ConstructorInfo.Invoke(args) as IElement;
                            //DataTemplateManager.RegisterDataTemplate(elInst as IRenderable);
                            //DataViewModel.Instance.Elements.Add(elInst);
                            //DataViewModel.WPFControl.ExpandContent();
                        }
                    }
                }
            }
        }
    }
}
