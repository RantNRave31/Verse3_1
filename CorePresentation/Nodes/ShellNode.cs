using Core;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Verse3.Elements;
using Newtonsoft.Json;
using static Core.Geometry2D;
using Verse3.Components;
using Core.Elements;
using Core.Nodes;

namespace Verse3.Nodes
{
    public class ShellNode : INode, ISerializable
    {
        public SerializationInfo _info;
        public StreamingContext _context;
        protected PropertiesViewModel propertiesViewModel;
        public virtual PropertiesViewModel Properties { get { if (propertiesViewModel == null) propertiesViewModel = new PropertiesViewModel<ShellNode>(this); return propertiesViewModel; } set { if (value == propertiesViewModel) return; propertiesViewModel = value; OnPropertyChanged("PropertiesViewModel"); } }
        public ShellNode()
        {
        }

        public ShellNode(INode node)
        {
            Connections = node.Connections;
            ID = node.ID;
            Name = node.Name;
            Parent = node.Parent;
        }

        public ShellNode(SerializationInfo info, StreamingContext context)
        {
            _info = info;
            _context = context;
            Name = info.GetString("Name");
            ID = Guid.Parse(info.GetString("Id"));
            BezierElements = (ElementsLinkedList<BezierElementViewModel>)info.GetValue("BezierElements", typeof(ElementsLinkedList<BezierElementViewModel>));
            Parent = (IElement)info.GetValue("Parent", typeof(BaseCompViewModel));
        }
        public virtual void Accept(IVisitNodes visitor)
        {
            visitor.Visit(this);
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name);
            info.AddValue("Id", ID.ToString());
            info.AddValue("BezierElements", BezierElements);
            info.AddValue("Parent", Parent);
        }

        [JsonIgnore]
        public IElement Parent { get; set; }

        [JsonIgnore]
        public ElementsLinkedList<IConnection> Connections { get; private set; }

        public ElementsLinkedList<BezierElementViewModel> BezierElements
        {
            get
            {
                ElementsLinkedList<BezierElementViewModel> bezierElements = new ElementsLinkedList<BezierElementViewModel>();
                if (Connections.Count > 0)
                {
                    foreach (IConnection connection in Connections)
                    {
                        if (connection is BezierElementViewModel bezierElement)
                        {
                            bezierElements.Add(bezierElement);
                        }
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
                        Connections.Add(bezierElement);
                    }
                }
            }
        }

        [JsonIgnore]
        public NodeType NodeType => NodeType.Unset;

        [JsonIgnore]
        public CanvasPoint Hotspot { get; private set; }

        public string Name { get; set; }

        [JsonIgnore]
        public ElementType ElementType => ElementType.Node;

        public Guid ID { get; private set; }

        [JsonIgnore]
        public ElementState ElementState { get; set; }
        [JsonIgnore]
        ElementType IElement.ElementType { get; set; }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        ~ShellNode() => Dispose();

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
    }

    //public enum CompOrientation
    //{
    //    Horizontal,
    //    Vertical
    //}

}
