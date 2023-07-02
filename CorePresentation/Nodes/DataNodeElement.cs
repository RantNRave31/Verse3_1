using Core;
using Core.Nodes;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows;
using Verse3.Elements;
using static Core.Geometry2D;

namespace Verse3.Nodes
{
    [Serializable]
    public class DataNodeElement<T> : DataNode<T>
    {
        [JsonIgnore]
        public override Type ViewType => typeof(DataNodeElementModelView);

        #region Constructor and Compute

        public DataNodeElement(IRenderable parent, NodeType type = NodeType.Unset) : base(parent, type)
        {
            //_computationPipelineInfo = new ComputationPipelineInfo(this);
            //this.RenderPipelineInfo.Parent = parent as IRenderable;
            double x = DataModel.ContentCanvasMarginOffset + RenderPipelineInfo.Parent.X;
            double y = DataModel.ContentCanvasMarginOffset + RenderPipelineInfo.Parent.Y;
            boundingBox = new BoundingBox(x, y, RenderPipelineInfo.Parent.Width, 50);
            (this as IRenderable).RenderPipelineInfo.SetParent(RenderPipelineInfo.Parent);
            //this.DisplayedText = "Node";
            PropertyChanged += NodeElement_PropertyChanged;
            if (type == NodeType.Input)
            {
                HorizontalAlignment = HorizontalAlignment.Left;
            }
            else if (type == NodeType.Output)
            {
                HorizontalAlignment = HorizontalAlignment.Right;
            }
            else
            {
                HorizontalAlignment = HorizontalAlignment.Center;
            }
        }

        public DataNodeElement(DataViewModel dataViewModel, SerializationInfo info, StreamingContext context) : base(info, context)
        {
            PropertyChanged += NodeElement_PropertyChanged;
        }

        public new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        #endregion

        public override void Accept(IVisitNodes visitor)
        {
            visitor.Visit(this);
        }
        private void NodeElement_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            foreach (IRenderable renderable in Connections)
            {
                //renderable.Render();
            }
        }

        public override void ToggleActive()
        {
            //Set as active Node
            BezierElementViewModel b = (BezierElementViewModel)ArsenalViewModel.StaticArsenal.SelectedDataViewModel.SelectedConnection;
            if (ArsenalViewModel.StaticArsenal.SelectedDataViewModel.SelectedConnection == default)
            {
                ArsenalViewModel.StaticArsenal.SelectedDataViewModel.SelectedNode = this as INode;
                ArsenalViewModel.StaticArsenal.SelectedDataViewModel.SelectedConnection = ArsenalViewModel.StaticArsenal.SelectedDataViewModel.CreateConnection(ArsenalViewModel.StaticArsenal.SelectedDataViewModel.SelectedNode);
                b = (BezierElementViewModel)ArsenalViewModel.StaticArsenal.SelectedDataViewModel.SelectedConnection;
                if (b != null)
                {
                    RenderPipelineInfo.AddChild(b);
                    Connections.Add(b);
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
                    if (ArsenalViewModel.StaticArsenal.SelectedDataViewModel.SelectedNode.NodeType != NodeType)
                    {
                        if (NodeUtilities.CheckCompatibility(ArsenalViewModel.StaticArsenal.SelectedDataViewModel.SelectedNode, this))
                        {
                            if (b.SetDestination(this as INode))
                            {
                                if (ArsenalViewModel.StaticArsenal.SelectedDataViewModel.SelectedNode is IComputable && ArsenalViewModel.StaticArsenal.SelectedDataViewModel.SelectedNode != this && ArsenalViewModel.StaticArsenal.SelectedDataViewModel.SelectedNode.NodeType == NodeType.Output)
                                {
                                    ComputationPipelineInfo.AddDataUpStream(ArsenalViewModel.StaticArsenal.SelectedDataViewModel.SelectedNode as IComputable);
                                }
                                ArsenalViewModel.StaticArsenal.SelectedDataViewModel.SelectedNode = this as INode;
                                if (MousePositionNode.Instance.Connections.Contains(b))
                                    MousePositionNode.Instance.Connections.Remove(b);
                                RenderPipelineInfo.AddChild(b);
                                Connections.Add(b);
                                //this.nodeContentColor = System.Windows.Media.Brushes.White;
                                ArsenalViewModel.StaticArsenal.SelectedDataViewModel.SelectedConnection = default;
                                ArsenalViewModel.StaticArsenal.SelectedDataViewModel.SelectedNode = default;
                            }
                        }
                    }
                }
            }
            //ComputationPipeline.Compute();
            //this.Element.OnPropertyChanged("BoundingBox");
            if (RenderPipelineInfo.Parent is IComputable)
            {
                IComputable computable = (IComputable)RenderPipelineInfo.Parent;
                ComputationCore.Compute(computable);
            }
            RenderingCore.Render(RenderPipelineInfo.Parent);
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
                //if (this.Connections != null && this.Connections.Count > 0)
                //{
                //    if (nodeContentColor != System.Windows.Media.Brushes.White)
                //    {
                //        nodeContentColor = System.Windows.Media.Brushes.White;
                //        SetProperty(ref nodeContentColor, System.Windows.Media.Brushes.White);
                //        //OnPropertyChanged("NodeContentColor");
                //    }
                //}
                //else
                //{
                //    if (nodeContentColor != System.Windows.Media.Brushes.Transparent)
                //    {
                //        nodeContentColor = System.Windows.Media.Brushes.Transparent;
                //        SetProperty(ref nodeContentColor, System.Windows.Media.Brushes.Transparent);
                //        //OnPropertyChanged("NodeContentColor");
                //    }
                //}
                return nodeColor;
            }
            internal set => SetProperty(ref nodeColor, value);
        }

        private System.Windows.Media.Brush nodeContentColor = System.Windows.Media.Brushes.Transparent;
        [JsonIgnore]
        public System.Windows.Media.Brush NodeContentColor
        {
            get
            {
                //if (this.Connections != null && this.Connections.Count > 0)
                //{
                //    if (nodeContentColor != System.Windows.Media.Brushes.White)
                //    {
                //        nodeContentColor = System.Windows.Media.Brushes.White;
                //        SetProperty(ref nodeContentColor, System.Windows.Media.Brushes.White);
                //        //OnPropertyChanged("NodeContentColor");
                //    }
                //}
                //else
                //{
                //    if (nodeContentColor != System.Windows.Media.Brushes.Transparent)
                //    {
                //        nodeContentColor = System.Windows.Media.Brushes.Transparent;
                //        SetProperty(ref nodeContentColor, System.Windows.Media.Brushes.Transparent);
                //        //OnPropertyChanged("NodeContentColor");
                //    }
                //}
                return nodeContentColor;
            }
            internal set => SetProperty(ref nodeContentColor, value);
        }

    }
}