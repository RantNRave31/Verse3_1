using Core;
using Core.Nodes;

namespace Verse3.Nodes
{
    //[Serializable]
    public class TextDataNode : DataNodeElement<string>
    {
        public TextDataNode(IRenderable parent, NodeType type = NodeType.Unset) : base(parent, type)
        {
        }
        public virtual void Accept(IVisitNodes visitor)
        {
            visitor.Visit(this);
        }
    }
}
