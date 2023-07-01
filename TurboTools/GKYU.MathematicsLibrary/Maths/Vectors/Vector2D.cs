using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GKYU.MathLibrary.Tensors.Vectors
{
    public class Vector2D
    {
        public double X { get; private set; }
        public double Y { get; private set; }

        public Vector2D(double x, double y)
        {
            X = x;
            Y = y;
        }
        public Vector2D()
            : this(0,0)
        {
        }

        public Vector2D(IList<double> a)
        {
            if (a.Count != 2) throw new ArgumentException("Array should be T[2]");
            X = a[0];
            Y = a[1];
        }
        public double Length
        {
            get { return Math.Sqrt(LengthSquared); }
        }
        public double LengthSquared
        {
            get { return X * X + Y * Y; }
        }
        public static Vector2D Zero
        {
            get { return new Vector2D(0, 0); }
        }

        public static Vector2D UnitX
        {
            get { return new Vector2D(0 + 1, 0); }
        }

        public static Vector2D UnitY
        {
            get { return new Vector2D(0, 0 + 1); }
        }

        public static Vector2D Min(Vector2D a, Vector2D b)
        {
            return new Vector2D((a.X <= b.X ? a.X : b.X), (a.Y <= b.Y ? a.Y : b.Y));
        }
        public static Vector2D Max(Vector2D a, Vector2D b)
        {
            return new Vector2D((a.X > b.X ? a.X : b.X), (a.Y > b.Y ? a.Y : b.Y));
        }

        public Vector2D Normalize()
        {
            var num = 1f / Length;
            return new Vector2D(X * num, Y * num);
        }

        public double DotProduct(Vector2D v)
        {
            return X * v.X + Y * v.Y;
        }

        public static Vector2D operator +(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2D operator -(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2D operator -(Vector2D v)
        {
            return new Vector2D(-v.X, -v.Y);
        }

        public static Vector2D operator *(Vector2D v, double d)
        {
            return new Vector2D(v.X * d, v.Y * d);
        }

        public static Vector2D operator *(double d, Vector2D v)
        {
            return v * d;
        }

        public static Vector2D operator /(Vector2D v, double d)
        {
            return new Vector2D(v.X / d, v.Y / d);
        }

        public static bool operator ==(Vector2D a, Vector2D b)
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

        public static bool operator !=(Vector2D a, Vector2D b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Vector2D)obj);
        }

        protected bool Equals(Vector2D other)
        {
            return X.NearlyEquals(other.X) && Y.NearlyEquals(other.Y);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public double[] ToArray()
        {
            return new[] { X, Y };
        }

        public Vector2D Lerp(Vector2D end, double t)
        {
            return (1 - t) * this + t * end;
        }

        public override string ToString()
        {
            return String.Format("Vector2D[{0},{1}]", X, Y);
        }
    }
}
