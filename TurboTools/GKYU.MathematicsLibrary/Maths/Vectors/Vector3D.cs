using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GKYU.MathLibrary.Tensors.Vectors
{
    public class Vector3D
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }

        public Vector3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3D()
            : this(0,0,0)
        {
        }

        public Vector3D(IList<double> a)
        {
            if (a.Count != 3) throw new ArgumentException("Array should be double[3]");
            X = a[0];
            Y = a[1];
            Z = a[2];
        }

        public double Length
        {
            get { return Math.Sqrt(LengthSquared); }
        }

        public double LengthSquared
        {
            get { return X * X + Y * Y + Z * Z; }
        }

        public static Vector3D Zero
        {
            get { return new Vector3D(0, 0, 0); }
        }

        public static Vector3D UnitX
        {
            get { return new Vector3D(0 + 1, 0, 0); }
        }

        public static Vector3D UnitY
        {
            get { return new Vector3D(0, 0 + 1, 0); }
        }

        public static Vector3D UnitZ
        {
            get { return new Vector3D(0, 0, 0 + 1); }
        }

        public static Vector3D Min(Vector3D a, Vector3D b)
        {
            return new Vector3D((a.X <= b.X ? a.X : b.X), (a.Y <= b.Y ? a.Y : b.Y), (a.Z <= b.Z ? a.Z : b.Z));
        }
        public static Vector3D Max(Vector3D a, Vector3D b)
        {
            return new Vector3D((a.X > b.X ? a.X : b.X), (a.Y > b.Y ? a.Y : b.Y), (a.Z > b.Z ? a.Z : b.Z));
        }
        public Vector3D Normalize()
        {
            var num = 1f / Length;
            return new Vector3D(X * num, Y * num, Z * num);
        }

        public Vector3D CrossProduct(Vector3D b)
        {
            return new Vector3D(Y * b.Z - Z * b.Y, Z * b.X - X * b.Z, X * b.Y - Y * b.X);
        }

        public double DotProduct(Vector3D v)
        {
            return X * v.X + Y * v.Y + Z * v.Z;
        }

        public static Vector3D operator +(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector3D operator -(Vector3D v1, Vector3D v2)
        {
            return new Vector3D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector3D operator -(Vector3D v)
        {
            return new Vector3D(-v.X, -v.Y, -v.Z);
        }

        public static Vector3D operator *(Vector3D v, double d)
        {
            return new Vector3D(v.X * d, v.Y * d, v.Z * d);
        }

        public static Vector3D operator *(double d, Vector3D v)
        {
            return v * d;
        }

        public static Vector3D operator /(Vector3D v, double d)
        {
            return new Vector3D(v.X / d, v.Y / d, v.Z / d);
        }

        public static bool operator ==(Vector3D a, Vector3D b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }
            return a.Equals(b);
        }

        public static bool operator !=(Vector3D a, Vector3D b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Vector3D)obj);
        }

        protected bool Equals(Vector3D other)
        {
            return (X).NearlyEquals(other.X) && (Y).NearlyEquals(other.Y) && (Z).NearlyEquals(other.Z);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        public double[] ToArray()
        {
            return new[] { X, Y, Z };
        }

        public Vector3D Lerp(Vector3D end, double t)
        {
            return (1 - t) * this + t * end;
        }

        public override string ToString()
        {
            return String.Format("Vector3D<{0},{1},{2}>", X, Y, Z);
        }
    }
}
