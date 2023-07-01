using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.CollectionsLibrary.Collections.Graphs
{
    public class EdgeList<T, W>
        : ObservableCollection<Edge<T, W>>
        where T : IEquatable<T>
    {
        public EdgeList()
            : base()
        {
        }
        public EdgeList(EdgeList<T, W> edges)
            : base(edges)
        {
        }
        public EdgeList(IEnumerable<Edge<T, W>> edges)
            : base()
        {
            foreach (Edge<T, W> edge in edges)
            {
                Add(edge);
            }
        }

        public EdgeList(int initialSize)
        {
            for (int i = 0; i < initialSize; i++)
                Items.Add(default);
        }
        public void AddRange(IEnumerable<Edge<T, W>> edges)
        {
            foreach (Edge<T, W> edge in edges)
            {
                Add(edge);
            }
        }
        public object FindByID(int sourceID, int targetID)
        {
            foreach (Edge<T, W> edge in Items)
                if (edge.Source.nodeID.Equals(sourceID) && edge.Target.nodeID.Equals(targetID))
                    return edge;
            return null;
        }
        public object FindByValue(T source, T target)
        {
            foreach (Edge<T, W> edge in Items)
                if (edge.Source.Value.Equals(source) && edge.Target.Value.Equals(target))
                    return edge;
            return null;
        }
    }
}
