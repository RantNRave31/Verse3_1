using Core;
using Verse3.Collections.Generic;
using Verse3.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Verse3.Components;
using Verse3.Elements;
using Verse3.Tools;
using Core.Assemblies;
using Verse3.Assemblies;
using Core.Elements;

namespace Verse3
{
    public class MainWindowViewModel
        : ViewModelBase
    {
        #region MainWindow
        public static AppDomain domain_;
        public static MainWindowModelView ActiveMain { get => MainApplication._mainWindowModelView; }
        public ObservableCollection<MainWindowModelView> MainWindowModelViews { get; set; }
        public MainWindowModelView SelectedMainWindowModelView { get; set; }
        public ObservableCollection<MainWindowViewModel> MainWindowViewModels { get; set; }
        MainWindowViewModel _selectedMainWindowViewModel;
        public MainWindowViewModel SelectedMainWindowViewModel { get { return  _selectedMainWindowViewModel; } set { if (value == _selectedMainWindowViewModel) return; _selectedMainWindowViewModel = value; OnPropertyChanged(); } }
        public MainWindowModelView MainWindowModelView { get; set; }
        #endregion
        #region Data Models and Views
        private int childFormNumber = 0;
        public ObservableCollection<DataViewModel> DataViewModels { get; set; }
        private DataModelView _selectedDataModelView;
        public DataModelView SelectedDataModelView
        {
            get
            {
                return _selectedDataModelView;
            }
            set
            {
                _selectedDataModelView = value;
            }
        }
        DataViewModel _selectedDataViewModel;
        public DataViewModel SelectedDataViewModel { get { return _selectedDataViewModel; } set { if (value == _selectedDataViewModel) return; _selectedDataViewModel = value; OnPropertyChanged(); } }
        #endregion
        #region Comps Pending
        public static Dictionary<CompInfo, object[]> compsPendingInst = new Dictionary<CompInfo, object[]>();
        public static List<CompInfo> compsPendingAddToArsenal = new List<CompInfo>();
        public static List<BezierElementViewModel> connectionsPending = new List<BezierElementViewModel>();
        #endregion
        #region Assemblies and Components
        public AssemblyManagerViewModel AssemblyManager { get; set; }
        private Dictionary<string, CompInfo> _loadedLibraries = new Dictionary<string, CompInfo>();
        public Dictionary<string, CompInfo> LoadedLibraries { get => _loadedLibraries; private set => _loadedLibraries = value; }
        private Dictionary<string, IEnumerable<IElement>> Elements = new Dictionary<string, IEnumerable<IElement>>();
        #endregion
        #region Instrumentation
        public string framesPerSecond;
        public string FramesPerSecond { get { return framesPerSecond; } set { if (value == framesPerSecond) return; framesPerSecond = value; OnPropertyChanged(); } }
        public string status;
        public string Status { get { return status; } set { if (value == status) return; status = value; OnPropertyChanged(); } }
        #endregion
        #region Tools
        public ObservableDictionary<string, CompInfo> Arsenal { get; set; }
        public ToolPanelViewModel ToolPanel { get; set; }
        #endregion
        public MainWindowViewModel(string displayName)
            : base(displayName)
        {
            MainWindowViewModels = new ObservableCollection<MainWindowViewModel>();
            AssemblyManager = new AssemblyManagerViewModel("Library Manager");
            Arsenal = new ObservableDictionary<string, CompInfo>();
            ToolPanel = new ToolPanelViewModel("ToolPanel");
        }
        public void ShowNewForm(object sender, EventArgs e)
        {
            SelectedMainWindowViewModel = new MainWindowViewModel("Show New Form");
            MainWindowViewModels.Add(SelectedMainWindowViewModel);
            if (Debugger.IsAttached)
            {
            }
            else
            {

            }
        }
        public void HotLoadLibraryFolder(string path)
        {
            if (Directory.Exists(path))
            {
                foreach (string file in Directory.GetFiles(path))
                {
                    if (file.EndsWith(".verse"))
                    {
                        if (!LoadedLibraries.ContainsKey(file))
                        {
                            HotLoadLibrary(file);
                        }
                    }
                }
            }
        }
        public void HotLoadLibrary(string path)
        {
            try
            {
                if (SelectedDataModelView == null) return;
                if (File.Exists(path))
                {
                    if (!LoadedLibraries.ContainsKey(path))
                    {
                        var es = AssemblyManager.LoadLibrary(path);
                        if (!Elements.ContainsValue(es))
                        {
                            Elements.Add(path, es);
                            LoadElements(path, es);

                        }
                    }
                    else
                    {
                        if (Elements.ContainsKey(path))
                        {
                            var es = AssemblyManager.LoadLibrary(path);
                            Elements[path] = es;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //throw;
                //CoreConsole.Log(ex);
                CoreConsole.Log(ex);
            }
        }
        private void LoadElements(string path, IEnumerable<IElement> es)
        {
            if (es != null)
            {
                foreach (IElement el in es)
                {
                    if (el is IRenderable)
                    {
                        DataTemplateManager.RegisterDataTemplate(el as IRenderable);
                        //TODO: Check for other types of constructors
                        //TODO: Get LibraryInfo
                        MethodInfo mi = el.GetType().GetRuntimeMethod("GetCompInfo", new Type[] { });
                        if (mi != null)
                        {
                            if (mi.ReturnType == typeof(CompInfo))
                            {
                                CompInfo compInfo = (CompInfo)mi.Invoke(el, null);
                                if (compInfo.ConstructorInfo != null)
                                {
                                    if (!LoadedLibraries.ContainsValue(compInfo))
                                    {
                                        if (compInfo.Group == "_CanvasElements" &&
                                            compInfo.Tab == "_CanvasElements")
                                        {
                                            if (compInfo.Name == "Extent")
                                            {
                                                LoadedLibraries.Add(path + "._" + compInfo.Name, compInfo);
                                                //Create two instances of CanvasExtent Element to make the initial/min canvas size 10000x10000
                                                int x = -5000;
                                                int y = -5000;
                                                IElement elInst = compInfo.ConstructorInfo.Invoke(new object[] { x, y }) as IElement;
                                                DataViewModel.DataModel.Elements.Add(elInst);
                                                x = 9990;
                                                y = 9990;
                                                elInst = compInfo.ConstructorInfo.Invoke(new object[] { x, y }) as IElement;
                                                DataModel.Instance.Elements.Add(elInst);
                                                SelectedDataModelView.ExpandContent();
                                                SelectedDataModelView.InfiniteCanvasControl1.AnimatedSnapTo(new System.Windows.Point(5000.0, 5000.0));
                                                continue;
                                            }
                                            else if (compInfo.Name == "Search")
                                            {
                                                SelectedDataViewModel.SearchBarCompInfo = compInfo;
                                                LoadedLibraries.Add(path + "._" + compInfo.Name, compInfo);
                                                continue;
                                            }
                                        }
                                        else if (compInfo.Group == "`" && compInfo.Tab == "`")
                                        {
                                            if (compInfo.Name == "Callback")
                                            {
                                                LoadedLibraries.Add(path + "._" + compInfo.Name, compInfo);
                                                continue;
                                            }
                                        }
                                        //TODO: Check for validity / scan library info
                                        AddToArsenal(compInfo);
                                        LoadedLibraries.Add(path + "._" + compInfo.Name, compInfo);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        public void AddToCanvas_OnCall(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                Button btn = sender as Button;
                if (btn.Tag != null && btn.Tag is CompInfo)
                {
                    if (SelectedDataModelView != null)
                    {
                        CompInfo ci = (CompInfo)btn.Tag;
                        if (ci.ConstructorInfo != null)
                        {
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
                                            args[i] = SelectedDataModelView.GetMouseRelPosition().X;
                                        else if (pi[i].ParameterType == typeof(int) && pi[i].Name.ToLower() == "y")
                                            args[i] = SelectedDataModelView.GetMouseRelPosition().Y;
                                    }
                                }
                                IElement elInst = ci.ConstructorInfo.Invoke(args) as IElement;
                                DataViewModel.DataModel.Elements.Add(elInst);
                                if (elInst is BaseCompViewModel) ComputationCore.Compute(elInst as BaseCompViewModel);
                                //DataViewModel.WPFControl.ExpandContent();
                            }
                            else
                            {
                                //throw new Exception("Constructor parameters not provided");
                                //DataViewModel.WPFControl.ExpandContent();
                            }
                            //IElement elInst = ci.ConstructorInfo.Invoke(new object[] { x, y, w, h }) as IElement;
                            //DataModel.Instance.Elements.Add(elInst);
                            //DataViewModel.WPFControl.ExpandContent();
                        }
                    }
                }
            }
            //else if (sender is BaseComp shell)
            //{
            //    if (DataViewModel.WPFControl != null)
            //    {
            //        CompInfo ci = shell.GetCompInfo();
            //        ci = FindInArsenal(ci, false);
            //        ConstructorInfo ctorInfo = GetDeserializationConstructor(ci);
            //        if (ctorInfo != null)
            //        {
            //            ////TODO: Invoke constructor based on <PluginName>.cfg json file
            //            ////TODO: Allow user to place the comp with MousePosition
            //            if (ctorInfo.GetParameters().Length > 0)
            //            {
            //                ParameterInfo[] pi = ctorInfo.GetParameters();
            //                object[] args = new object[pi.Length];
            //                for (int i = 0; i < pi.Length; i++)
            //                {
            //                    if (!(pi[i].DefaultValue is DBNull)) args[i] = pi[i].DefaultValue;
            //                    else
            //                    {
            //                        if (pi[i].ParameterType == typeof(int) && pi[i].Name.ToLower() == "x")
            //                            args[i] = shell.X;
            //                        //args[i] = shell._info;
            //                        //args[i] = InfiniteCanvasWPFControl.GetMouseRelPosition().X;
            //                        else if (pi[i].ParameterType == typeof(int) && pi[i].Name.ToLower() == "y")
            //                            args[i] = shell.Y;
            //                        //args[i] = shell._context;
            //                        //args[i] = InfiniteCanvasWPFControl.GetMouseRelPosition().Y;
            //                    }
            //                }
            //                IElement elInst = ci.ConstructorInfo.Invoke(args) as IElement;
            //                DataViewModel.Instance.Elements.Add(elInst);
            //                if (elInst is BaseComp) ComputationCore.Compute(elInst as BaseComp);
            //                //DataViewModel.WPFControl.ExpandContent();
            //            }
            //            else
            //            {
            //                //throw new Exception("Constructor parameters not provided");
            //                //DataViewModel.WPFControl.ExpandContent();
            //            }
            //            //IElement elInst = ci.ConstructorInfo.Invoke(new object[] { x, y, w, h }) as IElement;
            //            //DataModel.Instance.Elements.Add(elInst);
            //            //DataViewModel.WPFControl.ExpandContent();
            //        }
            //    }
            //}
            else
            {
                if (MainWindowViewModel.compsPendingInst.Count > 0)
                {
                    try
                    {
                        foreach (CompInfo compInfo in MainWindowViewModel.compsPendingInst.Keys)
                        {
                            if (compInfo.ConstructorInfo != null)
                            {
                                BaseCompViewModel elInst = compInfo.ConstructorInfo.Invoke(MainWindowViewModel.compsPendingInst[compInfo]) as BaseCompViewModel;
                                DataTemplateManager.RegisterDataTemplate(elInst);
                                DataViewModel.DataModel.Elements.Add(elInst);
                                //EditorForm.compsPendingInst.Remove(compInfo);
                                ComputationCore.Compute(elInst);
                            }
                        }
                        MainWindowViewModel.compsPendingInst.Clear();
                    }
                    catch (Exception ex)
                    {

                        CoreConsole.Log(ex);
                    }
                }
                if (MainWindowViewModel.connectionsPending.Count > 0)
                {
                    try
                    {
                        foreach (BezierElementViewModel b in MainWindowViewModel.connectionsPending)
                        {
                            DataTemplateManager.RegisterDataTemplate(b);
                            DataViewModel.DataModel.Elements.Add(b);
                            //EditorForm.connectionsPending.Remove(b);
                            b.RedrawBezier(b.Origin, b.Destination);
                        }
                        MainWindowViewModel.connectionsPending.Clear();
                    }
                    catch (Exception ex)
                    {

                        CoreConsole.Log(ex);
                    }
                }
                if (MainWindowViewModel.compsPendingAddToArsenal.Count > 0)
                {
                    try
                    {
                        foreach (CompInfo compInfo in MainWindowViewModel.compsPendingAddToArsenal)
                        {
                            AddToArsenal(compInfo);
                            MainWindowViewModel.compsPendingAddToArsenal.Remove(compInfo);
                        }
                        MainWindowViewModel.compsPendingAddToArsenal.Clear();
                    }
                    catch (Exception ex)
                    {

                        CoreConsole.Log(ex);
                    }
                }
            }
        }
        public void AddToArsenal(CompInfo compInfo)
        {
            if ((compInfo.ConstructorInfo != null) &&
                (compInfo.Name != String.Empty) &&
                (compInfo.Group != String.Empty) &&
                (compInfo.Tab != String.Empty))
            {
                /*                TabPage tp = new TabPage(compInfo.Tab);
                                tp.SuspendLayout();
                                FlowLayoutPanel flp = new FlowLayoutPanel();
                                flp.SuspendLayout();
                                GroupBox gb = new GroupBox();
                                gb.SuspendLayout();
                                FlowLayoutPanel flp1 = new FlowLayoutPanel();
                                flp1.SuspendLayout();
                                Button btn = new Button();
                                btn.SuspendLayout();
                                if (this.Tabs.ContainsKey(compInfo.Tab))
                                {
                                    tp = this.Tabs[compInfo.Tab];

                                    if (tp.Controls.Count == 1) flp = tp.Controls[0] as FlowLayoutPanel;
                                    //TODO: LOG else return;
                                }
                                else
                                {
                                    //tp.Location = new System.Drawing.Point(4, 24);
                                    tp.Name = "Tab" + EditorModelView.tabControl1.Items.Count.ToString();
                                    //tp.AutoScroll = true;
                                    //tp.HorizontalScroll.Enabled = true;
                                    //tp.HorizontalScroll.Visible = true;
                                    //tp.Size = new System.Drawing.Size(1005, 116);
                                    tp.TabIndex = 0;
                                    tp.Text = compInfo.Tab;
                                    tp.UseVisualStyleBackColor = true;

                                    EditorModelView.tabControl1.Items.Add(tp);
                                    this.Tabs.Add(compInfo.Tab, tp);

                                    if (tp.Controls.Count == 0)
                                    {
                                        flp.Dock = System.Windows.Forms.DockStyle.Fill;
                                        flp.FlowDirection = FlowDirection.LeftToRight;
                                        flp.Margin = new System.Windows.Forms.Padding(0);
                                        flp.Name = tp.Name + "_FLP";
                                        flp.MaximumSize = new System.Drawing.Size(0, 125);
                                        flp.WrapContents = false;
                                        flp.AutoScroll = true;
                                        //flp.HorizontalScroll.Enabled = true;
                                        //flp.HorizontalScroll.Visible = true;
                                        //flp.Size = new System.Drawing.Size(1005, 116);
                                        flp.TabIndex = 0;

                                        tp.Controls.Add(flp);
                                    }
                                }
                                if (this.Groups.ContainsKey(compInfo.Tab + ".." + compInfo.Group))
                                {
                                    gb = this.Groups[compInfo.Tab + ".." + compInfo.Group];

                                    if (gb.Controls.Count == 1) flp1 = gb.Controls[0] as FlowLayoutPanel;
                                }
                                else
                                {
                                    gb.AutoSize = true;
                                    gb.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                                    //gb.Location = new System.Drawing.Point(3, 3);
                                    gb.MinimumSize = new System.Drawing.Size(100, 100);
                                    gb.MaximumSize = new System.Drawing.Size(350, 100);
                                    gb.Name = tp.Name + "_GRP" + flp.Controls.Count.ToString();
                                    gb.Padding = new System.Windows.Forms.Padding(0);
                                    //gb.Size = new System.Drawing.Size(100, 100);
                                    gb.TabIndex = 0;
                                    gb.TabStop = false;
                                    gb.Text = compInfo.Group;

                                    flp.Controls.Add(gb);
                                    this.Groups.Add((compInfo.Tab + ".." + compInfo.Group), gb);

                                    if (gb.Controls.Count == 0)
                                    {
                                        flp1.Dock = System.Windows.Forms.DockStyle.Fill;
                                        flp1.FlowDirection = FlowDirection.LeftToRight;
                                        flp1.MaximumSize = new System.Drawing.Size(0, 100);
                                        flp1.AutoScroll = true;
                                        flp1.AutoSize = true;
                                        flp1.AutoSizeMode = AutoSizeMode.GrowOnly;
                                        flp1.WrapContents = true;
                                        //flp1.Location = new System.Drawing.Point(0, 16);
                                        flp1.Margin = new System.Windows.Forms.Padding(0);
                                        flp1.Name = gb.Name + "_FLP";
                                        //flp1.Size = new System.Drawing.Size(100, 84);
                                        flp1.TabIndex = 0;

                                        gb.Controls.Add(flp1);
                                    }
                                }
                                if (!this.Buttons.ContainsKey(compInfo.Tab + ".." + compInfo.Group + ".." + compInfo.Name))
                                {
                                    btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
                                    btn.Name = gb.Name + "_BTN" + this.Buttons.Count.ToString();
                                    btn.MinimumSize = new System.Drawing.Size(30, 30);
                                    btn.MaximumSize = new System.Drawing.Size(0, 30);
                                    btn.Size = new System.Drawing.Size(30, 30);
                                    btn.Text = compInfo.Name;
                                    btn.AutoSize = true;
                                    btn.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                                    btn.TabIndex = 0;
                                    btn.UseVisualStyleBackColor = true;
                                    btn.Tag = compInfo;

                                    btn.Click += AddToCanvas_OnCall;

                                    flp1.Controls.Add(btn);
                                    this.Buttons.Add((compInfo.Tab + ".." + compInfo.Group + ".." + compInfo.Name), btn);
                                }

                                //if (gb.Width > 300 && gb.Width < 350)
                                //{
                                //    gb.MaximumSize = new System.Drawing.Size(350, 100);
                                //}
                                //else if (gb.Width >= 350 && gb.Width < 550)
                                //{
                                //    gb.MaximumSize = new System.Drawing.Size(550, 100);
                                //}
                                //else if (gb.Width >= 550)
                                //{
                                //    gb.MaximumSize = new System.Drawing.Size()
                                //}
                                btn.ResumeLayout(false);
                                flp1.ResumeLayout(false);
                                gb.ResumeLayout(false);
                                flp.ResumeLayout(false);
                                tp.ResumeLayout(false);
                */
                if (!Arsenal.ContainsKey(compInfo.Name))
                {
                    Arsenal.Add(compInfo.Name, compInfo);
                    ToolCategoryViewModel toolCategory;
                    ToolSubcategoryViewModel toolSubcategory;
                    if (ToolPanel.ToolCategories.Count(c => c.Name == compInfo.Tab) <= 0)
                    {
                        ToolPanel.ToolCategories.Add(toolCategory = new ToolCategoryViewModel(compInfo.Tab));
                    }
                    else
                    {
                        toolCategory = ToolPanel.ToolCategories.Where(c => c.Name == compInfo.Tab).FirstOrDefault();
                    }
                    if (toolCategory.ToolSubcategories.Count(c => c.Name == compInfo.Group) <= 0)
                    {
                        toolCategory.ToolSubcategories.Add(toolSubcategory = new ToolSubcategoryViewModel(compInfo.Group));
                    }
                    else
                    {
                        toolSubcategory = toolCategory.ToolSubcategories.Where(c => c.Name == compInfo.Group).FirstOrDefault();
                    }
                    if (toolSubcategory.Tools.Count(c => c.Name == compInfo.Name) <= 0)
                    {
                        toolSubcategory.Tools.Add(new ToolViewModel(compInfo.Group, compInfo.Name) { CompInfo = compInfo});
                    }
                }
            }
        }
        public CompInfo FindInArsenal(CompInfo compInfo, bool showToUser = false)
        {
            foreach (CompInfo comp in Arsenal)
            {
                if (comp.Name == compInfo.Name &&
                    comp.Group == compInfo.Group &&
                    comp.Tab == compInfo.Tab &&
                    comp.Author == compInfo.Author &&
                    comp.Version == compInfo.Version &&
                    comp.ConstructorInfo != null)
                {
                    return comp;
                }
            }
            return default;
        }
        public void LoadLibraries(object sender, EventArgs e)
        {
            HotLoadLibraryFolder(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Verse3\\Libraries\\"));
        }
    }
}
