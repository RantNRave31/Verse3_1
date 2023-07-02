using Core;
using Core.Nodes;

namespace Verse3.Nodes
{
    //[Serializable]
    public class BooleanDataNode : DataNodeElement<bool>
    {
        public BooleanDataNode(IRenderable parent, NodeType type = NodeType.Unset) : base(parent, type)
        {
        }

        public override void Accept(IVisitNodes visitor)
        {
            visitor.Visit(this);
        }
    }
}
