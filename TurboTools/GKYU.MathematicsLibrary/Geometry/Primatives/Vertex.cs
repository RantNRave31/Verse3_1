using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GKYU.MathLibrary.Tensors.Vectors;

namespace GKYU.MathLibrary.Geometry.Primatives
{
    public interface IVertex
    {
        Vector3D Position { get; }
        Vector3D Normal { get; }
    }
    public class Vertex
        : IVertex
    {
        public Vector3D Position { get; private set; }
        public Vector3D Normal { get; private set; }

        public Vertex(Vector3D position, Vector3D normal)
        {
            Position = position;
            Normal = normal;
        }

        public Vertex Invert()
        {
            return new Vertex(Position, -Normal);
        }
    }
}
