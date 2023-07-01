using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GKYU.MathLibrary.Tensors.Vectors
{
    public class Vector2F
    {
        public float X { get; private set; }
        public float Y { get; private set; }

        public Vector2F(float x, float y)
        {
            X = x;
            Y = y;
        }
        public Vector2F()
            : this(0,0)
        {
        }

        public Vector2F(IList<float> a)
        {
            if (a.Count != 2) throw new ArgumentException("Array should be T[2]");
            X = a[0];
            Y = a[1];
        }
        public float Length
        {
            get { return (float)Math.Sqrt(LengthSquared); }
        }
        public float LengthSquared
        {
            get { return X * X + Y * Y; }
        }
        public static Vector2F Zero
        {
            get { return new Vector2F(0, 0); }
        }

        public static Vector2F UnitX
        {
            get { return new Vector2F(0 + 1, 0); }
        }

        public static Vector2F UnitY
        {
            get { return new Vector2F(0, 0 + 1); }
        }

        public static Vector2F Min(Vector2F a, Vector2F b)
        {
            return new Vector2F((a.X <= b.X ? a.X : b.X), (a.Y <= b.Y ? a.Y : b.Y));
        }
        public static Vector2F Max(Vector2F a, Vector2F b)
        {
            return new Vector2F((a.X > b.X ? a.X : b.X), (a.Y > b.Y ? a.Y : b.Y));
        }

        public Vector2F Normalize()
        {
            var num = 1f / Length;
            return new Vector2F(X * num, Y * num);
        }

        public float DotProduct(Vector2F v)
        {
            return X * v.X + Y * v.Y;
        }

        public static Vector2F operator +(Vector2F v1, Vector2F v2)
        {
            return new Vector2F(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2F operator -(Vector2F v1, Vector2F v2)
        {
            return new Vector2F(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2F operator -(Vector2F v)
        {
            return new Vector2F(-v.X, -v.Y);
        }

        public static Vector2F operator *(Vector2F v, float d)
        {
            return new Vector2F(v.X * d, v.Y * d);
        }

        public static Vector2F operator *(float d, Vector2F v)
        {
            return v * d;
        }

        public static Vector2F operator /(Vector2F v, float d)
        {
            return new Vector2F(v.X / d, v.Y / d);
        }

        public static bool operator ==(Vector2F a, Vector2F b)
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

        public static bool operator !=(Vector2F a, Vector2F b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Vector2F)obj);
        }

        protected bool Equals(Vector2F other)
        {
            return X.NearlyEquals(other.X) && Y.NearlyEquals(other.Y);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public float[] ToArray()
        {
            return new[] { X, Y };
        }

        public Vector2F Lerp(Vector2F end, float t)
        {
            return (1 - t) * this + t * end;
        }

        public override string ToString()
        {
            return String.Format("Vector2F[{0},{1}]", X, Y);
        }
    }
}
