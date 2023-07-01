using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.CollectionsLibrary.Collections
{
    public class NodePath<T>
        : IEnumerable<Node<T>>
        where T : IEquatable<T>
    {
        public Stack<Node<T>> path = new Stack<Node<T>>();
        public NodePath()
        {

        }
        public NodePath(IEnumerable<Node<T>> path)
        {
            foreach (Node<T> node in path)
            {
                this.path.Push(node);
            }
        }
        public NodePath(NodePath<T> path)
        {
            this.path = new Stack<Node<T>>(path.path);
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Node<T> node in path)
            {
                sb.Append(node.Value.ToString());
                sb.Append('.');
            }
            if (sb.Length > 0)
                sb.Length--;
            return sb.ToString();
        }
        public void Clear()
        {
            path.Clear();
        }
        public void Push(Node<T> path)
        {
            this.path.Push(path);
        }
        public Node<T> Peek()
        {
            return path.Peek();
        }
        public Node<T> Pop()
        {
            return path.Pop();
        }

        public IEnumerator<Node<T>> GetEnumerator()
        {
            return path.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    /// <summary>
    ///// Pathfinder will only work on Nodes that are acyclic.  An exeption will be thrown for nodes graphs that are acyclic.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PathFinder<T>
        where T : IEquatable<T>
    {
        private List<NodePath<T>> paths = new List<NodePath<T>>();
        private NodePath<T> path = new NodePath<T>();
        private void Traverse(Node<T> node)
        {
            int nodeCount = 0;
            path.Push(node);
            foreach (Node<T> neighbor in node.Neighbors)
            {
                if (path.Contains(neighbor))// bypass 
                    continue;
                nodeCount++;
                Traverse(neighbor);
            }
            if (nodeCount == 0)
            {
                paths.Add(new NodePath<T>(path));
            }
            path.Pop();
        }
        public static IEnumerable<NodePath<T>> GetPaths(Node<T> node)
        {
            PathFinder<T> pathFinder = new PathFinder<T>();
            pathFinder.Traverse(node);
            foreach (NodePath<T> path in pathFinder.paths)
            {
                yield return path;
            }
            yield break;
        }
    }
}
