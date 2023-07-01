using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GKYU.CollectionsLibrary.Collections;

namespace GKYU.CollectionsLibrary.Collections.Graphs
{
    public class Edge<T, W>
        where T : IEquatable<T>
    {
        public Node<T> Source { get; set; }
        public Node<T> Target { get; set; }
        public W Weight { get; set; }
        public Edge(Node<T> source, Node<T> target, W weight)
        {
            Source = source;
            Target = target;
            Weight = weight;
        }
    }
}
