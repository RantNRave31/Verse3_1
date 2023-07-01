using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GKYU.MathLibrary.Tensors.Vectors;

namespace GKYU.MathLibrary.Geometry.Shapes
{
    public interface ISphere
    {
        Vector3D Center { get; }
        double Radius { get; }
    }
    public class Sphere
        : ISphere
    {
        public Vector3D Center { get; private set; }
        public double Radius { get; private set; }

        public Sphere(Vector3D center, double radius)
        {
            Center = center;
            Radius = radius;
        }
        public override string ToString()
        {
            return string.Format("Sphere");
        }
    }
}
