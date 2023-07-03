using Core;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using static Core.Geometry2D;
using Newtonsoft.Json;
using Core.Elements;
using Verse3.CorePresentation.Workspaces;

namespace Verse3.Elements
{
    //[Serializable]
    public abstract class BaseElement : IRenderable
    {
        #region Data Members

        [JsonIgnore]
        [IgnoreDataMember]
        private RenderPipelineInfo renderPipelineInfo;
        protected BoundingBox boundingBox = BoundingBox.Unset;
        private Guid _id = Guid.NewGuid();
        [JsonIgnore]
        internal IRenderView elView;

        #endregion

        #region Properties

        [JsonIgnore]
        [IgnoreDataMember]
        public RenderPipelineInfo RenderPipelineInfo => renderPipelineInfo;
        [JsonIgnore]
        [IgnoreDataMember]
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
                    Exception ex = new InvalidCastException();
                    CoreConsole.Log(ex);
                }
            }
        }
        [JsonIgnore]
        [IgnoreDataMember]
        public abstract Type ViewType { get; }
        [JsonIgnore]
        [IgnoreDataMember]
        public object ViewKey { get; set; }

        public Guid ID { get => _id; private set => _id = value; }

        //public bool IsSelected { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public BoundingBox BoundingBox { get => boundingBox; private set => SetProperty(ref boundingBox, value); }

        [JsonIgnore]
        [IgnoreDataMember]
        public double X { get => boundingBox.Location.X; }

        [JsonIgnore]
        [IgnoreDataMember]
        public double Y { get => boundingBox.Location.Y; }

        [JsonIgnore]
        [IgnoreDataMember]
        public double Width
        {
            get => boundingBox.Size.Width;
            set => boundingBox.Size.Width = value;
        }

        [JsonIgnore]
        [IgnoreDataMember]
        public double Height
        {
            get => boundingBox.Size.Height;
            set => boundingBox.Size.Height = value;
        }

        [JsonIgnore]
        public ElementState State { get; set; }

        //public IRenderView ElementView { get; internal set; }

        [JsonIgnore]
        public ElementState ElementState { get; set; }

        private ElementType _elementType = ElementType.UIElement;
        public virtual ElementType ElementType { get => _elementType; set => _elementType = value; }
        [JsonIgnore]
        [IgnoreDataMember]
        bool IRenderable.Visible { get; set; }
        private bool sel = false;
        [JsonIgnore]
        [IgnoreDataMember]
        public bool IsSelected { get => sel; set => sel = false; }
        [JsonIgnore]
        [IgnoreDataMember]
        public bool RenderExpired { get; set; }

        //[JsonIgnore]
        //[IgnoreDataMember]
        public IRenderable Parent => RenderPipelineInfo.Parent;

        [JsonIgnore]
        [IgnoreDataMember]
        public ElementsLinkedList<IRenderable> Children => RenderPipelineInfo.Children;

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
        public virtual PropertiesViewModel Properties { get { if (propertiesViewModel == null) propertiesViewModel = new PropertiesViewModel<BaseElement>(this); return propertiesViewModel; } set { if (value == propertiesViewModel) return; propertiesViewModel = value; OnPropertyChanged("PropertiesViewModel"); } }

        public BaseElement()
        {
            renderPipelineInfo = new RenderPipelineInfo(this);
        }

        public BaseElement(SerializationInfo info, StreamingContext context)
        {
            renderPipelineInfo = new RenderPipelineInfo(this);
            _id = (Guid)info.GetValue("ID", typeof(Guid));
            //this.boundingBox = (BoundingBox)info.GetValue("BoundingBox", typeof(BoundingBox));
            ElementType = (ElementType)info.GetValue("ElementType", typeof(ElementType));
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

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
            if (RenderPipelineInfo.Children != null && RenderPipelineInfo.Children.Count > 0)
            {
                foreach (var child in RenderPipelineInfo.Children)
                {
                    if (child != null) child.Dispose();
                }
            }
            WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataViewModel.Elements.Remove(this);
            GC.SuppressFinalize(this);
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            try
            {
                info.AddValue("ID", ID);
                //info.AddValue("X", this.X);
                //info.AddValue("Y", this.Y);
                //info.AddValue("Width", this.Width);
                //info.AddValue("Height", this.Height);
                info.AddValue("ElementType", ElementType);
                //info.AddValue("State", this.State);
                //info.AddValue("IsSelected", this.IsSelected);
                //info.AddValue("BoundingBox", this.BoundingBox);
                //info.AddValue("ElementState", this.ElementState);
                //info.AddValue("Parent", this.Parent);
                //info.AddValue("Children", this.Children);
            }
            catch (Exception ex)
            {
                CoreConsole.Log(ex);
            }
        }

        ~BaseElement() => Dispose();
    }

}
