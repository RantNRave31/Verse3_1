using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GKYU.MathLibrary.Geometry.Primatives;
using GKYU.MathLibrary.Geometry.Shapes;
using GKYU.MathLibrary.Tensors;
using GKYU.MathLibrary.Tensors.Vectors;

namespace GKYU.MathLibrary.Geometry.Operations
{
    public enum IntersectResult { }
    public static class TriangleInteresctions
    {
        public static bool Intersects(this ITriangle tri, IAABB aabb)
        {
            var v0 = tri.Point1 - aabb.Center;
            var v1 = tri.Point2 - aabb.Center;
            var v2 = tri.Point3 - aabb.Center;

            var f0 = v1 - v0;
            var f1 = v2 - v1;
            var f2 = v0 - v2;

            var axes = new[]
            {
                new Vector3D (0, -f0.Z, f0.Y),
                new Vector3D(0, -f1.Z, f1.Y),
                new Vector3D(0, -f2.Z, f2.Y),
                new Vector3D(f0.Z, 0, -f0.X),
                new Vector3D(f1.Z, 0, -f1.X),
                new Vector3D(f2.Z, 0, -f2.X),
                new Vector3D(-f0.Y, f0.X, 0),
                new Vector3D(-f1.Y, f1.X, 0),
                new Vector3D(-f2.Y, f2.X, 0)
            };

            foreach (var axis in axes)
            {
                var p0 = v0.DotProduct(axis);
                var p1 = v1.DotProduct(axis);
                var p2 = v2.DotProduct(axis);
                var r = aabb.Extent.X * Math.Abs(axis.X) + aabb.Extent.Y * Math.Abs(axis.Y) + aabb.Extent.Z * Math.Abs(axis.Z);
                if (Math.Max(-MathsHelper.Max(p0, p1, p2), MathsHelper.Min(p0, p1, p2)) > r) return false;
            }

            if (MathsHelper.Max(v0.X, v1.X, v2.X) < -aabb.Extent.X || MathsHelper.Min(v0.X, v1.X, v2.X) > aabb.Extent.X) return false;
            if (MathsHelper.Max(v0.Y, v1.Y, v2.Y) < -aabb.Extent.Y || MathsHelper.Min(v0.Y, v1.Y, v2.Y) > aabb.Extent.Y) return false;
            if (MathsHelper.Max(v0.Z, v1.Z, v2.Z) < -aabb.Extent.Z || MathsHelper.Min(v0.Z, v1.Z, v2.Z) > aabb.Extent.Z) return false;
            var normal = f0.CrossProduct(f1);
            var p = new Plane(normal, normal.DotProduct(v0));
            return p.Intersects(aabb);
        }
        public static void Barycentric(this ITriangle tri, Vector3D p, out double u, out double v, out double w)
        {
            var v0 = tri.Point2 - tri.Point1;
            var v1 = tri.Point3 - tri.Point1;
            var v2 = p - tri.Point1;
            var d00 = v0.DotProduct(v0);
            var d01 = v0.DotProduct(v1);
            var d11 = v1.DotProduct(v1);
            var d20 = v2.DotProduct(v0);
            var d21 = v2.DotProduct(v1);
            var denom = d00 * d11 - d01 * d01;
            v = (d11 * d20 - d01 * d21) / denom;
            w = (d00 * d21 - d01 * d20) / denom;
            u = 1.0f - v - w;
        }
    }
}
