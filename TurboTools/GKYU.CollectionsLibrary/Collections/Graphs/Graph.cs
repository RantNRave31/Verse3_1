using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.CollectionsLibrary.Collections.Graphs
{
    using GKYU.CollectionsLibrary.Collections;

    /// <summary>
    /// i use Node[0] to be the root node
    /// WARNING:  if adding an edge by value, it graphs the FIRST.  node.value is  not unique to graph, you get undefined behavior
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Graph<T, W>
        where T : IEquatable<T>
        where W : IComparable<W>
    {
        public Dictionary<W, string> vertexLabels = new Dictionary<W, string>();
        public Dictionary<W, string> edgeLabels = new Dictionary<W, string>();
        public class Node
            : Node<T>
        {
            protected EdgeList<T, W> edges;
            public EdgeList<T, W> Edges
            {
                get
                {
                    return edges;
                }
            }
            public Node(int nodeID)
                : this(nodeID, default)
            {
                edges = new EdgeList<T, W>();
            }
            public Node(int nodeID, T value, NodeList<T> neighbors = null, EdgeList<T, W> edges = null)
                : base(nodeID, value, null == neighbors ? new NodeList<T>() : neighbors)
            {
                this.edges = edges == null ? new EdgeList<T, W>() : edges;
            }
            public bool Equals(Node node)
            {
                return nodeID.Equals(node.nodeID);
            }
        }
        protected NodeList<T> verticies;
        public NodeList<T> Verticies
        {
            get
            {
                return verticies;
            }
        }
        protected EdgeList<T, W> edges;
        public EdgeList<T, W> Edges
        {
            get
            {
                return edges;
            }
        }
        public Graph(params Node[] verticies)
        {
            this.verticies = null == verticies ? new NodeList<T>() : new NodeList<T>(verticies);
            edges = new EdgeList<T, W>();
        }
        public Graph(IEnumerable<Node> verticies = null, IEnumerable<Edge<T, W>> edges = null)
        {
            this.verticies = null == verticies ? new NodeList<T>() : new NodeList<T>(verticies);
            this.edges = null == edges ? new EdgeList<T, W>() : new EdgeList<T, W>(edges);
        }
        public virtual Node CreateNode(T value = default)
        {
            Node newNode = new Node(verticies.Count, value);
            verticies.Add(newNode);
            return newNode;
        }
        public virtual Edge<T, W> CreateEdge(Node first, Node second, W weight = default)
        {
            Edge<T, W> newEdge = new Edge<T, W>(first, second, weight);
            edges.Add(newEdge);
            return newEdge;
        }
        protected void AddNeighbor(Node first, Node second)
        {
            first.Neighbors.Add(second);
        }
        protected void AddNeighbor(T first, T second)
        {
            Node firstNode = (Node)verticies.FindByValue(first);
            Node secondNode = (Node)verticies.FindByValue(second);
            if (firstNode == null)
                firstNode = CreateNode(first);
            if (secondNode == null)
                secondNode = CreateNode(second);
            firstNode.Neighbors.Add(secondNode);
        }
        protected void AddNeighbors(Node first, Node second)
        {
            AddNeighbor(first, second);
            AddNeighbor(second, first);
        }
        protected void AddNeighbors(T first, T second)
        {
            AddNeighbor(first, second);
            AddNeighbor(second, first);
        }
        public void AddEdge(Node first, Node second, W weight = default)
        {
            Edge<T, W> edge;
            Edge<T, W> childEdge;
            AddNeighbor(first, second);
            edge = (Edge<T, W>)Edges.FindByID(first.nodeID, second.nodeID);
            if (null == edge || null != edge && !edge.Weight.Equals(weight))
                Edges.Add(edge = new Edge<T, W>(first, second, weight));
            childEdge = (Edge<T, W>)first.Edges.FindByID(first.nodeID, second.nodeID);
            if (null == childEdge || null != childEdge && !childEdge.Weight.Equals(weight))
                first.Edges.Add(edge);
        }
        public void AddEdge(T first, T second, W weight = default)
        {
            Node firstNode = (Node)verticies.FindByValue(first);
            Node secondNode = (Node)verticies.FindByValue(second);
            if (firstNode == null)
                firstNode = CreateNode(first);
            if (secondNode == null)
                secondNode = CreateNode(second);
            AddEdge(firstNode, secondNode, weight);
        }
        /// <summary>
        /// Code adapted from Dr. James McCaffrey
        /// https://msdn.microsoft.com/en-us/magazine/dn198246.aspx?f=255&MSPPError=-2147217396
        /// implements dykstras algorithm for shortest path</summary>
        /// <param name="startNodeID"></param>
        /// <param name="endNodeID"></param>
        /// <returns></returns>
        public IEnumerable<Tuple<Node, W>> ShortestPath(Node startNode, Node endNode, Func<W, W, W> fAdd, W maxDistance)
        {
            Dictionary<Node, W> distance = new Dictionary<Node, W>();
            Dictionary<Node, Node> previous = new Dictionary<Node, Node>();
            HashSet<Node> beenAdded = new HashSet<Node>();
            PriorityQueue<Node, W> priorityQueue = new PriorityQueue<Node, W>();
            int maxNodes = verticies.Count;

            distance[startNode] = default;
            previous[startNode] = default;
            priorityQueue.Enqueue(new Tuple<Node, W>(startNode, default));
            beenAdded.Add(startNode);

            W alt = default;

            while (priorityQueue.Count() > 0 && beenAdded.Count < maxNodes)
            {
                Tuple<Node, W> u = priorityQueue.Dequeue();
                if (u.Item1.Equals(endNode))
                    break;
                foreach (Node v in u.Item1.Neighbors)
                {
                    if (!beenAdded.Contains(v))
                    {
                        distance[v] = maxDistance;
                        previous[v] = default;
                        priorityQueue.Enqueue(new Tuple<Node, W>(v, maxDistance));
                        beenAdded.Add(v);
                    }
                    Edge<T, W> edge = (Edge<T, W>)Edges.FindByID(u.Item1.nodeID, v.nodeID);
                    //alt = distance[u.Item1] + edge.Cost;
                    alt = fAdd(distance[u.Item1], edge.Weight);
                    if (alt.CompareTo(distance[v]) < 0)
                    {
                        distance[v] = alt;
                        previous[v] = u.Item1;
                        priorityQueue.ChangePriority(v, alt);
                    }
                }
            }
            Stack<Tuple<Node, W>> results = new Stack<Tuple<Node, W>>();
            if (distance.ContainsKey(endNode))
            {
                W sp = distance[endNode];
                if (!sp.Equals(maxDistance))
                {
                    Node currentNode = endNode;
                    while (currentNode != startNode)
                    {
                        // pathResult += currentNode.Value.ToString() + ";";
                        results.Push(new Tuple<Node, W>(currentNode, distance[currentNode]));
                        currentNode = previous[currentNode];
                    }
                    results.Push(new Tuple<Node, W>(startNode, distance[startNode]));
                }
            }
            while (results.Count > 0)
            {
                yield return results.Pop();
            }
            yield break;
        }
        public IEnumerable<Tuple<Node, W>> ShortestPath(T startValue, T endValue, Func<W, W, W> fAdd, W maxDistance)
        {
            Node startNode = (Node)Verticies.FindByValue(startValue);
            Node endNode = (Node)Verticies.FindByValue(endValue);
            return ShortestPath(startNode, endNode, fAdd, maxDistance);
        }
    }
}
