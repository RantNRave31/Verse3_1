using Core;
using System.Collections.Generic;
using System.Text;

namespace Verse3.Nodes
{
    //[Serializable]
    public class GenericDataNode : DataNodeElement<object>
    {
        public GenericDataNode(IRenderable parent, NodeType type = NodeType.Unset) : base(parent, type)
        {
        }
        public override void Accept(IVisitNodes visitor)
        {
            visitor.Visit(this);
        }
    }
}
