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
    public static class VectorIntersections
    {


        public static bool On(this Vector3D p, IPlane plane, double epslion = 1E-07)
        {

            return true;
        }

        public static bool On(this Vector3D p, ITriangle triangle, double epslion = 1E-07)
        {
            double u, v, w;
            triangle.Barycentric(p, out u, out v, out w);
            return v >= 0f && w >= 0f && (v + w) <= 1f;
        }

        public static bool On(this Vector3D p, IAABB aabb, double epslion = 1E-07)
        {
            var distance = aabb.Center - p;
            return Math.Abs(distance.X).NearlyEquals(aabb.Extent.X, epslion)
                && Math.Abs(distance.Y).NearlyEquals(aabb.Extent.Y, epslion)
                && Math.Abs(distance.Z).NearlyEquals(aabb.Extent.Z, epslion);
        }

        public static bool In(this Vector3D p, IAABB aabb, double epslion = 1E-07)
        {
            var distance = aabb.Center - p;
            return Math.Abs(distance.X).NearlyLessThanOrEquals(aabb.Extent.X, epslion)
                && Math.Abs(distance.Y).NearlyLessThanOrEquals(aabb.Extent.Y, epslion)
                && Math.Abs(distance.Z).NearlyLessThanOrEquals(aabb.Extent.Z, epslion);
        }


        //public static bool In(this Vect3 p, IAABB aabb, double epslion = 0.0001)
        //{
        //    var distance = aabb.Center - p;

        //    MathsExtensions.NearlyEquals()

        //    return Math.Abs(distance.X).NearlyLessThanOrEquals(box.HalfSize.X) && Math.Abs(distance.Y).NearlyLessThanOrEquals(box.HalfSize.Y) && Math.Abs(distance.Z).NearlyLessThanOrEquals(box.HalfSize.Z);

        //}

    }
}
