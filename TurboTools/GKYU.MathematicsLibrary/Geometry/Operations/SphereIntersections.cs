using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GKYU.MathLibrary.Geometry.Primatives;
using GKYU.MathLibrary.Geometry.Shapes;
using GKYU.MathLibrary.Tensors.Vectors;

namespace GKYU.MathLibrary.Geometry.Operations
{
    public static class SphereIntersections
    {
        public static bool Intersects(this Sphere s, IAABB b)
        {
            return b.SqDistPointAABB(s.Center) <= Math.Pow(s.Radius, 2);
        }
        private static double SqDistPointAABB(this IAABB b, Vector3D p)
        {
            double sqDist = 0;
            if (p.X < b.Min.X) sqDist += (b.Min.X - p.X) * (b.Min.X - p.X);
            if (p.X > b.Max.X) sqDist += (p.X - b.Max.X) * (p.X - b.Max.X);

            if (p.Y < b.Min.Y) sqDist += (b.Min.Y - p.Y) * (b.Min.Y - p.Y);
            if (p.Y > b.Max.Y) sqDist += (p.Y - b.Max.Y) * (p.Y - b.Max.Y);

            if (p.Z < b.Min.Z) sqDist += (b.Min.Z - p.Z) * (b.Min.Z - p.Z);
            if (p.Z > b.Max.Z) sqDist += (p.Z - b.Max.Z) * (p.Z - b.Max.Z);

            return sqDist;
        }
    }
}
