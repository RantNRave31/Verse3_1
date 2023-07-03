using Core;
using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows.Threading;
using Verse3.Elements;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using Verse3.Components;
using Verse3.Nodes;
using Core.Elements;
using Core.Nodes;
using Verse3.Collections.Generic;
using Verse3.CorePresentation.Workspaces;

namespace Verse3
{

    /// <summary>
    /// A simple example of a data-model.  
    /// The purpose of this data-model is to share display data between the main window and overview window.
    /// </summary>
    //////[DataContract]
    [Serializable]
    ////[XmlRoot("DataViewModel")]
    ////[XmlType("DataViewModel")]
    public class DataViewModel : DataModel, ISerializable
    {
        ////[XmlIgnore]
        [JsonIgnore]
        private DataModelView dataModelView { get; set; }
        public DataModelView DataModelView { get { return dataModelView; } set { if (value == dataModelView) return; dataModelView = value; OnPropertyChanged("DataModelView"); } }
        ////[XmlIgnore]
        private IElement selectedElement;
        [JsonIgnore]
        public IElement SelectedElement { get { return selectedElement; } set { if (value == selectedElement) return; selectedElement = value; OnPropertyChanged("SelectedElement"); OnPropertyChanged("SelectedElementProperties"); } }
        private INode selectedNode;
        [JsonIgnore]
        public INode SelectedNode { get { return selectedNode; } set { if (value == selectedNode) return; selectedNode = value; OnPropertyChanged("SelectedNode"); } }
        ////[XmlIgnore]
        [JsonIgnore]
        public IConnection SelectedConnection { get; internal set; }

        private Dispatcher dispatcher = null;
        internal CompInfo SearchBarCompInfo;

        [JsonIgnore]
        ////[XmlIgnore]
        public Dispatcher Dispatcher { get => dispatcher; }

        //[XmlElement]
        [JsonProperty("Comps")]
        public ElementsLinkedList<BaseCompViewModel> Comps
        {
            get
            {
                ElementsLinkedList<IElement> _elementsBuffer = base.elements;
                ElementsLinkedList<BaseCompViewModel> comps = new ElementsLinkedList<BaseCompViewModel>();
                if (_elementsBuffer.Count > 0)
                {
                    foreach (IElement element in _elementsBuffer)
                    {
                        if (element is BaseCompViewModel comp)
                        {
                            comps.Add(/*new ShellComp(*/comp);
                        }
                    }
                }
                return comps;
            }
            set
            {
                if (value != null && value.Count > 0)
                {
                    foreach (BaseCompViewModel comp in value)
                    {
                        try
                        {
                            //Main_Verse3.ActiveMain.ActiveEditor.AddToCanvas_OnCall(comp, new EventArgs());
                            base.Elements.Add(comp);
                        }
                        catch (Exception ex)
                        {
                            CoreConsole.Log(ex);
                            //throw ex;
                        }
                    }
                }
            }
        }
        //[JsonProperty("Nodes")]
        [JsonIgnore]
        internal ElementsLinkedList<INode> Nodes
        {
            get
            {
                ElementsLinkedList<IElement> _elementsBuffer = base.elements;
                ElementsLinkedList<INode> comps = new ElementsLinkedList<INode>();
                if (_elementsBuffer.Count > 0)
                {
                    foreach (IElement element in _elementsBuffer)
                    {
                        if (element is INode node)
                        {
                            comps.Add(node);
                        }
                    }
                }
                return comps;
            }
            set
            {
                if (value != null && value.Count > 0)
                {
                    foreach (INode comp in value)
                    {
                        //base.Elements.Add(comp);
                    }
                }
            }
        }
        [JsonProperty("Connections")]
        public ElementsLinkedList<BezierElementViewModel> Connections
        {
            get
            {
                ElementsLinkedList<IElement> _elementsBuffer = base.elements;
                ElementsLinkedList<BezierElementViewModel> comps = new ElementsLinkedList<BezierElementViewModel>();
                if (_elementsBuffer.Count > 0)
                {
                    foreach (IElement element in _elementsBuffer)
                    {
                        if (element is BezierElementViewModel)
                        {
                            comps.Add((BezierElementViewModel)element);
                        }
                    }
                }
                return comps;
            }
            set
            {
                if (value != null && value.Count > 0)
                {
                    foreach (BezierElementViewModel comp in value)
                    {
                        base.Elements.Add(comp);
                    }
                }
            }
        }

        //protected static DataViewModel instance = new DataViewModel();
        ////[XmlIgnore]

        //public static void AddElement(IElement e)
        //{
        //    try
        //    {
        //        Action addElement = () =>
        //        {
        //            DataViewModel.Instance.Elements.Add(e);
        //        };
        //        if (dispatcher != null)
        //        {
        //            dispatcher.BeginInvoke(addElement);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CoreConsole.Log(ex);
        //    }
        //}
        public DataViewModel() : base()
        {
            dispatcher = Dispatcher.CurrentDispatcher;
        }

        public DataViewModel(SerializationInfo info, StreamingContext context)/* : base(info, context)*/
        {
             dispatcher = Dispatcher.CurrentDispatcher;

            if (info is null) throw new NullReferenceException("Invalid SerializationInfo");
            //this.elements = (ElementsLinkedList<IElement>)info.GetValue("elements", typeof(ElementsLinkedList<IElement>));
            //TODO: Add Comps, Elements and Connections from serialization info
            //this.Comps = (ElementsLinkedList<BaseComp>)info.GetValue("Comps", typeof(ElementsLinkedList<BaseComp>));
            //this.Elements = (ElementsLinkedList<BaseElement>)info.GetValue("Elements", typeof(ElementsLinkedList<BaseElement>));
            //this.Connections = (ElementsLinkedList<BezierElement>)info.GetValue("Connections", typeof(ElementsLinkedList<BezierElement>));


            ElementsLinkedList<BaseCompViewModel> comps = (ElementsLinkedList<BaseCompViewModel>)info.GetValue("Comps", typeof(ElementsLinkedList<BaseCompViewModel>));
            ElementsLinkedList<BezierElementViewModel> connections = (ElementsLinkedList<BezierElementViewModel>)info.GetValue("Connections", typeof(ElementsLinkedList<BezierElementViewModel>));


            //TODO: CONFIRM BEFORE CLEARING
            //DataViewModel.Instance.Elements.Clear();


            if (comps != null && comps.Count > 0)
            {
                foreach (BaseCompViewModel shell in comps)
                {
                    try
                    {
                        if (shell._sInfo != null)
                        {
                            if (DataModelView != null)
                            {
                                CompInfo ci = shell.GetCompInfo();
                                ci = WorkspaceViewModel.StaticWorkspaceViewModel.FindInArsenal(ci, false);
                                ConstructorInfo ctorInfo = GetDeserializationCtor(ci);
                                if (ctorInfo != null)
                                {
                                    ////TODO: Invoke constructor based on <PluginName>.cfg json file
                                    ////TODO: Allow user to place the comp with MousePosition
                                    if (ctorInfo.GetParameters().Length > 0)
                                    {
                                        ParameterInfo[] pi = ctorInfo.GetParameters();
                                        object[] args = new object[pi.Length];
                                        for (int i = 0; i < pi.Length; i++)
                                        {
                                            if (!(pi[i].DefaultValue is DBNull)) args[i] = pi[i].DefaultValue;
                                            else
                                            {
                                                if (pi[i].ParameterType == typeof(int) && pi[i].Name.ToLower() == "info")
                                                    args[i] = shell._sInfo;
                                                //args[i] = shell.X;
                                                //args[i] = InfiniteCanvasWPFControl.GetMouseRelPosition().X;
                                                else if (pi[i].ParameterType == typeof(int) && pi[i].Name.ToLower() == "context")
                                                    args[i] = shell._sContext;
                                                //args[i] = shell.Y;
                                                //args[i] = InfiniteCanvasWPFControl.GetMouseRelPosition().Y;
                                            }
                                        }
                                        IElement elInst = ci.ConstructorInfo.Invoke(args) as IElement;
                                        this.Elements.Add(elInst);
                                        //if (elInst is BaseComp) ComputationCore.Compute(elInst as BaseComp);
                                        //DataViewModel.WPFControl.ExpandContent();
                                    }
                                    else
                                    {
                                        throw new Exception("Constructor parameters not valid");
                                        //DataViewModel.WPFControl.ExpandContent();
                                    }
                                }
                            }
                        }

                        this.Elements.Clear();
                    }
                    catch (Exception ex)
                    {
                        CoreConsole.Log(ex);
                        //throw ex;
                    }
                }
            }

            if (DataModelView != null)
            {
                DataModelView.ExpandContent();
            }

            RenderPipeline.Render(this.Elements);
        }

        internal static ConstructorInfo GetDeserializationCtor(CompInfo ci)
        {
            Type t = ci.ConstructorInfo.DeclaringType;
            ConstructorInfo Dctor = t.BaseType.GetConstructor(new Type[] { typeof(SerializationInfo), typeof(StreamingContext) });
            if (Dctor != null)
            {
                return Dctor;
            }
            else return null;
        }

        public void InitDataViewModel(DataModelView c)
        {
            if (DataModelView == null)
                DataModelView = c;
            //if (Program.Dispatcher != null && dispatcher == null)
            //    dispatcher = Program.Dispatcher;


            //TODO Properly Load all available plugins            

            //TODO: Open a file here!!!!

            //
            // TODO: Populate the data model with file data
            //
        }
        public IConnection CreateConnection(INode start, INode end = default)
        {
            if (end == default)
            {
                end = MousePositionNode.Instance;
            }
            BezierElementViewModel bezier = new BezierElementViewModel(start, end);
            DataTemplateManager.RegisterDataTemplate(bezier as IRenderable);
            this.Elements.Add(bezier);
            //start.Connections.Add(bezier);
            //end.Connections.Add(bezier);
            return bezier;
        }
        
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            //TODO: Add Comps, Elements and Connections to serialization info
            info.AddValue("Comps", this.Comps);
            //info.AddValue("Elements", this.Elements);
            info.AddValue("Connections", this.Connections);

            //info.AddValue("elements", this.elements);
            //info.AddValue("contentScale", this.contentScale);
            //info.AddValue("contentOffsetX", this.contentOffsetX);
            //info.AddValue("contentOffsetY", this.contentOffsetY);
            //info.AddValue("contentWidth", this.contentWidth);
            //info.AddValue("contentHeight", this.contentHeight);
            //info.AddValue("contentViewportWidth", this.contentViewportWidth);
            //info.AddValue("contentViewportHeight", this.contentViewportHeight);
        }
    }

}
