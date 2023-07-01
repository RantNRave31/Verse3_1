using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.CollectionsLibrary.Collections.Graphs
{
    public class UndirectedGraph<T, W>
        : Graph<T, W>
        where T : IEquatable<T>
        where W : IComparable<W>
    {
        public new void AddEdge(Node first, Node second, W cost = default)
        {
            base.AddEdge(first, second, cost);
            base.AddEdge(second, first, cost);
        }
        public new void AddEdge(T first, T second, W cost = default)
        {
            base.AddEdge(first, second, cost);
            base.AddEdge(second, first, cost);
        }

    }
}
