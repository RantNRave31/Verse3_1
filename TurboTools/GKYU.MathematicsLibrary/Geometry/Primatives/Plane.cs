using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GKYU.MathLibrary.Tensors.Vectors;
using GKYU.MathLibrary.Geometry.Shapes;

namespace GKYU.MathLibrary.Geometry.Primatives
{
    public interface IPlane
    {
        Vector3D Normal { get; }
        double Constant { get; }
        IPlane Flipped();
    }
    public class Plane 
        : IPlane
    {
        public Vector3D Normal { get; private set; }
        public double Constant { get; private set; }

        public Plane(Vector3D normal, double constant)
        {
            Normal = normal;
            Constant = constant;
        }
        public override string ToString()
        {
            return string.Format("Plane");
        }
        public static Plane FromPoints(Vector3D a, Vector3D b, Vector3D c)
        {
            //Vector3D n = (b - a).CrossProduct(c - a).Normalize();
            Vector3D n = (b - a).CrossProduct(c - a).Normalize();
            return new Plane(n, n.DotProduct(a));
        }

        public static Plane FromPoints(params Vector3D[] points)
        {
            if (points.Length < 3) throw new ArgumentException();
            return FromPoints(points[0], points[1], points[2]);
        }

        public static Plane FromEquation(double a, double b, double c, double d)
        {
            var l = new Vector3D(a, b, c).Length;
            Vector3D normal = new Vector3D(a / l, b / l, c / l).Normalize();
            return new Plane(normal, d / l);
        }

        public double DistanceTo(Vector3D p)
        {
            return Normal.DotProduct(p) + Constant;
        }

        public double DistanceTo(Sphere s)
        {
            return DistanceTo(s.Center) - s.Radius;
        }

        public Plane Invert()
        {
            return new Plane(-Normal, -Constant);
        }
        public Plane Normalize()
        {
            Vector3D newNormal = Normal.Normalize();
            return new Plane(newNormal, Constant / Normal.Length);
        }

        public IPlane Flipped()
        {
            return new Plane(Normal * -1.0, Constant * -1.0);
        }
    }
}
