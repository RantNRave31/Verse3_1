using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GKYU.MathLibrary.Tensors.Vectors;

namespace GKYU.MathLibrary.Geometry.Primatives
{
    public interface ILineSegment
    {
        Vector3D Start { get; }
        Vector3D End { get; }
    }
    public class LineSegment : ILineSegment
    {
        public Vector3D Start { get; private set; }
        public Vector3D End { get; private set; }

        public Func<double, Vector3D> Equation
        {
            get
            {
                return d => Start.Lerp(End, d);
            }
        }

        public LineSegment(Vector3D start, Vector3D end)
        {
            Start = start;
            End = end;
        }
    }
}
