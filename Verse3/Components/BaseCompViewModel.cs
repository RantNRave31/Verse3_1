using Core;
using System;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Xml.Serialization;
using Verse3.Elements;
using Newtonsoft.Json;
using static Core.Geometry2D;
using System.Text;
using Verse3.ViewModels;
using Core.Elements;
using Core.Nodes;

namespace Verse3.Components
{
    [Serializable]
    //[XmlRoot("BaseComp")]
    //[XmlType("BaseComp")]
    public abstract class BaseCompViewModel : ViewModelBase, IRenderable, IComputable
    {
        #region Data Members

        private RenderPipelineInfo renderPipelineInfo;
        protected BoundingBox boundingBox = BoundingBox.Unset;
        private Guid _id = Guid.NewGuid();
        internal BaseCompModelView elView;
        internal ChildElementManager _cEManager;

        #endregion

        #region Properties

        protected PropertiesViewModel propertiesViewModel;
        public virtual PropertiesViewModel Properties { get { if (propertiesViewModel == null) propertiesViewModel = new PropertiesViewModel<BaseCompViewModel>(this); return propertiesViewModel; } set { if (value == propertiesViewModel) return; propertiesViewModel = value; OnPropertyChanged("PropertiesViewModel"); } }
        private string _nameOverride;
        public string Name
        {
            get
            {
                CompInfo ci = GetCompInfo();
                if (_nameOverride == ci.Name || _nameOverride == null)
                {
                    return ci.Name;
                }
                else
                {
                    return _nameOverride;
                }
            }
            set
            {
                _nameOverride = value;
            }
        }

        private string _metaDataCompInfo;
        public string MetadataCompInfo
        {
            get
            {
                if (_metaDataCompInfo is null) return GetCompInfo().ToString();
                else return _metaDataCompInfo;
            }
            set
            {
                _metaDataCompInfo = value;
                try
                {
                    CompInfo ci = CompInfo.FromString(this, value);
                    if (ci.IsValid)
                    {
                        _metaDataCompInfo = ci.ToString();
                    }
                    //TODO: Try and get a CompInfo from the string and match it to the current CompInfo

                }
                catch (Exception ex)
                {
                    CoreConsole.Log(ex);
                }
            }
        }
        //[XmlIgnore]
        //[JsonIgnore]
        public ChildElementManager ChildElementManager => _cEManager;
        //[XmlIgnore]
        [JsonIgnore]
        public RenderPipelineInfo RenderPipelineInfo => renderPipelineInfo;
        //[XmlIgnore]
        [JsonIgnore]
        public IRenderView RenderView
        {
            get
            {
                return elView;
            }
            set
            {
                if (ViewType.IsAssignableFrom(value.GetType()))
                {
                    elView = value as BaseCompModelView;
                }
                else
                {
                    Exception ex = new Exception("Invalid View Type");
                    CoreConsole.Log(ex);
                }
            }
        }
        //[XmlIgnore]
        public Type ViewType => typeof(BaseCompModelView);
        //[XmlIgnore]
        public object ViewKey { get; set; }

        public Guid ID { get => _id; set => _id = value; }

        [JsonIgnore]
        public bool IsSelected { get; set; }

        public BoundingBox BoundingBox { get => boundingBox; set => SetProperty(ref boundingBox, value); }

        [JsonIgnore]
        public double X { get => boundingBox.Location.X; }

        [JsonIgnore]
        public double Y { get => boundingBox.Location.Y; }

        [JsonIgnore]
        public double Width
        {
            get => boundingBox.Size.Width;
            set
            {
                BoundingBox.Size.Width = value;
                OnPropertyChanged("Width");
            }
        }

        [JsonIgnore]
        public double Height
        {
            get => boundingBox.Size.Height;
            set
            {
                BoundingBox.Size.Height = value;
                OnPropertyChanged("Height");
            }
        }

        [JsonIgnore]
        public ElementState State { get; set; }

        //public IRenderView ElementView { get; internal set; }

        [JsonIgnore]
        public ElementState ElementState { get; set; }
        private ElementType eType = ElementType.BaseComp;
        public ElementType ElementType { get => eType; set => eType = ElementType.BaseComp; }
        //[XmlIgnore]
        [JsonIgnore]
        bool IRenderable.Visible { get; set; }

        private Brush background;
        //[XmlIgnore]
        [JsonIgnore]
        public Brush Background { get => background; set => SetProperty(ref background, value); }

        private Brush accent;
        //[XmlIgnore]
        [JsonIgnore]
        public Brush Accent { get => accent; set => SetProperty(ref accent, value); }

        //internal CompOrientation _orientation = CompOrientation.Vertical;
        //public string Orientation
        //{
        //    get => _orientation.ToString();
        //    set
        //    {
        //        if (Enum.TryParse(value, out CompOrientation orientation))
        //        {
        //            _orientation = orientation;
        //        }
        //    }
        //}


        //[XmlIgnore]
        [JsonIgnore]
        public IRenderable Parent => RenderPipelineInfo.Parent;
        //[XmlIgnore]
        [JsonIgnore]
        public ElementsLinkedList<IRenderable> Children => RenderPipelineInfo.Children;

        internal ComputationPipelineInfo computationPipelineInfo;
        //[XmlIgnore]
        //[JsonIgnore]
        public ComputationPipelineInfo ComputationPipelineInfo => computationPipelineInfo;

        //private ElementsLinkedList<INode> _nodes = new ElementsLinkedList<INode>();
        //public ElementsLinkedList<INode> Nodes => _nodes;

        public ComputableElementState ComputableElementState { get; set; } = ComputableElementState.Unset;
        //[XmlIgnore]
        [JsonIgnore]
        IRenderView IRenderable.RenderView
        {
            get => RenderView as IRenderView;
            set
            {
                if (value is BaseCompModelView)
                {
                    RenderView = value as BaseCompModelView;
                }
            }
        }

        [JsonIgnore]
        public bool RenderExpired { get; set; }
        [JsonIgnore]
        public virtual ContextMenu ContextMenu
        {
            get
            {
                ContextMenu contextMenu = new ContextMenu();

                //Delete
                MenuItem menuItem = new MenuItem();
                menuItem.Header = "Delete";
                menuItem.Click += (s, e) =>
                {
                    DataViewModel.DataModel.Elements.Remove(this);
                };
                contextMenu.Items.Add(menuItem);

                return contextMenu;
            }
        }

        #endregion

        #region Constructor and Compute

        //#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public BaseCompViewModel()
        {
            boundingBox = new BoundingBox();

            CompInfo ci = GetCompInfo();

            Accent = new SolidColorBrush(ci.Accent);
            Background = new SolidColorBrush(Colors.Gray);
        }

        public BaseCompViewModel(int x, int y)
        {
            _cEManager = new ChildElementManager(this);

            renderPipelineInfo = new RenderPipelineInfo(this);
            computationPipelineInfo = new ComputationPipelineInfo(this);

            boundingBox = new BoundingBox(x, y, 0, 0);

            CompInfo ci = GetCompInfo();

            Accent = new SolidColorBrush(ci.Accent);
            Background = new SolidColorBrush(Colors.Gray);
        }

        internal SerializationInfo _sInfo;
        internal StreamingContext _sContext;
        public BaseCompViewModel(SerializationInfo info, StreamingContext context)
        {
            _sInfo = info;
            _sContext = context;
            _cEManager = new ChildElementManager(this);

            //_cEManager = info.GetValue("ChildElementManager", typeof(ChildElementManager)) as ChildElementManager;


            //renderPipelineInfo = new RenderPipelineInfo(this);
            //computationPipelineInfo = new ComputationPipelineInfo(this);
            //computationPipelineInfo = info.GetValue("ComputationPipelineInfo", typeof(ComputationPipelineInfo)) as ComputationPipelineInfo;

            //this.boundingBox = new BoundingBox();


            ID = (Guid)info.GetValue("ID", typeof(Guid));
            Name = info.GetString("Name");
            _metaDataCompInfo = info.GetString("MetadataCompInfo");
            //CompInfo ci = this.GetCompInfo();

            //this.Accent = new SolidColorBrush(ci.Accent);
            //this.Background = new SolidColorBrush(Colors.Gray);
            //this.ElementType = (ElementType)info.GetValue("ElementType", typeof(ElementType));
            //this.State = (ElementState)info.GetValue("State", typeof(ElementState));
            boundingBox = (BoundingBox)info.GetValue("BoundingBox", typeof(BoundingBox));
            //this.IsSelected = info.GetBoolean("IsSelected");
            //this.ElementState = (ElementState)info.GetValue("ElementState", typeof(ElementState));
        }

        public abstract void Initialize();

        protected TextElementViewModel titleTextBlock = new TextElementViewModel();
        protected TextElementViewModel previewTextBlock = new TextElementViewModel();
        /// <summary>
        /// Override only if you know what you're doing
        /// </summary>
        public virtual void RenderComp()
        {
            if (Children.Count > 0)
            {
                //TODO: At every render
                //textBlock.DisplayedText = this.ElementText;
                ChildElementManager.AdjustBounds(true);
                return;
            }

            //textBlock.DisplayedText = this.ElementText;
            titleTextBlock.DisplayedText = GetCompInfo().Name;
            titleTextBlock.TextAlignment = TextAlignment.Left;
            titleTextBlock.TextRotation = 90;
            titleTextBlock.ElementType = ElementType.DisplayUIElement;
            //double h = titleTextBlock.Height;
            //titleTextBlock.Height = titleTextBlock.Width;
            //titleTextBlock.Width = h;
            ChildElementManager.AddElement(titleTextBlock);

            //this.ChildElementManager.AddElement()


            Initialize();


            previewTextBlock = new TextElementViewModel();
            previewTextBlock.TextAlignment = TextAlignment.Left;
            previewTextBlock.DisplayedText = string.Empty;
            previewTextBlock.Width = 200;
            ChildElementManager.AddElement(previewTextBlock);
            if (ComputationPipelineInfo.IOManager.PrimaryDataOutput >= 0)
            {
                IDataNode node = ComputationPipelineInfo.IOManager.DataOutputNodes[ComputationPipelineInfo.IOManager.PrimaryDataOutput];
                string primaryDataName = "Out";
                if (!string.IsNullOrEmpty(node.Name))
                {
                    primaryDataName = node.Name;
                }
                previewTextBlock.DisplayedText = primaryDataName + " = " + node.DataGoo.ToString();
            }

            //if (this.RenderView is BaseCompView)
            //{
            //    BaseCompView view = this.RenderView as BaseCompView;

            //    if (this.Width != (view.InputsList.ActualWidth + view.OutputsList.ActualWidth + view.CenterBar.Width))
            //    {
            //        this.Width = (view.InputsList.ActualWidth + view.OutputsList.ActualWidth + view.CenterBar.Width);
            //    }
            //    if (this.Height != view.MainStackPanel.ActualHeight) this.Height = view.MainStackPanel.ActualHeight;

            //    //TODO: Add Center Bar Elements (Title, Icon, etc)
            //    //this.ChildElementManager.AddElement()
            //}
            ChildElementManager.AdjustBounds(true);
        }

        public abstract void Compute();

        public virtual bool CollectData()
        {
            bool result = false;
            if (ComputationPipelineInfo.IOManager.DataInputNodes != null && ComputationPipelineInfo.IOManager.DataInputNodes.Count > 1)
            {
                result = ComputationPipelineInfo.CollectData();
                if (ComputationPipelineInfo.IOManager.PrimaryDataOutput > -1)
                {
                    if (previewTextBlock == null)
                    {
                        previewTextBlock = new TextElementViewModel();
                        previewTextBlock.TextAlignment = TextAlignment.Left;
                        ChildElementManager.AddElement(previewTextBlock);
                    }
                    IDataNode node = ComputationPipelineInfo.IOManager.DataOutputNodes[ComputationPipelineInfo.IOManager.PrimaryDataOutput];
                    string primaryDataName = "Out";
                    if (!string.IsNullOrEmpty(node.Name))
                    {
                        primaryDataName = node.Name;
                    }
                    previewTextBlock.DisplayedText = primaryDataName + " = " + node.DataGoo.ToString();
                }
            }
            return result;
        }
        public virtual void DeliverData()
        {
            if (ComputationPipelineInfo.IOManager.DataOutputNodes != null && ComputationPipelineInfo.IOManager.DataOutputNodes.Count > 0/* && computable.Nodes[0] is NodeElement*/)
            {
                ComputationPipelineInfo.DeliverData();
            }
            if (ComputationPipelineInfo.IOManager.PrimaryDataOutput >= 0)
            {
                IDataNode node = ComputationPipelineInfo.IOManager.DataOutputNodes[ComputationPipelineInfo.IOManager.PrimaryDataOutput];
                string primaryDataName = "Out";
                if (!string.IsNullOrEmpty(node.Name))
                {
                    primaryDataName = node.Name;
                }
                previewTextBlock.DisplayedText = primaryDataName + " = " + node.DataGoo.ToString();
            }
        }

        //#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        #endregion

        private StringBuilder sbLog = new StringBuilder();
        public void OnLog_Internal(EventArgData e)
        {
            sbLog.AppendLine(e.ToString());
        }

        public abstract CompInfo GetCompInfo();
        public void Dispose()
        {
            if (RenderPipelineInfo != null)
            {
                if (RenderPipelineInfo.Children != null && RenderPipelineInfo.Children.Count > 0)
                {
                    foreach (var child in RenderPipelineInfo.Children)
                    {
                        if (child != null) child.Dispose();
                    }
                }
            }
            DataViewModel.DataModel.Elements.Remove(this);
            GC.SuppressFinalize(this);
        }

        public void SetX(double x)
        {
            BoundingBox.Location.X = x;
            OnPropertyChanged("X");
        }

        public void SetY(double y)
        {
            BoundingBox.Location.Y = y;
            OnPropertyChanged("Y");
        }

        public void SetWidth(double x)
        {
            BoundingBox.Size.Width = x;
            OnPropertyChanged("Width");
        }

        public void SetHeight(double x)
        {
            BoundingBox.Size.Height = x;
            OnPropertyChanged("Height");
        }

        public void Render()
        {
            if (RenderView != null)
                RenderView.Render();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            try
            {
                info.AddValue("ID", ID);
                info.AddValue("MetadataCompInfo", MetadataCompInfo);
                CompInfo ci = GetCompInfo();
                info.AddValue("Name", ci.Name);
                info.AddValue("ComputationPipelineInfo", ComputationPipelineInfo);
                //info.AddValue("X", this.X);
                //info.AddValue("Y", this.Y);
                //info.AddValue("Width", this.Width);
                //info.AddValue("Height", this.Height);
                info.AddValue("ElementType", ElementType);
                //info.AddValue("State", this.State);
                //info.AddValue("IsSelected", this.IsSelected);
                info.AddValue("BoundingBox", BoundingBox);
                info.AddValue("ChildElementManager", ChildElementManager);
                //info.AddValue("ElementState", this.ElementState);
            }
            catch (Exception ex)
            {
                CoreConsole.Log(ex);
            }
        }

        ~BaseCompViewModel() => Dispose();
    }

    //public enum CompOrientation
    //{
    //    Horizontal,
    //    Vertical
    //}

}
