using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verse3.Nodes
{
    public interface IVisitNodes
    {
        void Visit(BooleanDataNode node);
        void Visit<T>(DataNode<T> node);
        void Visit(DateTimeDataNode node);
        void Visit(EventNode node);
        void Visit(GenericDataNode node);
        void Visit(GenericEventNode node);
        void Visit(InteropMessageEventNode node);
        void Visit(MousePositionNode node);
        void Visit(NumberDataNode node);
        void Visit(ShellNode node);
        void Visit(TextDataNode node);
        void Visit(TimeSpanDataNode node);
        void Visit(TimeZoneDataNode node);
    }
    public class NodeVisitor
        : IVisitNodes
    {
        public void Visit(BooleanDataNode node)
        {
            
        }

        public void Visit<T>(DataNode<T> node)
        {
            
        }

        public void Visit(DateTimeDataNode node)
        {
            
        }

        public void Visit(EventNode node)
        {
            
        }

        public void Visit(GenericDataNode node)
        {
            
        }

        public void Visit(GenericEventNode node)
        {
            
        }

        public void Visit(InteropMessageEventNode node)
        {
            
        }

        public void Visit(MousePositionNode node)
        {
            
        }

        public void Visit(NumberDataNode node)
        {
            
        }

        public void Visit(ShellNode node)
        {
            
        }

        public void Visit(TextDataNode node)
        {
            
        }

        public void Visit(TimeSpanDataNode node)
        {
            
        }

        public void Visit(TimeZoneDataNode node)
        {
            
        }
    }
}
