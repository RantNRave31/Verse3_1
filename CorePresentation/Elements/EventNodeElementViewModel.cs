using Core;
using Core.Nodes;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows;
using Verse3.CorePresentation.Workspaces;
using Verse3.Nodes;
using static Core.Geometry2D;

namespace Verse3.Elements
{
    public class EventNodeElementViewModel : EventNode
    {
        [JsonIgnore]
        public override Type ViewType => typeof(EventNodeElementModelView);

        #region Constructor
        public EventNodeElementViewModel(IRenderable parent, NodeType type = NodeType.Unset) : base(parent, type)
        {
            //_computationPipelineInfo = new ComputationPipelineInfo(this);
            this.RenderPipelineInfo.Parent = parent as IRenderable;
            double x = DataViewModel.ContentCanvasMarginOffset + this.RenderPipelineInfo.Parent.X;
            double y = DataViewModel.ContentCanvasMarginOffset + this.RenderPipelineInfo.Parent.Y;
            base.boundingBox = new BoundingBox(x, y, this.RenderPipelineInfo.Parent.Width, 50);
            (this as IRenderable).RenderPipelineInfo.SetParent(this.RenderPipelineInfo.Parent);
            //this.DisplayedText = "Node";
            this.PropertyChanged += NodeElement_PropertyChanged;
            if (type == NodeType.Input)
            {
                this.HorizontalAlignment = HorizontalAlignment.Left;
            }
            else if (type == NodeType.Output)
            {
                this.HorizontalAlignment = HorizontalAlignment.Right;
            }
            else
            {
                this.HorizontalAlignment = HorizontalAlignment.Center;
            }
        }
        public EventNodeElementViewModel(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            this.PropertyChanged += NodeElement_PropertyChanged;
        }
        public new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
        #endregion

        private void NodeElement_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            foreach (IRenderable renderable in this.Connections)
            {
                //renderable.Render();
            }
        }

        public override void ToggleActive()
        {
            //if (this.IsActive)
            //{
            //    this.IsActive = false;
            //}
            //else
            //{
            //    this.IsActive = true;
            //}
            //Set as active Node
            BezierElementViewModel b = (BezierElementViewModel)WorkspaceViewModel.StaticSelectedDataViewModel.SelectedConnection;
            if (WorkspaceViewModel.StaticSelectedDataViewModel.SelectedConnection == default)
            {
                WorkspaceViewModel.StaticSelectedDataViewModel.SelectedNode = this as INode;
                WorkspaceViewModel.StaticSelectedDataViewModel.SelectedConnection = WorkspaceViewModel.StaticSelectedDataViewModel.CreateConnection(WorkspaceViewModel.StaticSelectedDataViewModel.SelectedNode);
                b = (BezierElementViewModel)WorkspaceViewModel.StaticSelectedDataViewModel.SelectedConnection;
                if (b != null)
                {
                    this.RenderPipelineInfo.AddChild(b);
                    this.Connections.Add(b);
                    //this.nodeContentColor = System.Windows.Media.Brushes.White;
                    MousePositionNode.Instance.Connections.Add(b);
                    //b.RedrawBezier(b.Origin, b.Destination);
                    //b.RenderView.Render();
                }
            }
            else
            {
                if (b != null)
                {
                    if (WorkspaceViewModel.StaticSelectedDataViewModel.SelectedNode.NodeType != this.NodeType)
                    {
                        if (WorkspaceViewModel.StaticSelectedDataViewModel.SelectedNode.GetType().BaseType == this.GetType().BaseType)
                        {
                            if (b.SetDestination(this as INode))
                            {
                                if (WorkspaceViewModel.StaticSelectedDataViewModel.SelectedNode is IComputable && WorkspaceViewModel.StaticSelectedDataViewModel.SelectedNode != this && WorkspaceViewModel.StaticSelectedDataViewModel.SelectedNode.NodeType == NodeType.Output)
                                {
                                    this.ComputationPipelineInfo.AddEventUpStream(WorkspaceViewModel.StaticSelectedDataViewModel.SelectedNode as IComputable);
                                }
                                WorkspaceViewModel.StaticSelectedDataViewModel.SelectedNode = this as INode;
                                if (MousePositionNode.Instance.Connections.Contains(b))
                                    MousePositionNode.Instance.Connections.Remove(b);
                                this.RenderPipelineInfo.AddChild(b);
                                this.Connections.Add(b);
                                //this.nodeContentColor = System.Windows.Media.Brushes.White;
                                WorkspaceViewModel.StaticSelectedDataViewModel.SelectedConnection = default;
                                WorkspaceViewModel.StaticSelectedDataViewModel.SelectedNode = default;
                            }
                        }
                    }
                }
            }
            //ComputationPipeline.Compute();
            //RenderPipeline.Render();
            //this.Element.OnPropertyChanged("BoundingBox");
            if (this.RenderPipelineInfo.Parent is IComputable)
            {
                IComputable computable = (IComputable)this.RenderPipelineInfo.Parent;
                ComputationCore.Compute(computable);
            }
            RenderingCore.Render(this.RenderPipelineInfo.Parent);
        }

        private string _name = "";
        public override string Name { get => _name; set => _name = value; }

        private HorizontalAlignment horizontalAlignment = HorizontalAlignment.Center;
        [JsonIgnore]
        public HorizontalAlignment HorizontalAlignment
        {
            get
            {
                //if (this.NodeType == NodeType.Input)
                //{
                //    if (horizontalAlignment != HorizontalAlignment.Left)
                //    {
                //        horizontalAlignment = HorizontalAlignment.Left;
                //        //SetProperty(ref horizontalAlignment, HorizontalAlignment.Left);
                //        OnPropertyChanged("HorizontalAlignment");
                //    }
                //}
                //else if (this.NodeType == NodeType.Output)
                //{
                //    if (horizontalAlignment != HorizontalAlignment.Right)
                //    {
                //        horizontalAlignment = HorizontalAlignment.Right;
                //        //SetProperty(ref horizontalAlignment, HorizontalAlignment.Right);
                //        OnPropertyChanged("HorizontalAlignment");
                //    }
                //}
                return horizontalAlignment;
            }
            private set => SetProperty(ref horizontalAlignment, value);
        }

        private System.Windows.Media.Brush nodeColor = System.Windows.Media.Brushes.Transparent;
        [JsonIgnore]
        public System.Windows.Media.Brush NodeColor
        {
            get
            {
                return nodeColor;
            }
            set => SetProperty(ref nodeColor, value);
        }

        private System.Windows.Media.Brush nodeContentColor = System.Windows.Media.Brushes.Transparent;
        [JsonIgnore]
        public System.Windows.Media.Brush NodeContentColor
        {
            get
            {
                return nodeContentColor;
            }
            set => SetProperty(ref nodeContentColor, value);
        }

        [JsonIgnore]
        public bool IsActive { get; protected set; }
    }
}