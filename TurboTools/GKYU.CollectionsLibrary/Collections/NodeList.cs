using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.CollectionsLibrary.Collections
{
    public class NodeList<T>
        : ObservableCollection<Node<T>>
    where T : IEquatable<T>
    {
        public NodeList()
            : base()
        {
        }
        public NodeList(NodeList<T> nodes)
            : base(nodes)
        {
        }
        public NodeList(IEnumerable<Node<T>> nodes)
            : base(nodes)
        {
        }

        public NodeList(int initialSize)
        {
            for (int i = 0; i < initialSize; i++)
                Items.Add(default);
        }
        public void AddRange(IEnumerable<Node<T>> nodes)
        {
            foreach (Node<T> node in nodes)
            {
                Add(node);
            }
        }
        public object FindByID(int nodeID)
        {
            foreach (Node<T> node in Items)
                if (node.nodeID.Equals(nodeID))
                    return node;
            return null;
        }
        public object FindByValue(T value)
        {
            foreach (Node<T> node in Items)
                if (node.Value.Equals(value))
                    return node;
            return null;
        }
    }
}
