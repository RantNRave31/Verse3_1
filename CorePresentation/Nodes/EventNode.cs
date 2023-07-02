using Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Verse3.Elements;
using Newtonsoft.Json;
using static Core.Geometry2D;
using System.Text;
using Verse3.Components;
using Core.Elements;
using Core.Nodes;

namespace Verse3.Nodes
{
    [Serializable] //READ ONLY SERIALIZATION
    public abstract class EventNode : IRenderable, IEventNode
    {
        #region Data Members

        private ElementsLinkedList<IConnection> connections = new ElementsLinkedList<IConnection>();
        private RenderPipelineInfo renderPipelineInfo;
        protected BoundingBox boundingBox = BoundingBox.Unset;
        private Guid _id = Guid.NewGuid();
        internal IRenderView elView;
        //public ComputableElementState ComputableElementState { get; set; }

        #endregion

        #region Properties

        //public object DisplayedText { get => displayedText; set => SetProperty(ref displayedText, value); }

        [JsonIgnore]
        [IgnoreDataMember]
        IElement INode.Parent
        {
            get => RenderPipelineInfo.Parent;
            set
            {
                if (value is IRenderable)
                    RenderPipelineInfo.Parent = value as IRenderable;
                //else
                //    this.RenderPipelineInfo.Parent = null;
            }
        }

        [JsonIgnore]
        [IgnoreDataMember]
        public ElementsLinkedList<IConnection> Connections => connections;

        [JsonIgnore]
        [IgnoreDataMember]
        public ElementsLinkedList<BezierElementViewModel> BezierElements
        {
            get
            {
                ElementsLinkedList<BezierElementViewModel> bezierElements = new ElementsLinkedList<BezierElementViewModel>();
                foreach (IConnection connection in Connections)
                {
                    if (connection is BezierElementViewModel)
                    {
                        bezierElements.Add(connection as BezierElementViewModel);
                    }
                }
                return bezierElements;
            }
            set
            {
                if (value != null && value.Count > 0)
                {
                    foreach (BezierElementViewModel bezierElement in value)
                    {
                        //if (!this.Connections.Contains(bezierElement))
                        //{
                        //    this.Connections.Add(bezierElement);
                        //}
                    }
                }
            }
        }

        private NodeType _nodeType = NodeType.Unset;
        [JsonIgnore]
        [IgnoreDataMember]
        public NodeType NodeType { get => _nodeType; }

        internal CanvasPoint _hotspot = new CanvasPoint(0, 0);
        [JsonIgnore]
        [IgnoreDataMember]
        public CanvasPoint Hotspot
        {
            get
            {
                double v = 20.0;
                if ((this as INode).Parent is IComputable)
                {
                    IComputable c = (this as INode).Parent as IComputable;
                    if (NodeType == NodeType.Output)
                    {
                        if (c.ComputationPipelineInfo.IOManager.EventOutputNodes.Count > 1 && c.ComputationPipelineInfo.IOManager.EventOutputNodes.Contains(this))
                        {
                            int i = c.ComputationPipelineInfo.IOManager.EventOutputNodes.IndexOf(this);
                            v = v + i * BoundingBox.Size.Height;
                        }
                        _hotspot = RenderPipelineInfo.Parent.BoundingBox.Location +
                        new CanvasPoint(RenderPipelineInfo.Parent.BoundingBox.Size.Width,
                            BoundingBox.Size.Height / 2 + v);
                    }
                    else
                    {
                        if (c.ComputationPipelineInfo.IOManager.EventInputNodes.Count > 1 && c.ComputationPipelineInfo.IOManager.EventInputNodes.Contains(this))
                        {
                            int i = c.ComputationPipelineInfo.IOManager.EventInputNodes.IndexOf(this);
                            v = v + i * BoundingBox.Size.Height;
                        }
                        _hotspot = RenderPipelineInfo.Parent.BoundingBox.Location +
                        new CanvasPoint(0.0, BoundingBox.Size.Height / 2 + v);
                    }
                }
                return _hotspot;
            }
        }

        [JsonIgnore]
        [IgnoreDataMember]
        public double HotspotThresholdRadius { get; }

        //public Type DataValueType => typeof(D);

        //private DataStructure<D> _dataGoo = new DataStructure<D>();
        //public DataStructure<D> DataGoo { get => _dataGoo; set => _dataGoo = value; }

        [JsonIgnore]
        private ComputationPipelineInfo _computationPipelineInfo;
        [JsonIgnore]
        [IgnoreDataMember]
        public ComputationPipelineInfo ComputationPipelineInfo => _computationPipelineInfo;

        [JsonIgnore]
        [IgnoreDataMember]
        public ElementsLinkedList<INode> Nodes => new ElementsLinkedList<INode>() { this };
        [JsonIgnore]
        [IgnoreDataMember]
        public ComputableElementState ComputableElementState { get; set; }

        #endregion
        protected PropertiesViewModel propertiesViewModel;
        public virtual PropertiesViewModel Properties { get { if (propertiesViewModel == null) propertiesViewModel = new PropertiesViewModel<EventNode>(this); return propertiesViewModel; } set { if (value == propertiesViewModel) return; propertiesViewModel = value; OnPropertyChanged("PropertiesViewModel"); } }

        public virtual void Accept(IVisitNodes visitor)
        {
            visitor.Visit(this);
        }
        public EventNode(IRenderable parent, NodeType type = NodeType.Unset)
        {
            renderPipelineInfo = new RenderPipelineInfo(this);
            _computationPipelineInfo = new ComputationPipelineInfo(this);
            RenderPipelineInfo.Parent = parent as IRenderable;
            //this.DataGoo.DataChanged += OnDataChanged;
            _nodeType = type;
        }

        public EventNode(SerializationInfo info, StreamingContext context)
        {
            //this.renderPipelineInfo = new RenderPipelineInfo(this);
            //_computationPipelineInfo = new ComputationPipelineInfo(this);
            BaseCompViewModel parentComp = info.GetValue("Parent", typeof(BaseCompViewModel)) as BaseCompViewModel;
            //info.AddValue("BezierElements", this.BezierElements);
            ElementsLinkedList<BezierElementViewModel> connections = info.GetValue("BezierElements", typeof(ElementsLinkedList<BezierElementViewModel>)) as ElementsLinkedList<BezierElementViewModel>;
            //this.DataGoo = info.GetValue("DataGoo", typeof(DataStructure<D>)) as DataStructure<D>;
            if (/*this.DataGoo != null || */connections != null && connections.Count > 0)
            {
                if (DataViewModel.DataModel.GetElementWithGuid(parentComp.ID) is BaseCompViewModel comp)
                {
                    //TODO: Replicate the connections and set the data
                    //comp.ChildElementManager.InputNodes.
                }
                else
                {
                    //TODO: Create a new comp, replicate connections and set the data
                    CoreConsole.Log("Parent comp not found", true);
                }
            }
            //this._nodeType = (NodeType)info.GetValue("NodeType", typeof(NodeType));
        }


        #region Properties


        //private NodeType _nodeType = Core.NodeType.Unset;
        //public NodeType NodeType { get => _nodeType; }
        //private ComputationPipelineInfo _computationPipelineInfo;
        //public ComputationPipelineInfo ComputationPipelineInfo => _computationPipelineInfo;
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
                    Exception ex = new Exception("Invalid View Type");
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

        [JsonIgnore]
        [IgnoreDataMember]
        public bool IsSelected { get; set; }

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
        [IgnoreDataMember]
        public ElementState State { get; set; }

        //public IRenderView ElementView { get; internal set; }

        [JsonIgnore]
        [IgnoreDataMember]
        public ElementState ElementState { get; set; }
        public ElementType ElementType { get => ElementType.Node; set => ElementType = ElementType.Node; }

        [JsonIgnore]
        [IgnoreDataMember]
        bool IRenderable.Visible { get; set; }


        [JsonIgnore]
        [IgnoreDataMember]
        public IEnumerable<IElement> ElementDS
        {
            get
            {
                List<IElement> elements = new List<IElement>();
                if (NodeType == NodeType.Input) return elements;
                if (Connections != null && Connections.Count > 0)
                {
                    foreach (IConnection connection in Connections)
                    {
                        if (connection.Origin == this)
                        {
                            elements.Add(connection.Destination.Parent);
                        }
                    }
                }
                return elements;
            }
        }
        [JsonIgnore]
        [IgnoreDataMember]
        public IEnumerable<IElement> ElementUS
        {
            get
            {
                List<IElement> elements = new List<IElement>();
                if (NodeType == NodeType.Output) return elements;
                if (Connections != null && Connections.Count > 0)
                {
                    foreach (IConnection connection in Connections)
                    {
                        if (connection.Destination == this)
                        {
                            elements.Add(connection.Origin.Parent);
                        }
                    }
                }
                return elements;
            }
        }

        [JsonIgnore]
        [IgnoreDataMember]
        public EventArgData EventArgData { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public IRenderable Parent { get => RenderPipelineInfo.Parent; set => RenderPipelineInfo.SetParent(value); }
        public abstract string Name { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public bool RenderExpired { get; set; }

        //public IRenderable Parent => this.RenderPipelineInfo.Parent;

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

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        public event IEventNode.NodeEventHandler NodeEvent;

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

        public void TriggerEvent(EventArgData e)
        {
            try
            {
                NodeEvent.Invoke(this, e);

            }
            catch(System.Exception se)
            {

            }
            if (Parent is IComputable)
            {
                ComputationCore.Compute(Parent as IComputable, false);
            }
        }

        public bool EventOccured(EventArgData e, bool upstreamCallback = false)
        {
            EventArgData = e;
            if (Parent is IComputable)
            {
                IComputable computable = Parent as IComputable;
                //TODO: Call a delegate method that triggers a call-back once complete
                if (upstreamCallback)
                {
                    foreach (IComputable compUS in ElementUS)
                    {
                        if (compUS != null)
                        {
                            if (compUS.ComputationPipelineInfo.IOManager.EventOutputNodes != null &&
                                compUS.ComputationPipelineInfo.IOManager.EventOutputNodes.Count > 0)
                            {
                                foreach (IEventNode en in compUS.ComputationPipelineInfo.IOManager.EventOutputNodes)
                                {
                                    if (en.Connections != null && en.Connections.Count > 0)
                                    {
                                        foreach (IConnection connection in en.Connections)
                                        {
                                            if (connection.Destination == this)
                                            {
                                                if (connection.Origin is EventNode d)
                                                {
                                                    d.TriggerEvent(e);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (IComputable compDS in ElementDS)
                    {
                        if (compDS != null)
                        {
                            if (compDS.ComputationPipelineInfo.IOManager.EventInputNodes != null &&
                                compDS.ComputationPipelineInfo.IOManager.EventInputNodes.Count > 0)
                            {
                                foreach (IEventNode en in compDS.ComputationPipelineInfo.IOManager.EventInputNodes)
                                {
                                    if (en.Connections != null && en.Connections.Count > 0)
                                    {
                                        foreach (IConnection connection in en.Connections)
                                        {
                                            if (connection.Origin == this)
                                            {
                                                if (connection.Destination is EventNode d)
                                                {
                                                    d.TriggerEvent(e);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                //if (computable.ComputationPipelineInfo.IOManager.EventInputNodes.Contains(this))
                //{
                //    int i = computable.ComputationPipelineInfo.IOManager.EventInputNodes.IndexOf(this);
                //    computable.ComputationPipelineInfo.IOManager.EventDelegates[i].Invoke(this, new EventArgData());
                //    return true;
                //}
                //ComputationPipeline.ComputeComputable(computable);
            }
            return false;
        }
        public bool EventOccured(EventArgData e)
        {
            EventArgData = e;
            if (Parent is IComputable)
            {
                IComputable computable = Parent as IComputable;
                //TODO: Call a delegate method that triggers a call-back once complete
                foreach (IComputable compDS in ElementDS)
                {
                    if (compDS != null)
                    {
                        if (compDS.ComputationPipelineInfo.IOManager.EventInputNodes != null &&
                            compDS.ComputationPipelineInfo.IOManager.EventInputNodes.Count > 0)
                        {
                            foreach (IEventNode en in compDS.ComputationPipelineInfo.IOManager.EventInputNodes)
                            {
                                if (en.Connections != null && en.Connections.Count > 0)
                                {
                                    foreach (IConnection connection in en.Connections)
                                    {
                                        if (connection.Origin == this)
                                        {
                                            if (connection.Destination is EventNode d)
                                            {
                                                d.TriggerEvent(e);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                //if (computable.ComputationPipelineInfo.IOManager.EventInputNodes.Contains(this))
                //{
                //    int i = computable.ComputationPipelineInfo.IOManager.EventInputNodes.IndexOf(this);
                //    computable.ComputationPipelineInfo.IOManager.EventDelegates[i].Invoke(this, new EventArgData());
                //    return true;
                //}
                //ComputationPipeline.ComputeComputable(computable);
            }
            return false;
        }
        public abstract void ToggleActive();
        public void Compute()
        {
            ComputableElementState = ComputableElementState.Computed;
        }
        public bool CollectData()
        {
            bool result = false;
            if (Connections != null && Connections.Count > 0)
            {
                foreach (IConnection conn in Connections)
                {
                    //INCOMING CONNECTIONS
                    if (conn.Destination == this && conn.Origin is IEventNode)
                    {
                        IEventNode no = conn.Origin as IEventNode;
                        if (!no.ComputationPipelineInfo.EventUS.Contains(this)) result = true;
                        else
                        {
                            //TODO: Handle Error & loop checking!!!!
                        }
                        //if (!this.DataGoo.IsValid)
                        //{
                        //    this.DataGoo.Clear();
                        //    this.DataGoo.Data = no.DataGoo.Data;
                        //}
                        //else if (!this.DataGoo.Data.Equals(no.DataGoo.Data))
                        //{
                        //    this.DataGoo.Data = no.DataGoo.Data;
                        //}
                        //this.NodeContentColor = System.Windows.Media.Brushes.White;
                        //break;
                    }
                    //OUTGOING CONNECTIONS
                    //else if (conn.Origin == n/* && conn.Destination is NodeElement*/)
                    //{
                    //NodeElement nd = (NodeElement)conn.Destination;
                    //nd.DataGoo.Data = _sliderValue + _inputValue;
                    //RenderPipeline.RenderRenderable(conn.Destination.Parent as IRenderable);
                    //}
                }
            }
            return result;
        }
        public void DeliverData()
        {
            if (Connections != null && Connections.Count > 0)
            {
                foreach (IConnection conn in Connections)
                {
                    if (conn.Origin == this && conn.Destination is IEventNode)
                    {
                        IEventNode no = conn.Origin as IEventNode;
                        //if (!nd.DataGoo.IsValid)
                        //{
                        //    nd.DataGoo.Clear();
                        //    nd.DataGoo.Data = this.DataGoo.Data;
                        //}
                        //else if (!(nd.DataGoo.Data.Equals(this.DataGoo.Data)))
                        //{
                        //    nd.DataGoo.Data = this.DataGoo.Data;
                        //}
                        //this.NodeContentColor = System.Windows.Media.Brushes.White;
                        //break;
                    }
                    //OUTGOING CONNECTIONS
                    //else if (conn.Origin == n/* && conn.Destination is NodeElement*/)
                    //{
                    //NodeElement nd = (NodeElement)conn.Destination;
                    //nd.DataGoo.Data = _sliderValue + _inputValue;
                    //RenderPipeline.RenderRenderable(conn.Destination.Parent as IRenderable);
                    //}
                }
            }
        }


        private StringBuilder sbLog = new StringBuilder();
        public void OnLog_Internal(EventArgData e)
        {
            sbLog.AppendLine(e.ToString());
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
                //info.AddValue("BezierElements", this.BezierElements);
                //info.AddValue("NodeType", this.NodeType);
                //info.AddValue("Children", this.Children);
            }
            catch (Exception ex)
            {
                CoreConsole.Log(ex);
            }
        }

        public void Dispose()
        {
            if (RenderPipelineInfo.Children != null && RenderPipelineInfo.Children.Count > 0)
            {
                foreach (var child in RenderPipelineInfo.Children)
                {
                    if (child != null) child.Dispose();
                }
                foreach (var connection in Connections)
                {
                    if (connection != null) connection.Dispose();
                }
            }
            DataViewModel.DataModel.Elements.Remove(this);
            GC.SuppressFinalize(this);
        }
        ~EventNode() => Dispose();
    }

    //public enum CompOrientation
    //{
    //    Horizontal,
    //    Vertical
    //}

}
