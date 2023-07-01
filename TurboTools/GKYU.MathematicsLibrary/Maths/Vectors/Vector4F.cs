using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GKYU.MathLibrary.Tensors.Vectors
{
    public class Vector4F
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public float Z { get; private set; }
        public float W { get; private set; }

        public Vector4F(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
        public Vector4F(IList<float> a)
        {
            if (a.Count != 4) throw new ArgumentException("Array should be T[4]");
            X = a[0];
            Y = a[1];
            Z = a[2];
            W = a[3];
        }

        public static bool operator ==(Vector4F a, Vector4F b)
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

        public static bool operator !=(Vector4F a, Vector4F b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Vector4F)obj);
        }

        protected bool Equals(Vector4F other)
        {
            return (X).NearlyEquals(other.X) && (Y).NearlyEquals(other.Y) && (Z).NearlyEquals(other.Z) && (W).NearlyEquals(other.W);
        }

        public float Length
        {
            get { return (float)Math.Sqrt(LengthSquared); }
        }

        public float LengthSquared
        {
            get { return X * X + Y * Y + Z * Z + W * W; }
        }

        public Vector4F Normalize()
        {
            var num = 1f / Length;
            return new Vector4F(X * num, Y * num, Z * num, W * num);
        }

        public static Vector4F Zero
        {
            get { return new Vector4F(0, 0, 0, 0); }
        }
        public static Vector4F UnitX
        {
            get { return new Vector4F(0 + 1, 0, 0, 0); }
        }

        public static Vector4F UnitY
        {
            get { return new Vector4F(0, 0 + 1, 0, 0); }
        }

        public static Vector4F UnitZ
        {
            get { return new Vector4F(0, 0, 0 + 1, 0); }
        }

        public static Vector4F UnitW
        {
            get { return new Vector4F(0, 0, 0, 0 + 1); }
        }

        public float DotProduct(Vector4F v)
        {
            return X * v.X + Y * v.Y + Z * v.Z + W * v.W;
        }

        public Vector4F ExteriorProduct(Vector4F v)
        {
            throw new NotImplementedException();
        }

        public static Vector4F operator +(Vector4F v1, Vector4F v2)
        {
            return new Vector4F(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z, v1.W + v2.W);
        }

        public static Vector4F operator -(Vector4F v1, Vector4F v2)
        {
            return new Vector4F(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z, v1.W - v2.W);
        }
        public static Vector4F operator -(Vector4F v)
        {
            return new Vector4F(-v.X, -v.Y, -v.Z, -v.W);
        }

        public static Vector4F operator *(Vector4F v, float d)
        {
            return new Vector4F(v.X * d, v.Y * d, v.Z * d, v.W * d);
        }

        public static Vector4F operator *(float d, Vector4F v)
        {
            return v * d;
        }

        public static Vector4F operator /(Vector4F v, float d)
        {
            return new Vector4F(v.X / d, v.Y / d, v.Z / d, v.W / d);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode() ^ Z.GetHashCode() ^ W.GetHashCode();
        }


        public Vector4F Lerp(Vector4F end, float t)
        {
            return (1 - t) * this + t * end;
        }

        public override string ToString()
        {
            return String.Format("Vector4({0},{1},{2},{3})", X, Y, Z, W);
        }

        public float[] ToArray()
        {
            return new[] { X, Y, Z, W };
        }

    }
}
