using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GKYU.MathLibrary.Geometry.Primatives;
using GKYU.MathLibrary.Tensors.Vectors;

namespace GKYU.MathLibrary.Geometry.Shapes
{
    public interface ICube
    {
        Vector3D Center { get; }
        Vector3D Size { get; }

        IEnumerable<IPolygon> ToPolygons();
    }
    public class Cube : ICube
    {
        public Vector3D Center { get; private set; }
        public Vector3D Size { get; private set; }

        public Cube(Vector3D center, double size)
        {
            Center = center;
            Size = new Vector3D(size, size, size);
        }
        public IEnumerable<IPolygon> ToPolygons()
        {
            throw new NotImplementedException();
        }
        //public IEnumerable<IPolygon> ToPolygons()
        //{
        //    var r = Size / 2.0;
        //    var a = new int[][]
        //    {
        //        new [] {0, 4, 6, 2},
        //        new [] {1, 3, 7, 5},
        //        new [] {0, 1, 5, 4},
        //        new [] {2, 6, 7, 3},
        //        new [] {0, 2, 3, 1},
        //        new [] {4, 5, 7, 6}

        //    };
        //    return a.Select(a =>
        //    {

        //    return new Vector3D(Center.X + r.X * (2 * !!(a & 1) - 1),Center.Y + r[1] * (2 * !!(a & 2) - 1), Center.Z + r[2] * (2 * !!(a & 4) - 1));
        //    })
        //}
    }
}
