using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GKYU.MathLibrary.Geometry.Primatives;
using GKYU.MathLibrary.Tensors.Vectors;

namespace GKYU.MathLibrary.Geometry.Operations
{
    public static class PlaneInteresctions
    {
        public static bool Intersects(this IPlane p, IAABB aabb)
        {
            var r = aabb.Extent.X * Math.Abs(p.Normal.X) + aabb.Extent.Y * Math.Abs(p.Normal.Y) + aabb.Extent.Z * Math.Abs(p.Normal.Z);
            var s = p.Normal.DotProduct(aabb.Center) - p.Constant;
            return Math.Abs(s) <= r;
        }
    }
}
