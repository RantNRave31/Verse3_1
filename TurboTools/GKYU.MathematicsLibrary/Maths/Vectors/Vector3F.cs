using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GKYU.MathLibrary.Tensors.Vectors
{
    public class Vector3DF
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }

        public Vector3DF(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3DF()
            : this(0,0,0)
        {
        }

        public Vector3DF(IList<float> a)
        {
            if (a.Count != 3) throw new ArgumentException("Array should be float[3]");
            X = a[0];
            Y = a[1];
            Z = a[2];
        }

        public float Length
        {
            get { return (float)Math.Sqrt(LengthSquared); }
        }

        public float LengthSquared
        {
            get { return X * X + Y * Y + Z * Z; }
        }

        public static Vector3DF Zero
        {
            get { return new Vector3DF(0, 0, 0); }
        }

        public static Vector3DF UnitX
        {
            get { return new Vector3DF(0 + 1, 0, 0); }
        }

        public static Vector3DF UnitY
        {
            get { return new Vector3DF(0, 0 + 1, 0); }
        }

        public static Vector3DF UnitZ
        {
            get { return new Vector3DF(0, 0, 0 + 1); }
        }

        public static Vector3DF Min(Vector3DF a, Vector3DF b)
        {
            return new Vector3DF((a.X <= b.X ? a.X : b.X), (a.Y <= b.Y ? a.Y : b.Y), (a.Z <= b.Z ? a.Z : b.Z));
        }
        public static Vector3DF Max(Vector3DF a, Vector3DF b)
        {
            return new Vector3DF((a.X > b.X ? a.X : b.X), (a.Y > b.Y ? a.Y : b.Y), (a.Z > b.Z ? a.Z : b.Z));
        }
        public Vector3DF Normalize()
        {
            var num = 1f / Length;
            return new Vector3DF(X * num, Y * num, Z * num);
        }

        public Vector3DF CrossProduct(Vector3DF b)
        {
            return new Vector3DF(Y * b.Z - Z * b.Y, Z * b.X - X * b.Z, X * b.Y - Y * b.X);
        }

        public float DotProduct(Vector3DF v)
        {
            return X * v.X + Y * v.Y + Z * v.Z;
        }

        public static Vector3DF operator +(Vector3DF v1, Vector3DF v2)
        {
            return new Vector3DF(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static Vector3DF operator -(Vector3DF v1, Vector3DF v2)
        {
            return new Vector3DF(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
        }

        public static Vector3DF operator -(Vector3DF v)
        {
            return new Vector3DF(-v.X, -v.Y, -v.Z);
        }

        public static Vector3DF operator *(Vector3DF v, float d)
        {
            return new Vector3DF(v.X * d, v.Y * d, v.Z * d);
        }

        public static Vector3DF operator *(float d, Vector3DF v)
        {
            return v * d;
        }

        public static Vector3DF operator /(Vector3DF v, float d)
        {
            return new Vector3DF(v.X / d, v.Y / d, v.Z / d);
        }

        public static bool operator ==(Vector3DF a, Vector3DF b)
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

        public static bool operator !=(Vector3DF a, Vector3DF b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Vector3DF)obj);
        }

        protected bool Equals(Vector3DF other)
        {
            return (X).NearlyEquals(other.X) && (Y).NearlyEquals(other.Y) && (Z).NearlyEquals(other.Z);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode();
        }

        public float[] ToArray()
        {
            return new[] { X, Y, Z };
        }

        public Vector3DF Lerp(Vector3DF end, float t)
        {
            return (1 - t) * this + t * end;
        }

        public override string ToString()
        {
            return String.Format("Vector3DF<{0},{1},{2}>", X, Y, Z);
        }
    }
}
