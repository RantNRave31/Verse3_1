using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GKYU.MathLibrary.Geometry.Primatives;
using GKYU.MathLibrary.Tensors.Vectors;

namespace GKYU.MathLibrary.Geometry.Shapes
{
    public interface ITriangle
        : IPolygon
    {
        Vector3D Point1 { get; }
        Vector3D Point2 { get; }
        Vector3D Point3 { get; }
        Vector3D Normal { get; }
    }
    public class Triangle 
        : BasePolygon
        , ITriangle
    {
        public Vector3D Point1 { get; private set; }
        public Vector3D Point2 { get; private set; }
        public Vector3D Point3 { get; private set; }
        public Vector3D Normal { get; private set; }

        public Triangle(Vector3D point1, Vector3D point2, Vector3D point3, Vector3D normal)
            : base(new[] { point1, point2, point3 }, new[] { new LineSegment(point1, point2), new LineSegment(point2, point3), new LineSegment(point3, point1) })
        {
            Point1 = point1;
            Point2 = point2;
            Point3 = point3;
            Normal = normal;
        }

        public Triangle(Vector3D point1, Vector3D point2, Vector3D point3)
            : this(point1, point2, point3, CalculateNormal(point1, point2, point3))
        {

        }

        private static Vector3D CalculateNormal(Vector3D point1, Vector3D point2, Vector3D point3)
        {
            var u = point2 - point1;
            var v = point3 - point1;
            return u.CrossProduct(v);

        }

        public override IPolygon Flipped()
        {
            return new Triangle(Point3, Point2, Point1);
        }

        public Vector3D Centroid()
        {
            return (Point1 + Point2 + Point3) / 3.0;
        }
        public Vector3D Circumcenter()
        {
            return Circle.FromPoints(Point1, Point2, Point3).Center;
        }

    }
}
