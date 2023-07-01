using Core;
using Verse3.Elements;

namespace Verse3.Nodes
{
    //[Serializable]
    public class GenericEventNode : EventNodeElementViewModel
    {
        public GenericEventNode(IRenderable parent, NodeType type = NodeType.Unset) : base(parent, type)
        {
        }
        public override void Accept(IVisitNodes visitor)
        {
            visitor.Visit(this);
        }
    }
}
