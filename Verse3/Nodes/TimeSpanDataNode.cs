using Core;
using System;
using Verse3.Components;

namespace Verse3.Nodes
{
    //[Serializable]
    public class TimeSpanDataNode : DataNodeElement<TimeSpan>
    {
        public TimeSpanDataNode(BaseCompViewModel parent, NodeType nodeType) : base(parent, nodeType)
        {
        }
        public virtual void Accept(IVisitNodes visitor)
        {
            visitor.Visit(this);
        }
    }
}
