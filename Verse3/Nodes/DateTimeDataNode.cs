using Core.Nodes;
using System;
using Verse3.Components;

namespace Verse3.Nodes
{
    //[Serializable]
    public class DateTimeDataNode : DataNodeElement<DateTime>
    {
        public DateTimeDataNode(BaseCompViewModel parent, NodeType nodeType) : base(parent, nodeType)
        {
        }
        public override void Accept(IVisitNodes visitor)
        {
            visitor.Visit(this);
        }
    }
}
