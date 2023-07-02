using Core;
using Core.Elements;
using Core.Nodes;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Verse3.Elements;
using static Core.Geometry2D;

namespace Verse3.Nodes
{
    public class MousePositionNode : INode
    {
        public static readonly MousePositionNode Instance = new MousePositionNode();
        private ElementsLinkedList<IConnection> connections = new ElementsLinkedList<IConnection>();
        protected PropertiesViewModel propertiesViewModel;
        public virtual PropertiesViewModel Properties { get { if (propertiesViewModel == null) propertiesViewModel = new PropertiesViewModel<MousePositionNode>(this); return propertiesViewModel; } set { if (value == propertiesViewModel) return; propertiesViewModel = value; OnPropertyChanged("PropertiesViewModel"); } }

        private MousePositionNode()
        {
        }
        public virtual void Accept(IVisitNodes visitor)
        {
            visitor.Visit(this);
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }

        public static void RefreshPosition()
        {
            System.Drawing.Point p = MainWindowViewModel.ActiveMain.MainWindowViewModel.SelectedDataViewModel.DataModelView.GetMouseRelPosition();
            Instance._hotspot = new CanvasPoint(p.X, p.Y);

            if (Instance.Connections != null)
            {
                if (Instance.Connections.Count > 0)
                {
                    //RenderPipeline.Render();
                    foreach (IConnection c in Instance.Connections)
                    {
                        if (c is BezierElementViewModel)
                        {
                            BezierElementViewModel b = c as BezierElementViewModel;
                            b.RedrawBezier(b.Origin, b.Destination);
                            if (b.RenderView != null) b.RenderView.Render();
                        }
                    }
                }
            }
        }

        public IElement Parent { get; set; } = default;

        public ElementsLinkedList<IConnection> Connections => connections;

        public NodeType NodeType => NodeType.Unset;

        private CanvasPoint _hotspot = CanvasPoint.Unset;
        public CanvasPoint Hotspot
        {
            get
            {
                return _hotspot;
            }
            private set
            {
                _hotspot = value;
            }
        }


        public Guid ID { get; }

        public ElementState ElementState { get; set; }
        public ElementType ElementType { get => ElementType.Node; set => ElementType = ElementType.Node; }
        public string Name { get; set; } = "";

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
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        ~MousePositionNode() => Dispose();
    }
}