using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.CollectionsLibrary.Collections
{
    /// <summary>
    /// WARNING:  Node should only be used in the context of a graph as acyclic neighbors are allowed
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Node<T>
        : IEquatable<Node<T>>
        where T : IEquatable<T>
    {
        public readonly int nodeID;
        private T value;
        protected NodeList<T> neighbors;
        public T Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }
        public NodeList<T> Neighbors
        {
            get
            {
                return neighbors;
            }
        }
        public Node(int nodeID)
            : this(nodeID, default, new NodeList<T>())
        {
        }
        public Node(Node<T> node)
            : this(node.nodeID, node.value, node.neighbors)
        {
        }
        public Node(int nodeID, T value, IEnumerable<Node<T>> neighbors = null)
        {
            this.nodeID = nodeID;
            this.value = value;
            this.neighbors = neighbors == null ? null : new NodeList<T>(neighbors);
        }
        public override string ToString()
        {
            if (value.Equals(default))
                return string.Format("n{0}", nodeID);
            else
                return string.Format("n{0}", value);
        }
        public bool Equals(Node<T> node)
        {
            return nodeID.Equals(node.nodeID);
        }
    }
}
