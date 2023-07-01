using Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Verse3.Elements;
using Newtonsoft.Json;
using static Core.Geometry2D;
using System.Text;
using Verse3.Components;

namespace Verse3.Nodes
{
    [Serializable] //READ ONLY SERIALIZATION
    public abstract class DataNode<D> : IRenderable, IDataNode<D>
    {
        #region Data Members

        private RenderPipelineInfo renderPipelineInfo;
        protected BoundingBox boundingBox = BoundingBox.Unset;
        private Guid _id = Guid.NewGuid();
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
                    Exception ex = new Exception("Invalid RenderView type");
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
        public bool RenderExpired { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
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

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        public event IDataNode<D>.NodeDataChangedEventHandler NodeDataChanged;

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

        #region Data Members

        private ElementsLinkedList<IConnection> connections = new ElementsLinkedList<IConnection>();
        //internal IRenderable parentElement = default;
        //private object displayedText;

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

        [JsonIgnore]
        [IgnoreDataMember]
        internal CanvasPoint _hotspot = new CanvasPoint(0, 0);
        [JsonIgnore]
        [IgnoreDataMember]
        public CanvasPoint Hotspot
        {
            get
            {
                //TODO: Turn into a converter for BaseComp
                double v = 20.0;
                if ((this as INode).Parent is IComputable)
                {
                    IComputable c = (this as INode).Parent as IComputable;
                    if (NodeType == NodeType.Output)
                    {
                        if (c.ComputationPipelineInfo.IOManager.EventOutputNodes.Count > 0)
                        {
                            for (int indexEvent = 0; indexEvent < c.ComputationPipelineInfo.IOManager.EventOutputNodes.Count; indexEvent++)
                            {
                                v += ((EventNode)c.ComputationPipelineInfo.IOManager.EventOutputNodes[indexEvent]).BoundingBox.Size.Height;
                            }
                        }
                        if (c.ComputationPipelineInfo.IOManager.DataOutputNodes.Count > 1 && c.ComputationPipelineInfo.IOManager.DataOutputNodes.Contains(this))
                        {
                            for (int indexData = 0; indexData < c.ComputationPipelineInfo.IOManager.DataOutputNodes.Count; indexData++)
                            {
                                if (c.ComputationPipelineInfo.IOManager.DataOutputNodes[indexData] != this)
                                {
                                    if (c.ComputationPipelineInfo.IOManager.DataOutputNodes[indexData] is IRenderable)
                                    {
                                        v += ((IRenderable)c.ComputationPipelineInfo.IOManager.DataOutputNodes[indexData]).BoundingBox.Size.Height;
                                    }
                                }
                                else
                                {
                                    //if (c.ComputationPipelineInfo.IOManager.DataOutputNodes[indexData] is IRenderable)
                                    //{
                                    //    v += this.BoundingBox.Size.Height;
                                    break;
                                    //}
                                }
                            }
                        }
                        _hotspot = RenderPipelineInfo.Parent.BoundingBox.Location +
                        new CanvasPoint(RenderPipelineInfo.Parent.BoundingBox.Size.Width,
                            BoundingBox.Size.Height / 2 + v);
                    }
                    else
                    {
                        if (c.ComputationPipelineInfo.IOManager.EventInputNodes.Count > 0)
                        {
                            for (int indexEvent = 0; indexEvent < c.ComputationPipelineInfo.IOManager.EventInputNodes.Count; indexEvent++)
                            {
                                v += ((EventNode)c.ComputationPipelineInfo.IOManager.EventInputNodes[indexEvent]).BoundingBox.Size.Height;
                            }
                        }
                        if (c.ComputationPipelineInfo.IOManager.DataInputNodes.Count > 1 && c.ComputationPipelineInfo.IOManager.DataInputNodes.Contains(this))
                        {
                            for (int indexData = 0; indexData < c.ComputationPipelineInfo.IOManager.DataInputNodes.Count; indexData++)
                            {
                                if (c.ComputationPipelineInfo.IOManager.DataInputNodes[indexData] != this)
                                {
                                    if (c.ComputationPipelineInfo.IOManager.DataInputNodes[indexData] is IRenderable)
                                    {
                                        v += ((IRenderable)c.ComputationPipelineInfo.IOManager.DataInputNodes[indexData]).BoundingBox.Size.Height;
                                    }
                                }
                                else
                                {
                                    //if (c.ComputationPipelineInfo.IOManager.DataInputNodes[indexData] is IRenderable)
                                    //{
                                    //    v += this.BoundingBox.Size.Height;
                                    break;
                                    //}
                                }
                            }
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
        public Type DataValueType => typeof(D);

        private DataStructure<D> _dataGoo = new DataStructure<D>();
        [JsonIgnore]
        [IgnoreDataMember]
        public DataStructure<D> DataGoo
        {
            get => _dataGoo;
            set
            {
                try
                {
                    if (value != null && value is DataStructure<D>)
                        _dataGoo = value as DataStructure<D>;
                    else
                    {
                        if (value != null && value.Data != null)
                        {
                            if (value.Data.GetType().IsAssignableTo(typeof(D)))
                            {
                                _dataGoo = value.DuplicateAsType<D>();
                            }
                            else if (value.DataType.IsAssignableTo(typeof(DataStructure<D>)))
                            {
                                //TODO: Improve DataStructure depth with a while loop to iterate into branches
                                _dataGoo = ((DataStructure<D>)value.Data).DuplicateAsType<D>();
                            }
                        }
                        _dataGoo = new DataStructure<D>();
                    }
                }
                catch (Exception ex)
                {
                    CoreConsole.Log(ex);
                }
            }
        }

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
        [JsonIgnore]
        [IgnoreDataMember]
        DataStructure IDataGooContainer.DataGoo
        {
            get => _dataGoo;
            set
            {
                try
                {
                    if (value != null && value is DataStructure<D>)
                        _dataGoo = value as DataStructure<D>;
                    else if (value != null && value is DataStructure)
                    {
                        if (value.DataType.IsAssignableTo(typeof(D)))
                        {
                            _dataGoo = value.DuplicateAsType<D>();
                        }
                    }
                    else
                    {
                        _dataGoo = new DataStructure<D>();
                    }
                }
                catch (Exception ex)
                {
                    CoreConsole.Log(ex);
                }
            }
        }

        public abstract string Name { get; set; }

        #endregion
        protected PropertiesViewModel propertiesViewModel;
        public virtual PropertiesViewModel Properties { get { if (propertiesViewModel == null) propertiesViewModel = new PropertiesViewModel<DataNode<D>>(this); return propertiesViewModel; } set { if (value == propertiesViewModel) return; propertiesViewModel = value; OnPropertyChanged("PropertiesViewModel"); } }
        public virtual void Accept(IVisitNodes visitor)
        {
            visitor.Visit(this);
        }


        private void OnDataChanged(DataStructure<D> sender, DataChangedEventArgs<D> e)
        {
            NodeDataChanged.Invoke(this, e);
        }
        public DataNode(IRenderable parent, NodeType type = NodeType.Unset) : base()
        {
            renderPipelineInfo = new RenderPipelineInfo(this);
            _computationPipelineInfo = new ComputationPipelineInfo(this);
            RenderPipelineInfo.Parent = parent as IRenderable;
            //this.DataGoo.DataChanged += OnDataChanged;
            _nodeType = type;
            //this.DataGoo.DataChanged += DataChanged;
            //double x = DataViewModel.ContentCanvasMarginOffset + this.RenderPipelineInfo.Parent.X;
            //double y = DataViewModel.ContentCanvasMarginOffset + this.RenderPipelineInfo.Parent.Y;
            //base.boundingBox = new BoundingBox(x, y, this.RenderPipelineInfo.Parent.Width, 50);
            //(this as IRenderable).RenderPipelineInfo.SetParent(this.RenderPipelineInfo.Parent);
            //this.DisplayedText = "Node";
            //this.PropertyChanged += NodeElement_PropertyChanged;
            //if (type == NodeType.Input)
            //{
            //    this.HorizontalAlignment = HorizontalAlignment.Left;
            //}
            //else if (type == NodeType.Output)
            //{
            //    this.HorizontalAlignment = HorizontalAlignment.Right;
            //}
            //else
            //{
            //    this.HorizontalAlignment = HorizontalAlignment.Center;
            //}
        }

        public DataNode(SerializationInfo info, StreamingContext context)
        {
            //this.renderPipelineInfo = new RenderPipelineInfo(this);
            //_computationPipelineInfo = new ComputationPipelineInfo(this);
            BaseCompViewModel parentComp = info.GetValue("Parent", typeof(BaseCompViewModel)) as BaseCompViewModel;
            //info.AddValue("BezierElements", this.BezierElements);
            ElementsLinkedList<BezierElementViewModel> connections = info.GetValue("BezierElements", typeof(ElementsLinkedList<BezierElementViewModel>)) as ElementsLinkedList<BezierElementViewModel>;
            DataGoo = info.GetValue("DataGoo", typeof(DataStructure<D>)) as DataStructure<D>;
            if (DataGoo != null || connections != null && connections.Count > 0)
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

        //event EventHandler<DataChangedEventArgs> IDataNode.DataChanged
        //{
        //    add => DataChanged += value;
        //    remove => DataChanged -= value;
        //}

        //event EventHandler<DataChangedEventArgs<D>> IDataNode<D>.DataChanged
        //{
        //    add => DataChanged += value;
        //    remove => DataChanged -= value;
        //}

        //public new event EventHandler<DataChangedEventArgs> DataChanged;


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
                    if (conn.Destination == this && conn.Origin is IDataNode)
                    {
                        if (conn.Origin is IDataNode<D> norigin)
                        {
                            if (!DataGoo.IsValid)
                            {
                                DataGoo.Clear();
                                DataGoo = norigin.DataGoo;
                                result = true;
                            }
                            else if (!DataGoo.Equals(norigin.DataGoo))
                            {
                                DataGoo = norigin.DataGoo;
                                result = true;
                            }
                        }
                        else if ((conn.Origin as IDataNode).DataValueType.IsAssignableFrom(DataValueType)
                            || (conn.Origin as IDataNode).DataValueType.IsAssignableTo(DataValueType))
                        {
                            IDataNode noriginCAST = conn.Origin as IDataNode;
                            try
                            {
                                if (!DataGoo.IsValid)
                                {
                                    DataGoo.Clear();
                                    if (noriginCAST.DataGoo != null)
                                    {
                                        DataGoo = noriginCAST.DataGoo.DuplicateAsType<D>();
                                        result = true;
                                    }
                                }
                                else if (!DataGoo.Equals(noriginCAST.DataGoo))
                                {
                                    DataGoo = noriginCAST.DataGoo.DuplicateAsType<D>();
                                    result = true;
                                }
                            }
                            catch (Exception ex)
                            {
                                CoreConsole.Log(ex);
                            }
                        }
                        else
                        {
                            result = false;
                            Exception ex = new Exception("Data type mismatch error");
                            CoreConsole.Log(ex);
                        }
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
                    if (conn.Origin == this && conn.Destination is IDataNode)
                    {
                        if (conn.Destination is IDataNode<D> ndest)
                        {
                            if (!ndest.DataGoo.IsValid)
                            {
                                ndest.DataGoo.Clear();
                                ndest.DataGoo = DataGoo;
                            }
                            else if (!ndest.DataGoo.Equals(DataGoo))
                            {
                                ndest.DataGoo = DataGoo;
                            }
                        }
                        else if ((conn.Destination as IDataNode).DataValueType.IsAssignableFrom(DataValueType)
                            || (conn.Destination as IDataNode).DataValueType.IsAssignableTo(DataValueType))
                        {
                            IDataNode ndestCAST = conn.Destination as IDataNode;
                            try
                            {
                                if (ndestCAST != null)
                                {
                                    if (ndestCAST.DataGoo != null && !ndestCAST.DataGoo.IsValid)
                                    {
                                        ndestCAST.DataGoo.Clear();
                                        //ndestCAST.DataGoo = this.DataGoo.Duplicate<ndestCAST.DataValueType>;
                                        MethodInfo mi = typeof(DataStructure).GetMethod("DuplicateAsType").MakeGenericMethod(ndestCAST.DataValueType);
                                        object result = mi.Invoke(DataGoo, null);
                                        if (result != null && result is DataStructure resultDs)
                                        {
                                            ndestCAST.DataGoo = resultDs.Duplicate();
                                            ndestCAST.DataGoo.ToString();
                                        }
                                    }
                                    else if ((conn.Destination as IDataNode).DataValueType.IsAssignableTo(typeof(double)))
                                    {
                                        ndestCAST.DataGoo = DataGoo.DuplicateAsType<double>();
                                        ndestCAST.DataGoo.ToString();
                                    }
                                    else if (!ndestCAST.DataGoo.Equals(DataGoo))
                                    {
                                        ndestCAST.DataGoo = DataGoo.Duplicate();
                                        ndestCAST.DataGoo.ToString();
                                    }
                                    else
                                    {
                                        ndestCAST.DataGoo = DataGoo.Duplicate();
                                        ndestCAST.DataGoo.ToString();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                CoreConsole.Log(ex);
                            }
                        }
                        else
                        {
                            Exception ex = new Exception("Data type mismatch error");
                            CoreConsole.Log(ex);
                        }
                        //IDataNode<D> nd = conn.Destination as IDataNode<D>;
                        //if (!nd.DataGoo.IsValid)
                        //{
                        //    nd.DataGoo.Clear();
                        //    nd.DataGoo.Data = this.DataGoo.Data;
                        //}
                        //else if (!(nd.DataGoo.Data.Equals(this.DataGoo.Data)))
                        //{
                        //    nd.DataGoo.Data = this.DataGoo.Data;
                        //}
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
        public abstract void ToggleActive();


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
                info.AddValue("DataGoo", DataGoo);
                //info.AddValue("BezierElements", this.BezierElements);
                //info.AddValue("DataValueType", this.DataValueType);
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
        ~DataNode() => Dispose();
    }

    //public enum CompOrientation
    //{
    //    Horizontal,
    //    Vertical
    //}

}
