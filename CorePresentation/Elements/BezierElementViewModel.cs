using Core;
using Core.Nodes;
using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
using System.Windows;
using Verse3.CorePresentation.Workspaces;
using Verse3.Nodes;
using static Core.Geometry2D;

namespace Verse3.Elements
{
    [Serializable]
    public class BezierElementViewModel : BaseElement, IConnection
    {
        #region Data Members

        private BoundingBox innerBoundingBox = BoundingBox.Unset;
        private INode origin;
        private INode destination;
        internal bool inflatedX = false;
        internal bool inflatedY = false;

        #endregion

        #region Properties

        [JsonIgnore]
        [IgnoreDataMember]
        public BoundingBox InnerBoundingBox { get => this.innerBoundingBox; private set => this.innerBoundingBox = value; }
        [JsonIgnore]
        [IgnoreDataMember]
        public Point StartPoint => new Point(this.Origin.Hotspot.X, this.Origin.Hotspot.Y);
        [JsonIgnore]
        [IgnoreDataMember]
        public Point EndPoint => new Point(this.Destination.Hotspot.X, this.Destination.Hotspot.Y);
        [JsonIgnore]
        [IgnoreDataMember]
        public override Type ViewType => typeof(BezierElementModelView);
        [JsonIgnore]
        [IgnoreDataMember]
        public BezierDirection Direction { get; private set; }
        //[JsonIgnore]
        //[IgnoreDataMember]
        public INode Origin { get => this.origin; }
        //[JsonIgnore]
        //[IgnoreDataMember]
        public INode Destination { get => this.destination; }
        public ConnectionType ConnectionType { get; }
        [JsonIgnore]
        [IgnoreDataMember]
        public bool TopToBottom => (this.origin.Hotspot.Y < this.destination.Hotspot.Y);
        [JsonIgnore]
        [IgnoreDataMember]
        public bool LeftToRight => (this.origin.Hotspot.X < this.destination.Hotspot.X);
        
        #endregion

        public bool SetDestination(INode destination)
        {
            //TODO: LOOP WARNING!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //this.destination.Connections.Remove(this);
            if (this.destination != MousePositionNode.Instance && this.origin == MousePositionNode.Instance)
            {
                //TODO: Check whether destination is valid
                bool check = (destination.GetType().BaseType == this.destination.GetType().BaseType);
                check = check && (destination.NodeType != this.destination.NodeType);
                check = NodeUtilities.CheckCompatibility(destination, this.destination);
                if (!check) return false;
                this.origin = destination;
            }
            else
            {
                //TODO: Check whether destination is valid
                bool check = (destination.GetType().BaseType == this.origin.GetType().BaseType);
                check = check && (destination.NodeType != this.origin.NodeType);
                check = NodeUtilities.CheckCompatibility(this.origin, destination);
                if (!check) return false;
                this.destination = destination;
            }
            //this.destination.Connections.Add(this);
            RedrawBezier(this.origin, this.destination);
            if (this.RenderView != null)
            {
                this.RenderView.Render();
            }
            return true;
        }

        public void Remove()
        {
            try
            {
                this.origin.Connections.Remove(this);
                this.destination.Connections.Remove(this);
                //this.origin = null;
                //this.destination = null;
                WorkspaceViewModel.StaticWorkspaceViewModel.SelectedDataViewModel.Elements.Remove(this);
                this.Dispose();
            }
            catch (Exception ex)
            {
                CoreConsole.Log(ex);
            }
        }

        #region Constructors

        public BezierElementViewModel() : base()
        {
        }
        
        public BezierElementViewModel(INode start, INode end) : base()
        {
            if (this.origin != start) this.origin = start;
            if (this.destination != end) this.destination = end;
            //if (this.origin.NodeType == NodeType.Input) this.Direction = BezierDirection.ForceRightToLeft;
            //else this.Direction = BezierDirection.ForceLeftToRight;
            if (this.origin.NodeType == NodeType.Input)
            {
                this.destination = start;
                this.origin = end;
            }
            RedrawBezier(this.origin, this.destination);
        }

        public BezierElementViewModel(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            INode o = (INode)info.GetValue("origin", typeof(INode));
            INode d = (INode)info.GetValue("destination", typeof(INode));
            if (o != null) this.origin = o;
            if (d != null) this.destination = d;
            if (this.origin != null && this.Destination != null) RedrawBezier(this.origin, this.destination);
            else throw new NullReferenceException("BezierElement: Origin and/or Destination is null");
        }

        public new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("origin", this.origin);
            info.AddValue("destination", this.destination);
        }

        #endregion

        public void RedrawBezier(INode start, INode end)
        {
            if (this.origin != start) this.origin = start;
            if (this.destination != end) this.destination = end;
            //RedrawBezier(((start.Hotspot.X) - 200), ((start.Hotspot.Y) - 200), ((end.Hotspot.X) - (start.Hotspot.X)), ((end.Hotspot.Y) - (start.Hotspot.Y)));
            //RedrawBezier(start.Hotspot.X, start.Hotspot.Y, ((end.Hotspot.X) - (start.Hotspot.X)), ((end.Hotspot.Y) - (start.Hotspot.Y)));
            if (LeftToRight && TopToBottom)
            {
                //BottomRight
                base.boundingBox = new BoundingBox(start.Hotspot.X, start.Hotspot.Y, Math.Abs((end.Hotspot.X) - (start.Hotspot.X)), Math.Abs((end.Hotspot.Y) - (start.Hotspot.Y)));
            }
            else if (!LeftToRight && !TopToBottom)
            {
                //TopLeft
                base.boundingBox = new BoundingBox(end.Hotspot.X, end.Hotspot.Y, Math.Abs((end.Hotspot.X) - (start.Hotspot.X)), Math.Abs((end.Hotspot.Y) - (start.Hotspot.Y)));
            }
            else if (LeftToRight && !TopToBottom)
            {
                //TopRight
                base.boundingBox = new BoundingBox(start.Hotspot.X, end.Hotspot.Y, Math.Abs((end.Hotspot.X) - (start.Hotspot.X)), Math.Abs((end.Hotspot.Y) - (start.Hotspot.Y)));
            }
            else if (!LeftToRight && TopToBottom)
            {
                //BottomLeft
                base.boundingBox = new BoundingBox(end.Hotspot.X, start.Hotspot.Y, Math.Abs((end.Hotspot.X) - (start.Hotspot.X)), Math.Abs((end.Hotspot.Y) - (start.Hotspot.Y)));
            }
            this.OnPropertyChanged("BoundingBox");
            this.InnerBoundingBox = this.BoundingBox;
            this.inflatedX = false;
            this.inflatedY = false;
        }
    }
}