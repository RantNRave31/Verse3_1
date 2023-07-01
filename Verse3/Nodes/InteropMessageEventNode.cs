using Core;
using Verse3.Elements;

namespace Verse3.Nodes
{
    ////[Serializable]
    //public class ButtonClickedEventNode : EventNodeElement
    //{
    //    public ButtonClickedEventNode(IRenderable parent, NodeType type = NodeType.Unset) : base(parent, type)
    //    {
    //    }

    //    public static implicit operator GenericEventNode(ButtonClickedEventNode v)
    //    {
    //        GenericEventNode outValue = new GenericEventNode(v.Parent, v.NodeType);
    //        outValue = (GenericEventNode)(v as EventNodeElement);
    //        return outValue;
    //    }
    //}
    //[Serializable]
    public class InteropMessageEventNode : EventNodeElementViewModel
    {
        public InteropMessageEventNode(IRenderable parent, NodeType type = NodeType.Unset) : base(parent, type)
        {
        }

        public static implicit operator GenericEventNode(InteropMessageEventNode v)
        {
            GenericEventNode outValue = new GenericEventNode(v.Parent, v.NodeType);
            outValue = (GenericEventNode)(v as EventNodeElementViewModel);
            outValue.EventArgData = v.EventArgData;
            return outValue;
        }
        public override void Accept(IVisitNodes visitor)
        {
            visitor.Visit(this);
        }
    }
}
