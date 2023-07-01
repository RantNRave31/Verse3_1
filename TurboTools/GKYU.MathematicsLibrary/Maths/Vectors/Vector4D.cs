using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GKYU.MathLibrary.Tensors.Vectors
{
    public class Vector4D
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Z { get; private set; }
        public double W { get; private set; }

        public Vector4D(double x, double y, double z, double w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
        public Vector4D(IList<double> a)
        {
            if (a.Count != 4) throw new ArgumentException("Array should be T[4]");
            X = a[0];
            Y = a[1];
            Z = a[2];
            W = a[3];
        }

        public static bool operator ==(Vector4D a, Vector4D b)
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

        public static bool operator !=(Vector4D a, Vector4D b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Vector4D)obj);
        }

        protected bool Equals(Vector4D other)
        {
            return (X).NearlyEquals(other.X) && (Y).NearlyEquals(other.Y) && (Z).NearlyEquals(other.Z) && (W).NearlyEquals(other.W);
        }

        public double Length
        {
            get { return Math.Sqrt(LengthSquared); }
        }

        public double LengthSquared
        {
            get { return X * X + Y * Y + Z * Z + W * W; }
        }

        public Vector4D Normalize()
        {
            var num = 1f / Length;
            return new Vector4D(X * num, Y * num, Z * num, W * num);
        }

        public static Vector4D Zero
        {
            get { return new Vector4D(0, 0, 0, 0); }
        }
        public static Vector4D UnitX
        {
            get { return new Vector4D(0 + 1, 0, 0, 0); }
        }

        public static Vector4D UnitY
        {
            get { return new Vector4D(0, 0 + 1, 0, 0); }
        }

        public static Vector4D UnitZ
        {
            get { return new Vector4D(0, 0, 0 + 1, 0); }
        }

        public static Vector4D UnitW
        {
            get { return new Vector4D(0, 0, 0, 0 + 1); }
        }

        public double DotProduct(Vector4D v)
        {
            return X * v.X + Y * v.Y + Z * v.Z + W * v.W;
        }

        public Vector4D ExteriorProduct(Vector4D v)
        {
            throw new NotImplementedException();
        }

        public static Vector4D operator +(Vector4D v1, Vector4D v2)
        {
            return new Vector4D(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z, v1.W + v2.W);
        }

        public static Vector4D operator -(Vector4D v1, Vector4D v2)
        {
            return new Vector4D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z, v1.W - v2.W);
        }
        public static Vector4D operator -(Vector4D v)
        {
            return new Vector4D(-v.X, -v.Y, -v.Z, -v.W);
        }

        public static Vector4D operator *(Vector4D v, double d)
        {
            return new Vector4D(v.X * d, v.Y * d, v.Z * d, v.W * d);
        }

        public static Vector4D operator *(double d, Vector4D v)
        {
            return v * d;
        }

        public static Vector4D operator /(Vector4D v, double d)
        {
            return new Vector4D(v.X / d, v.Y / d, v.Z / d, v.W / d);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode() ^ W.GetHashCode();
        }


        public Vector4D Lerp(Vector4D end, double t)
        {
            return (1 - t) * this + t * end;
        }

        public override string ToString()
        {
            return String.Format("Vector4({0},{1},{2},{3})", X, Y, Z, W);
        }

        public double[] ToArray()
        {
            return new[] { X, Y, Z, W };
        }

    }
}
