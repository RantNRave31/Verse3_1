using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.CollectionsLibrary.Collections.BinaryTrees
{
    public class RedBlackTree<KEY, DATA> : object
        where KEY : IComparable<KEY>
    {
        public class Exception : System.Exception
        {
            public Exception()
            {
            }

            public Exception(string msg) : base(msg)
            {
            }
        }
        public class Enumerator
            : IEnumerator<Node>
        {
            // the treap uses the stack to order the nodes
            private Stack<Node> stack;
            // return the keys
            private bool keys;
            // return in ascending order (true) or descending (false)
            private bool ascending;

            protected Node _current;
            public Node Current
            {
                get
                {
                    return _current;
                }
            }
            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            public Enumerator()
            {
            }
            ///<summary>
            /// Determine order, walk the tree and push the nodes onto the stack
            ///</summary>
            public Enumerator(Node tnode, bool keys, bool ascending)
            {

                stack = new Stack<Node>();
                this.keys = keys;
                this.ascending = ascending;

                // use depth-first traversal to push nodes into stack
                // the lowest node will be at the top of the stack
                if (ascending)
                {   // find the lowest node
                    while (tnode != RedBlackTree<KEY, DATA>.sentinelNode)
                    {
                        stack.Push(tnode);
                        tnode = tnode.Left;
                    }
                }
                else
                {
                    // the highest node will be at top of stack
                    while (tnode != RedBlackTree<KEY, DATA>.sentinelNode)
                    {
                        stack.Push(tnode);
                        tnode = tnode.Right;
                    }
                }

            }
            public void Dispose()
            {

            }
            public void Reset()
            {
                throw new NotImplementedException();
            }
            ///<summary>
            /// HasMoreElements
            ///</summary>
            public bool HasMoreElements()
            {
                return stack.Count > 0;
            }
            ///<summary>
            /// NextElement
            ///</summary>
            public object NextElement()
            {
                if (stack.Count == 0)
                    throw new Exception("Element not found");

                // the top of stack will always have the next item
                // get top of stack but don't remove it as the next nodes in sequence
                // may be pushed onto the top
                // the stack will be popped after all the nodes have been returned
                Node node = stack.Peek(); //next node in sequence

                if (ascending)
                {
                    if (node.Right == RedBlackTree<KEY, DATA>.sentinelNode)
                    {
                        // yes, top node is lowest node in subtree - pop node off stack 
                        Node tn = stack.Pop();
                        // peek at right node's parent 
                        // get rid of it if it has already been used
                        while (HasMoreElements() && stack.Peek().Right == tn)
                            tn = stack.Pop();
                    }
                    else
                    {
                        // find the next items in the sequence
                        // traverse to left; find lowest and push onto stack
                        Node tn = node.Right;
                        while (tn != RedBlackTree<KEY, DATA>.sentinelNode)
                        {
                            stack.Push(tn);
                            tn = tn.Left;
                        }
                    }
                }
                else            // descending, same comments as above apply
                {
                    if (node.Left == RedBlackTree<KEY, DATA>.sentinelNode)
                    {
                        // walk the tree
                        Node tn = stack.Pop();
                        while (HasMoreElements() && stack.Peek().Left == tn)
                            tn = stack.Pop();
                    }
                    else
                    {
                        // determine next node in sequence
                        // traverse to left subtree and find greatest node - push onto stack
                        Node tn = node.Left;
                        while (tn != RedBlackTree<KEY, DATA>.sentinelNode)
                        {
                            stack.Push(tn);
                            tn = tn.Right;
                        }
                    }
                }
                _current = node;
                if (node.Color == 0)                     // testing only
                    Current.Color = 0;
                else
                    Current.Color = 1;
                return keys == true ? node.Key : node.Data;
            }
            ///<summary>
            /// MoveNext
            /// For .NET compatibility
            ///</summary>
            public bool MoveNext()
            {
                if (HasMoreElements())
                {
                    NextElement();
                    return true;
                }
                return false;
            }


        }
        public class Node
        {
            // tree node colors
            public static int RED = 0;
            public static int BLACK = 1;

            // key provided by the calling class
            private KEY ordKey;
            // the data or value associated with the key
            private DATA objData;
            // color - used to balance the tree
            private int intColor;
            // left node 
            private Node rbnLeft;
            // right node 
            private Node rbnRight;
            // parent node 
            private Node rbnParent;

            ///<summary>
            ///Key
            ///</summary>
            public KEY Key
            {
                get
                {
                    return ordKey;
                }

                set
                {
                    ordKey = value;
                }
            }
            ///<summary>
            ///Data
            ///</summary>
            public DATA Data
            {
                get
                {
                    return objData;
                }

                set
                {
                    objData = value;
                }
            }
            ///<summary>
            ///Color
            ///</summary>
            public int Color
            {
                get
                {
                    return intColor;
                }

                set
                {
                    intColor = value;
                }
            }
            ///<summary>
            ///Left
            ///</summary>
            public Node Left
            {
                get
                {
                    return rbnLeft;
                }

                set
                {
                    rbnLeft = value;
                }
            }
            ///<summary>
            /// Right
            ///</summary>
            public Node Right
            {
                get
                {
                    return rbnRight;
                }

                set
                {
                    rbnRight = value;
                }
            }
            public Node Parent
            {
                get
                {
                    return rbnParent;
                }

                set
                {
                    rbnParent = value;
                }
            }

            public Node()
            {
                Color = RED;
            }
        }
        public class Generator
        {
            protected readonly StreamWriter _streamWriter;
            protected readonly RedBlackTree<KEY, DATA> _redBlackTree;
            public Generator(StreamWriter streamWriter, RedBlackTree<KEY, DATA> redBlackTree)
            {
                _streamWriter = streamWriter;
                _redBlackTree = redBlackTree;
            }
            public void DumpMinMaxValue()
            {
                _streamWriter.WriteLine("** Dumping Min/Max Values  **");
                _streamWriter.WriteLine("Min MyObj value: " + (DATA)_redBlackTree.GetMinValue());
                _streamWriter.WriteLine("Max MyObj value: " + (DATA)_redBlackTree.GetMaxValue());
                _streamWriter.WriteLine("Min MyObj key: " + _redBlackTree.GetMinKey().ToString());
                _streamWriter.WriteLine("Max MyObj key: " + _redBlackTree.GetMaxKey().ToString());
            }
            public void DumpRedBlackTree(bool boolDesc)
            {
                // returns keys only
                RedBlackTree<KEY, DATA>.Enumerator k = _redBlackTree.Keys(boolDesc);
                // returns data only, in this case, MyObjs
                RedBlackTree<KEY, DATA>.Enumerator e = _redBlackTree.Elements(boolDesc);

                if (boolDesc)
                    _streamWriter.WriteLine("** Dumping RedBlack: Ascending **");
                else
                    _streamWriter.WriteLine("** Dumping RedBlack: Descending **");

                _streamWriter.WriteLine("RedBlack Size: " + _redBlackTree.Size().ToString() + Environment.NewLine);

                _streamWriter.WriteLine("- keys -");
                while (k.HasMoreElements())
                    _streamWriter.WriteLine(k.NextElement());

                _streamWriter.WriteLine("- my objects -");
                DATA cmmMyObj;
                while (e.HasMoreElements())
                {
                    cmmMyObj = (DATA)e.NextElement();
                    _streamWriter.Write("Key:" + cmmMyObj.ToString());
                    _streamWriter.WriteLine(" Data:" + cmmMyObj);
                }
            }
            public void Visit(RedBlackTree<KEY, DATA>.Node node)
            {
                _streamWriter.WriteLine("Key:{0}\t" + "  Data:{1}\t" + " Color:{2}\t" + " Parent:{3}", node.Key, node.Data, node.Color, node.Parent == null ? default : node.Parent.Key);
            }
            public void Visit(RedBlackTree<KEY, DATA> redBlackTree)
            {
                _streamWriter.WriteLine("** Traversing using Enumerator **");
                _streamWriter.WriteLine(Environment.NewLine);

                _streamWriter.WriteLine("To verify, compare against: " +
                    "http://www.ececs.uc.edu/~franco/C321/html/RedBlack/redblack.html");
                _streamWriter.WriteLine(Environment.NewLine);

                foreach (Node node in redBlackTree)
                    Visit(node);
            }
            public void Generate()
            {
                Visit(_redBlackTree);
            }
        }
        // the number of nodes contained in the tree
        private int intCount;
        // a simple randomized hash code. The hash code could be used as a key
        // if it is "unique" enough. Note: The IComparable interface would need to 
        // be replaced with int.
        private int intHashCode;
        // identifies the owner of the tree
        private string strIdentifier;
        // the tree
        private Node rbTree;
        //  sentinelNode is convenient way of indicating a leaf node.
        public static Node sentinelNode;
        // the node that was last found; used to optimize searches
        private Node lastNodeFound;
        private Random rand = new Random();

        public RedBlackTree()
        {
            strIdentifier = base.ToString() + rand.Next();
            intHashCode = rand.Next();

            // set up the sentinel node. the sentinel node is the key to a successfull
            // implementation and for understanding the red-black tree properties.
            sentinelNode = new Node();
            sentinelNode.Left = null;
            sentinelNode.Right = null;
            sentinelNode.Parent = null;
            sentinelNode.Color = Node.BLACK;
            rbTree = sentinelNode;
            lastNodeFound = sentinelNode;
        }

        public RedBlackTree(string strIdentifier)
        {
            intHashCode = rand.Next();
            this.strIdentifier = strIdentifier;
        }

        ///<summary>
        /// Add
        /// args: ByVal key As IComparable, ByVal data As Object
        /// key is object that implements IComparable interface
        /// performance tip: change to use use int type (such as the hashcode)
        ///</summary>
        public void Add(KEY key, DATA data)
        {
            if (key == null || data == null)
                throw new Exception("RedBlackNode key and data must not be null");

            // traverse tree - find where node belongs
            int result = 0;
            // create new node
            Node node = new Node();
            Node temp = rbTree;             // grab the rbTree node of the tree

            while (temp != sentinelNode)
            {   // find Parent
                node.Parent = temp;
                result = key.CompareTo(temp.Key);
                if (result == 0)
                    throw new Exception("A Node with the same key already exists");
                if (result > 0)
                    temp = temp.Right;
                else
                    temp = temp.Left;
            }

            // setup node
            node.Key = key;
            node.Data = data;
            node.Left = sentinelNode;
            node.Right = sentinelNode;

            // insert node into tree starting at parent's location
            if (node.Parent != null)
            {
                result = node.Key.CompareTo(node.Parent.Key);
                if (result > 0)
                    node.Parent.Right = node;
                else
                    node.Parent.Left = node;
            }
            else
                rbTree = node;					// first node added

            RestoreAfterInsert(node);           // restore red-black properities

            lastNodeFound = node;

            intCount = intCount + 1;
        }
        ///<summary>
        /// RestoreAfterInsert
        /// Additions to red-black trees usually destroy the red-black 
        /// properties. Examine the tree and restore. Rotations are normally 
        /// required to restore it
        ///</summary>
		private void RestoreAfterInsert(Node x)
        {
            // x and y are used as variable names for brevity, in a more formal
            // implementation, you should probably change the names

            Node y;

            // maintain red-black tree properties after adding x
            while (x != rbTree && x.Parent.Color == Node.RED)
            {
                // Parent node is .Colored red; 
                if (x.Parent == x.Parent.Parent.Left)   // determine traversal path			
                {                                       // is it on the Left or Right subtree?
                    y = x.Parent.Parent.Right;          // get uncle
                    if (y != null && y.Color == Node.RED)
                    {   // uncle is red; change x's Parent and uncle to black
                        x.Parent.Color = Node.BLACK;
                        y.Color = Node.BLACK;
                        // grandparent must be red. Why? Every red node that is not 
                        // a leaf has only black children 
                        x.Parent.Parent.Color = Node.RED;
                        x = x.Parent.Parent;    // continue loop with grandparent
                    }
                    else
                    {
                        // uncle is black; determine if x is greater than Parent
                        if (x == x.Parent.Right)
                        {   // yes, x is greater than Parent; rotate Left
                            // make x a Left child
                            x = x.Parent;
                            RotateLeft(x);
                        }
                        // no, x is less than Parent
                        x.Parent.Color = Node.BLACK;    // make Parent black
                        x.Parent.Parent.Color = Node.RED;       // make grandparent black
                        RotateRight(x.Parent.Parent);                   // rotate right
                    }
                }
                else
                {   // x's Parent is on the Right subtree
                    // this code is the same as above with "Left" and "Right" swapped
                    y = x.Parent.Parent.Left;
                    if (y != null && y.Color == Node.RED)
                    {
                        x.Parent.Color = Node.BLACK;
                        y.Color = Node.BLACK;
                        x.Parent.Parent.Color = Node.RED;
                        x = x.Parent.Parent;
                    }
                    else
                    {
                        if (x == x.Parent.Left)
                        {
                            x = x.Parent;
                            RotateRight(x);
                        }
                        x.Parent.Color = Node.BLACK;
                        x.Parent.Parent.Color = Node.RED;
                        RotateLeft(x.Parent.Parent);
                    }
                }
            }
            rbTree.Color = Node.BLACK;      // rbTree should always be black
        }

        ///<summary>
        /// RotateLeft
        /// Rebalance the tree by rotating the nodes to the left
        ///</summary>
        public void RotateLeft(Node x)
        {
            // pushing node x down and to the Left to balance the tree. x's Right child (y)
            // replaces x (since y > x), and y's Left child becomes x's Right child 
            // (since it's < y but > x).

            Node y = x.Right;           // get x's Right node, this becomes y

            // set x's Right link
            x.Right = y.Left;                   // y's Left child's becomes x's Right child

            // modify parents
            if (y.Left != sentinelNode)
                y.Left.Parent = x;				// sets y's Left Parent to x

            if (y != sentinelNode)
                y.Parent = x.Parent;            // set y's Parent to x's Parent

            if (x.Parent != null)
            {   // determine which side of it's Parent x was on
                if (x == x.Parent.Left)
                    x.Parent.Left = y;          // set Left Parent to y
                else
                    x.Parent.Right = y;         // set Right Parent to y
            }
            else
                rbTree = y;                     // at rbTree, set it to y

            // link x and y 
            y.Left = x;                         // put x on y's Left 
            if (x != sentinelNode)                      // set y as x's Parent
                x.Parent = y;
        }
        ///<summary>
        /// RotateRight
        /// Rebalance the tree by rotating the nodes to the right
        ///</summary>
        public void RotateRight(Node x)
        {
            // pushing node x down and to the Right to balance the tree. x's Left child (y)
            // replaces x (since x < y), and y's Right child becomes x's Left child 
            // (since it's < x but > y).

            Node y = x.Left;            // get x's Left node, this becomes y

            // set x's Right link
            x.Left = y.Right;                   // y's Right child becomes x's Left child

            // modify parents
            if (y.Right != sentinelNode)
                y.Right.Parent = x;				// sets y's Right Parent to x

            if (y != sentinelNode)
                y.Parent = x.Parent;            // set y's Parent to x's Parent

            if (x.Parent != null)               // null=rbTree, could also have used rbTree
            {   // determine which side of it's Parent x was on
                if (x == x.Parent.Right)
                    x.Parent.Right = y;         // set Right Parent to y
                else
                    x.Parent.Left = y;          // set Left Parent to y
            }
            else
                rbTree = y;                     // at rbTree, set it to y

            // link x and y 
            y.Right = x;                        // put x on y's Right
            if (x != sentinelNode)              // set y as x's Parent
                x.Parent = y;
        }
        ///<summary>
        /// GetData
        /// Gets the data object associated with the specified key
        ///<summary>
        public object GetData(KEY key)
        {
            int result;

            Node treeNode = rbTree;     // begin at root

            // traverse tree until node is found
            while (treeNode != sentinelNode)
            {
                result = key.CompareTo(treeNode.Key);
                if (result == 0)
                {
                    lastNodeFound = treeNode;
                    return treeNode.Data;
                }
                if (result < 0)
                    treeNode = treeNode.Left;
                else
                    treeNode = treeNode.Right;
            }

            throw new Exception("RedBlackNode key was not found");
        }
        ///<summary>
        /// GetMinKey
        /// Returns the minimum key value
        ///<summary>
        public KEY GetMinKey()
        {
            Node treeNode = rbTree;

            if (treeNode == null || treeNode == sentinelNode)
                throw new Exception("RedBlack tree is empty");

            // traverse to the extreme left to find the smallest key
            while (treeNode.Left != sentinelNode)
                treeNode = treeNode.Left;

            lastNodeFound = treeNode;

            return treeNode.Key;

        }
        ///<summary>
        /// GetMaxKey
        /// Returns the maximum key value
        ///<summary>
        public KEY GetMaxKey()
        {
            Node treeNode = rbTree;

            if (treeNode == null || treeNode == sentinelNode)
                throw new Exception("RedBlack tree is empty");

            // traverse to the extreme right to find the largest key
            while (treeNode.Right != sentinelNode)
                treeNode = treeNode.Right;

            lastNodeFound = treeNode;

            return treeNode.Key;

        }
        ///<summary>
        /// GetMinValue
        /// Returns the object having the minimum key value
        ///<summary>
        public object GetMinValue()
        {
            return GetData(GetMinKey());
        }
        ///<summary>
        /// GetMaxValue
        /// Returns the object having the maximum key
        ///<summary>
        public object GetMaxValue()
        {
            return GetData(GetMaxKey());
        }
        ///<summary>
        /// GetEnumerator
        /// return an enumerator that returns the tree nodes in order
        ///<summary>
        public Enumerator GetEnumerator()
        {
            // elements is simply a generic name to refer to the 
            // data objects the nodes contain
            return Elements(true);
        }
        ///<summary>
        /// Keys
        /// if(ascending is true, the keys will be returned in ascending order, else
        /// the keys will be returned in descending order.
        ///<summary>
        public Enumerator Keys()
        {
            return Keys(true);
        }
        public Enumerator Keys(bool ascending)
        {
            return new Enumerator(rbTree, true, ascending);
        }
        ///<summary>
        /// Values
        /// Provided for .NET compatibility. 
        ///<summary>
        public Enumerator Values()
        {
            return Elements(true);
        }
        ///<summary>
        /// Elements
        /// Returns an enumeration of the data objects.
        /// if(ascending is true, the objects will be returned in ascending order,
        /// else the objects will be returned in descending order.
        ///<summary>
        public Enumerator Elements()
        {
            return Elements(true);
        }
        public Enumerator Elements(bool ascending)
        {
            return new Enumerator(rbTree, false, ascending);
        }
        ///<summary>
        /// IsEmpty
        /// Is the tree empty?
        ///<summary>
        public bool IsEmpty()
        {
            return rbTree == null;
        }
        ///<summary>
        /// Remove
        /// removes the key and data object (delete)
        ///<summary>
        public void Remove(KEY key)
        {
            if (key == null)
                throw new Exception("RedBlackNode key is null");

            // find node
            int result;
            Node node;

            // see if node to be deleted was the last one found
            result = key.CompareTo(lastNodeFound.Key);
            if (result == 0)
                node = lastNodeFound;
            else
            {   // not found, must search		
                node = rbTree;
                while (node != sentinelNode)
                {
                    result = key.CompareTo(node.Key);
                    if (result == 0)
                        break;
                    if (result < 0)
                        node = node.Left;
                    else
                        node = node.Right;
                }

                if (node == sentinelNode)
                    return;             // key not found
            }

            Delete(node);

            intCount = intCount - 1;
        }
        ///<summary>
        /// Delete
        /// Delete a node from the tree and restore red black properties
        ///<summary>
        private void Delete(Node z)
        {
            // A node to be deleted will be: 
            //		1. a leaf with no children
            //		2. have one child
            //		3. have two children
            // If the deleted node is red, the red black properties still hold.
            // If the deleted node is black, the tree needs rebalancing

            Node x = new Node();    // work node to contain the replacement node
            Node y;                 // work node 

            // find the replacement node (the successor to x) - the node one with 
            // at *most* one child. 
            if (z.Left == sentinelNode || z.Right == sentinelNode)
                y = z;                      // node has sentinel as a child
            else
            {
                // z has two children, find replacement node which will 
                // be the leftmost node greater than z
                y = z.Right;                        // traverse right subtree	
                while (y.Left != sentinelNode)      // to find next node in sequence
                    y = y.Left;
            }

            // at this point, y contains the replacement node. it's content will be copied 
            // to the valules in the node to be deleted

            // x (y's only child) is the node that will be linked to y's old parent. 
            if (y.Left != sentinelNode)
                x = y.Left;
            else
                x = y.Right;

            // replace x's parent with y's parent and
            // link x to proper subtree in parent
            // this removes y from the chain
            x.Parent = y.Parent;
            if (y.Parent != null)
                if (y == y.Parent.Left)
                    y.Parent.Left = x;
                else
                    y.Parent.Right = x;
            else
                rbTree = x;         // make x the root node

            // copy the values from y (the replacement node) to the node being deleted.
            // note: this effectively deletes the node. 
            if (y != z)
            {
                z.Key = y.Key;
                z.Data = y.Data;
            }

            if (y.Color == Node.BLACK)
                RestoreAfterDelete(x);

            lastNodeFound = sentinelNode;
        }

        ///<summary>
        /// RestoreAfterDelete
        /// Deletions from red-black trees may destroy the red-black 
        /// properties. Examine the tree and restore. Rotations are normally 
        /// required to restore it
        ///</summary>
		private void RestoreAfterDelete(Node x)
        {
            // maintain Red-Black tree balance after deleting node 			

            Node y;

            while (x != rbTree && x.Color == Node.BLACK)
            {
                if (x == x.Parent.Left)         // determine sub tree from parent
                {
                    y = x.Parent.Right;         // y is x's sibling 
                    if (y.Color == Node.RED)
                    {   // x is black, y is red - make both black and rotate
                        y.Color = Node.BLACK;
                        x.Parent.Color = Node.RED;
                        RotateLeft(x.Parent);
                        y = x.Parent.Right;
                    }
                    if (y.Left.Color == Node.BLACK &&
                        y.Right.Color == Node.BLACK)
                    {   // children are both black
                        y.Color = Node.RED;     // change parent to red
                        x = x.Parent;                   // move up the tree
                    }
                    else
                    {
                        if (y.Right.Color == Node.BLACK)
                        {
                            y.Left.Color = Node.BLACK;
                            y.Color = Node.RED;
                            RotateRight(y);
                            y = x.Parent.Right;
                        }
                        y.Color = x.Parent.Color;
                        x.Parent.Color = Node.BLACK;
                        y.Right.Color = Node.BLACK;
                        RotateLeft(x.Parent);
                        x = rbTree;
                    }
                }
                else
                {   // right subtree - same as code above with right and left swapped
                    y = x.Parent.Left;
                    if (y.Color == Node.RED)
                    {
                        y.Color = Node.BLACK;
                        x.Parent.Color = Node.RED;
                        RotateRight(x.Parent);
                        y = x.Parent.Left;
                    }
                    if (y.Right.Color == Node.BLACK &&
                        y.Left.Color == Node.BLACK)
                    {
                        y.Color = Node.RED;
                        x = x.Parent;
                    }
                    else
                    {
                        if (y.Left.Color == Node.BLACK)
                        {
                            y.Right.Color = Node.BLACK;
                            y.Color = Node.RED;
                            RotateLeft(y);
                            y = x.Parent.Left;
                        }
                        y.Color = x.Parent.Color;
                        x.Parent.Color = Node.BLACK;
                        y.Left.Color = Node.BLACK;
                        RotateRight(x.Parent);
                        x = rbTree;
                    }
                }
            }
            x.Color = Node.BLACK;
        }

        ///<summary>
        /// RemoveMin
        /// removes the node with the minimum key
        ///<summary>
        public void RemoveMin()
        {
            if (rbTree == null)
                throw new Exception("RedBlackNode is null");

            Remove(GetMinKey());
        }
        ///<summary>
        /// RemoveMax
        /// removes the node with the maximum key
        ///<summary>
        public void RemoveMax()
        {
            if (rbTree == null)
                throw new Exception("RedBlackNode is null");

            Remove(GetMaxKey());
        }
        ///<summary>
        /// Clear
        /// Empties or clears the tree
        ///<summary>
        public void Clear()
        {
            rbTree = sentinelNode;
            intCount = 0;
        }
        ///<summary>
        /// Size
        /// returns the size (number of nodes) in the tree
        ///<summary>
        public int Size()
        {
            // number of keys
            return intCount;
        }
        ///<summary>
        /// Equals
        ///<summary>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (!(obj is Node))
                return false;

            if (this == obj)
                return true;

            return ToString().Equals(((Node)obj).ToString());

        }
        ///<summary>
        /// HashCode
        ///<summary>
        public override int GetHashCode()
        {
            return intHashCode;
        }
        ///<summary>
        /// ToString
        ///<summary>
        public override string ToString()
        {
            return strIdentifier.ToString();
        }
    }
}
