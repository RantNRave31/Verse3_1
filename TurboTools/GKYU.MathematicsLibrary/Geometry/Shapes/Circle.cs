using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GKYU.MathLibrary.Tensors.Vectors;

namespace GKYU.MathLibrary.Geometry.Shapes
{
    public interface ICircle
    {
        Vector3D Center { get; }
        Vector3D Axis { get; }
        double Radius { get; }
    }
    public class Circle : 
        ICircle
    {
        public Vector3D Axis { get; private set; }

        public Vector3D Center { get; private set; }

        public double Radius { get; private set; }

        public Circle(Vector3D center, double radius, Vector3D axis)
        {
            Center = center;
            Radius = radius;
            Axis = axis;
        }

        public static ICircle FromPoints(Vector3D a, Vector3D b, Vector3D c)
        {
            var t = b - a;
            var u = c - a;
            var v = c - b;

            var w = t.CrossProduct(u);
            var wsl = w.LengthSquared;

            double iwsl2 = 1.0 / (2.0 * wsl);

            var center = a + (u * t.DotProduct(t) * (u.DotProduct(v)) - t * u.DotProduct(u) * (t.DotProduct(v))) * iwsl2;
            var radius = Math.Sqrt(t.DotProduct(t) * u.DotProduct(u) * (v.DotProduct(v)) * iwsl2 * 0.5);
            var axis = w / Math.Sqrt(wsl);

            return new Circle(center, radius, axis);
        }
    }
}
