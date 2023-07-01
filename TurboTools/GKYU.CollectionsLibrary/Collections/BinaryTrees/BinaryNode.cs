using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GKYU.CollectionsLibrary.Collections;

namespace GKYU.CollectionsLibrary.Collections.BinaryTrees
{
    public class BinaryNode<T>
        : Node<T>
        where T : IEquatable<T>
    {
        public BinaryNode<T> Left
        {
            get
            {
                return null == neighbors ? null : (BinaryNode<T>)neighbors[0];
            }
            set
            {
                if (null == neighbors)
                    neighbors = new NodeList<T>(2);
                neighbors[0] = value;
            }
        }
        public BinaryNode<T> Right
        {
            get
            {
                return null == neighbors ? null : (BinaryNode<T>)neighbors[1];
            }
            set
            {
                if (null == neighbors)
                    neighbors = new NodeList<T>(2);
                neighbors[1] = value;
            }
        }
        public BinaryNode(int nodeID)
            : this(nodeID, default)
        {
        }
        public BinaryNode(int nodeID, T value, BinaryNode<T> left = null, BinaryNode<T> right = null)
            : base(nodeID, value)
        {
            neighbors[0] = left;
            neighbors[1] = right;
        }
    }
}
