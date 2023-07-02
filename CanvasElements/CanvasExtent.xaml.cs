using Core;
using Core.Elements;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows.Controls;
using Verse3.Components;
using static Core.Geometry2D;

namespace CanvasElements
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class CanvasExtent : UserControl, IRenderView
    {
        public CanvasExtent()
        {
            InitializeComponent();
        }
        
        public IRenderable Element => null;

        public void Render()
        {
        }
    }

    
    public class CanvasExtentElement : IRenderable
    {
        #region Data Members

        private RenderPipelineInfo renderPipelineInfo;
        protected BoundingBox boundingBox = BoundingBox.Unset;
        private Guid _id = Guid.NewGuid();
        internal IRenderView? elView;

        #endregion

        #region Properties

        public RenderPipelineInfo RenderPipelineInfo => renderPipelineInfo;
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
                    elView = value;
                }
                else
                {
                    Exception ex = new Exception("Invalid View Type");
                    CoreConsole.Log(ex);
                }
            }
        }
        public Type ViewType { get => typeof(CanvasExtent); }
        public object ViewKey { get; set; }

        public Guid ID { get => _id; private set => _id = value; }

        private bool sel = false;
        public bool IsSelected { get => sel; set => sel = false; }

        public BoundingBox BoundingBox { get => boundingBox; private set => SetProperty(ref boundingBox, value); }

        public double X { get => boundingBox.Location.X; }

        public double Y { get => boundingBox.Location.Y; }

        public double Width
        {
            get => boundingBox.Size.Width;
            set => boundingBox.Size.Width = value;
        }

        public double Height
        {
            get => boundingBox.Size.Height;
            set => boundingBox.Size.Height = value;
        }

        public ElementState State { get; set; }

        public ElementState ElementState { get; set; }
        public ElementType ElementType { get => ElementType.CanvasElement; set => ElementType = ElementType.CanvasElement; }
        bool IRenderable.Visible { get; set; }

        public IRenderable Parent => RenderPipelineInfo.Parent;
        public ElementsLinkedList<IRenderable> Children => RenderPipelineInfo.Children;
        public bool RenderExpired { get; set; }

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

        #endregion

        protected PropertiesViewModel propertiesViewModel;
        public virtual PropertiesViewModel Properties { get { if (propertiesViewModel == null) propertiesViewModel = new PropertiesViewModel<CanvasExtentElement>(this); return propertiesViewModel; } set { if (value == propertiesViewModel) return; propertiesViewModel = value; OnPropertyChanged("PropertiesViewModel"); } }
        public CanvasExtentElement() : base()
        {
        }

        public CanvasExtentElement(int x, int y) : base()
        {
            renderPipelineInfo = new RenderPipelineInfo(this);
            this.boundingBox = new BoundingBox(x, y, 10, 10);
        }

        public CompInfo GetCompInfo() => new CompInfo(this, "Extent", "_CanvasElements", "_CanvasElements");

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }

        #endregion

        public void Dispose()
        {
            if (this.RenderPipelineInfo.Children != null && this.RenderPipelineInfo.Children.Count > 0)
            {
                foreach (var child in this.RenderPipelineInfo.Children)
                {
                    if (child != null) child.Dispose();
                }
            }
            GC.SuppressFinalize(this);
        }
        ~CanvasExtentElement() => Dispose();
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }
    }
}