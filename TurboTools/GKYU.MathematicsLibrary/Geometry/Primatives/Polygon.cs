using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;


using GKYU.MathLibrary.Tensors.Vectors;

namespace GKYU.MathLibrary.Geometry.Primatives
{
    public interface IPolygon
    {
        IList<Vector3D> Points { get; }
        IList<ILineSegment> Edges { get; }
        Plane Plane { get; }
        IPolygon Flipped();
    }
    public abstract class BasePolygon
                : IPolygon
    {
        public IList<Vector3D> Points { get; protected set; }
        public IList<ILineSegment> Edges { get; protected set; }

        public Plane Plane
        {
            get
            {
                return Plane.FromPoints(Points[0], Points[1], Points[2]);
            }
        }

        protected BasePolygon(IList<Vector3D> points, IList<ILineSegment> edges)
        {

            Points = new ReadOnlyCollection<Vector3D>(points);
            Edges = new ReadOnlyCollection<ILineSegment>(edges);
        }

        protected BasePolygon(IList<Vector3D> points)
        {
            Points = new ReadOnlyCollection<Vector3D>(points);
            var edges = new List<ILineSegment>(points.Zip(points.Skip(1), (a, b) => new LineSegment(a, b)))
            {
                new LineSegment(points.Last(), points.First())
            };
            Edges = new ReadOnlyCollection<ILineSegment>(edges);
        }

        public abstract IPolygon Flipped();
    }
    public class Polygon : BasePolygon
    {
        public Polygon(IEnumerable<Vector3D> points) : base(points.ToList())
        {

        }
        public override IPolygon Flipped()
        {
            return new Polygon(Points.Reverse());
        }
    }
}
