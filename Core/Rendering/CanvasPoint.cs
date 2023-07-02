using System;
using System.Numerics;
using System.Runtime.Serialization;

namespace Core
{
    public partial class Geometry2D
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Canvas Point
        /// </summary>
        [Serializable]
        public class CanvasPoint : Observable, IEquatable<CanvasSize>, IEquatable<CanvasPoint>, ISerializable
        {
            //TODO: MAKE IMPLICIT CASTS TO OTHER POINT TYPES
            #region Constructors

            public static readonly CanvasPoint Unset = new CanvasPoint(double.NaN, double.NaN);
            public CanvasPoint()
            {
                this.X = double.NaN;
                this.Y = double.NaN;
            }

            public CanvasPoint(double x, double y)
            {
                this.X = x;
                this.Y = y;
            }
            public CanvasPoint(SerializationInfo info, StreamingContext context)
            {
                this.X = info.GetDouble("X");
                this.Y = info.GetDouble("Y");
            }

            #endregion

            #region Properties
            private double _x;
            private double _y;
            public double X
            {
                get { return _x; }
                set
                {
                    if (value != _x)
                    {
                        _x = value;
                        OnPropertyChanged("X");
                    }
                }
            }
            public double Y
            {
                get { return _y; }
                set
                {
                    if (value != _y)
                    {
                        _y = value;
                        OnPropertyChanged("Y");
                    }
                }
            }

            #endregion

            #region Methods

            public Vector2 ToVector()
            {
                return new Vector2((float)X, (float)Y);
            }

            public override string ToString()
            {
                return ($"CanvasPoint({this.X.ToString()}, {this.Y.ToString()})");
            }

            public static CanvasPoint operator +(CanvasPoint A, CanvasPoint B)
            {
                return new CanvasPoint((A.X + B.X), (A.Y + B.Y));
            }
            public static CanvasPoint operator -(CanvasPoint A, CanvasPoint B)
            {
                return new CanvasSize((A.X - B.X), (A.Y - B.Y));
            }
            public static CanvasPoint operator /(CanvasPoint A, CanvasPoint B)
            {
                return new CanvasPoint((A.X / B.X), (A.Y / B.Y));
            }
            public static CanvasPoint operator /(CanvasPoint A, int B)
            {
                return new CanvasPoint((A.X / B), (A.Y / B));
            }
            public static CanvasPoint operator /(CanvasPoint A, double B)
            {
                return new CanvasPoint((A.X / B), (A.Y / B));
            }
            public static bool operator ==(CanvasPoint A, CanvasPoint B)
            {
                return ((A.X == B.X) && (A.Y == B.Y));
            }
            public static bool operator !=(CanvasPoint A, CanvasPoint B)
            {
                return ((A.X != B.X) || (A.Y != B.Y));
            }
            public override bool Equals(object obj)
            {
                if (!(obj is CanvasPoint)) return false;
                CanvasPoint c = obj as CanvasPoint;
                return ((this.X == c.X) && (this.Y == c.Y));
            }
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
            public bool Equals(CanvasPoint other)
            {
                CanvasPoint s = this;
                return ((this.X == other.X) && (this.Y == other.Y));
            }
            public bool Equals(CanvasSize other)
            {
                return ((this.X == other.Width) && (this.Y == other.Height));
            }
            public static implicit operator CanvasPoint(CanvasSize v)
            {
                CanvasPoint s = new CanvasPoint(v.Width, v.Height);
                return s;
            }

            public bool IsValid()
            {
                if (this.X != double.NaN && this.Y != double.NaN) return true;
                else return false;
            }

            #endregion

            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("X", this.X);
                info.AddValue("Y", this.Y);
            }
        }
    }
}
