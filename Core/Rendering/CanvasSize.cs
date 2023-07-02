using System;
using System.Numerics;
using System.Runtime.Serialization;

namespace Core
{
    public partial class Geometry2D
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Canvas Size
        /// </summary>
        [Serializable]
        public class CanvasSize : Observable, IEquatable<CanvasSize>, IEquatable<CanvasPoint>, ISerializable
        {
            #region Constructors

            public static readonly CanvasSize Unset = new CanvasSize(double.NaN, double.NaN);

            public CanvasSize()
            {
                this.Width = double.NaN;
                this.Height = double.NaN;
            }

            public CanvasSize(double w, double h)
            {
                this.Width = w;
                this.Height = h;
            }
            public CanvasSize(SerializationInfo info, StreamingContext context)
            {
                this.Width = info.GetDouble("Width");
                this.Height = info.GetDouble("Height");
            }

            #endregion

            #region Properties
            private double _width;
            private double _height;
            public double Width
            {
                get { return _width; }
                set
                {
                    if (value != _width)
                    {
                        _width = value;
                        OnPropertyChanged("Width");
                    }
                }
            }
            public double Height
            {
                get { return _height; }
                set
                {
                    if (value != _height)
                    {
                        _height = value;
                        OnPropertyChanged("Height");
                    }
                }
            }

            #endregion

            #region Methods

            public Vector2 ToVector()
            {
                return new Vector2((float)Width, (float)Height);
            }
            public override string ToString()
            {
                return ($"CanvasSize({this.Width.ToString()}, {this.Height.ToString()})");
            }

            public static CanvasSize operator +(CanvasSize A, CanvasSize B)
            {
                return new CanvasSize((A.Width + B.Width), (A.Height + B.Height));
            }
            public static CanvasSize operator -(CanvasSize A, CanvasSize B)
            {
                return new CanvasSize((A.Width - B.Width), (A.Height - B.Height));
            }
            public static bool operator ==(CanvasSize A, CanvasSize B)
            {
                return ((A.Width == B.Width) && (A.Height == B.Height));
            }
            public static bool operator !=(CanvasSize A, CanvasSize B)
            {
                return ((A.Width != B.Width) || (A.Height != B.Height));
            }
            public override bool Equals(object obj)
            {
                if (!(obj is CanvasSize)) return false;
                CanvasSize c = obj as CanvasSize;
                return ((this.Width == c.Width) && (this.Height == c.Height));
            }
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
            public bool Equals(CanvasPoint other)
            {
                CanvasPoint s = this;
                return ((this.Width == other.X) && (this.Height == other.Y));
            }
            public bool Equals(CanvasSize other)
            {
                return ((this.Width == other.Width) && (this.Height == other.Height));
            }
            public double Area()
            {
                return (this.Width * this.Height);
            }

            public static implicit operator CanvasSize(CanvasPoint v)
            {
                CanvasSize s = new CanvasSize(v.X, v.Y);
                return s;
            }
            public bool IsValid()
            {
                if (this.Width != double.NaN && this.Height != double.NaN) return true;
                else return false;
            }

            #endregion

            public void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                info.AddValue("Width", this.Width);
                info.AddValue("Height", this.Height);
            }
        }
    }
}
