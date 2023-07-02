using Core.Nodes;
using System;
using Verse3.Components;

namespace Verse3.Nodes
{
    //[Serializable]
    public class TimeZoneDataNode : DataNodeElement<TimeZoneInfo>
    {
        public TimeZoneDataNode(BaseCompViewModel parent, NodeType nodeType) : base(parent, nodeType)
        {
        }
        public virtual void Accept(IVisitNodes visitor)
        {
            visitor.Visit(this);
        }
    }
}
