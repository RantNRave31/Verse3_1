using GKYU.MathLibrary.Tensors.Vectors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.MathLibrary.Tensors.Matrices
{
    public class Matrix4D
    {
        private readonly double[,] _data;

        public Matrix4D(double[,] data)
        {
            _data = data;
        }

        public Matrix4D(Matrix4D other)
        {
            _data = other._data;
        }

        public Matrix4D()
            : this(Matrix4D.Zero)
        {

        }

        public double this[int i, int j]
        {
            get
            {
                return _data[i, j];
            }
            set
            {
                _data[i, j] = value;
            }
        }

        public double X
        {
            get { return this[0, 3]; }
            set { this[0, 3] = value; }
        }

        public double Y
        {
            get { return this[1, 3]; }
            set { this[1, 3] = value; }
        }

        public double Z
        {
            get { return this[2, 3]; }
            set { this[2, 3] = value; }
        }

        public static Matrix4D Zero
        {
            get
            {
                return new Matrix4D(new[,]
                    {
                        {0.0, 0.0, 0.0, 0.0},
                        {0.0, 0.0, 0.0, 0.0},
                        {0.0, 0.0, 0.0, 0.0},
                        {0.0, 0.0, 0.0, 0.0}
                    });
            }
        }
        public static Matrix4D Identity
        {
            get
            {
                return new Matrix4D(new[,]
                    {
                        {1.0, 0.0, 0.0, 0.0},
                        {0.0, 1.0, 0.0, 0.0},
                        {0.0, 0.0, 1.0, 0.0},
                        {0.0, 0.0, 0.0, 1.0}
                    });
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Matrix4D)obj);
        }
        public bool Equals(Matrix4D other)
        {
            if (other == null)
            {
                return false;
            }

            var m = other as Matrix4D;
            if (m == null)
            {
                return false;
            }
            return _data[0, 0].NearlyEquals(m._data[0, 0]) && _data[0, 1].NearlyEquals(m._data[0, 1]) && _data[0, 2].NearlyEquals(m._data[0, 2]) && _data[0, 3].NearlyEquals(m._data[0, 3]) &&
                   _data[1, 0].NearlyEquals(m._data[1, 0]) && _data[1, 1].NearlyEquals(m._data[1, 1]) && _data[1, 2].NearlyEquals(m._data[1, 2]) && _data[1, 3].NearlyEquals(m._data[1, 3]) &&
                   _data[2, 0].NearlyEquals(m._data[2, 0]) && _data[2, 1].NearlyEquals(m._data[2, 1]) && _data[2, 2].NearlyEquals(m._data[2, 2]) && _data[2, 3].NearlyEquals(m._data[2, 3]) &&
                   _data[3, 0].NearlyEquals(m._data[3, 0]) && _data[3, 1].NearlyEquals(m._data[3, 1]) && _data[3, 2].NearlyEquals(m._data[3, 2]) && _data[3, 3].NearlyEquals(m._data[3, 3]);
        }

        public Matrix4D Add(Matrix4D matrix)
        {
            return new Matrix4D(new[,]
                {
                    {
                        this[0, 0] + matrix[0, 0], this[0, 1] + matrix[0, 1], this[0, 2] + matrix[0, 2],
                        this[0, 2] + matrix[0, 2]
                    },
                    {
                        this[1, 0] + matrix[1, 0], this[1, 1] + matrix[1, 1], this[1, 2] + matrix[1, 2],
                        this[2, 3] + matrix[2, 3]
                    },
                    {
                        this[2, 0] + matrix[2, 0], this[2, 1] + matrix[2, 1], this[2, 2] + matrix[2, 2],
                        this[2, 3] + matrix[2, 3]
                    },
                    {
                        this[3, 0] + matrix[3, 0], this[3, 1] + matrix[3, 1], this[3, 2] + matrix[3, 2],
                        this[2, 2] + matrix[2, 2]
                    }
                });
        }

        public Matrix4D Add(double scalar)
        {
            return new Matrix4D(new[,]
                {
                    {this[0, 0] + scalar, this[0, 1] + scalar, this[0, 2] + scalar, this[0, 3] + scalar},
                    {this[1, 0] + scalar, this[1, 1] + scalar, this[1, 2] + scalar, this[1, 3] + scalar},
                    {this[2, 0] + scalar, this[2, 1] + scalar, this[2, 2] + scalar, this[2, 3] + scalar},
                    {this[3, 0] + scalar, this[3, 1] + scalar, this[3, 2] + scalar, this[3, 3] + scalar}
                });
        }

        public Matrix4D Subtract(Matrix4D matrix)
        {
            return new Matrix4D(new[,]
                {
                    {
                        this[0, 0] - matrix[0, 0], this[0, 1] - matrix[0, 1], this[0, 2] - matrix[0, 2],
                        this[0, 3] - matrix[0, 3]
                    },
                    {
                        this[1, 0] - matrix[1, 0], this[1, 1] - matrix[1, 1], this[1, 2] - matrix[1, 2],
                        this[1, 3] - matrix[1, 3]
                    },
                    {
                        this[2, 0] - matrix[2, 0], this[2, 1] - matrix[2, 1], this[2, 2] - matrix[2, 2],
                        this[2, 3] - matrix[2, 3]
                    },
                    {
                        this[3, 0] - matrix[3, 0], this[3, 1] - matrix[3, 1], this[3, 2] - matrix[3, 2],
                        this[3, 3] - matrix[3, 3]
                    }
                });
        }

        public Matrix4D Subtract(double scalar)
        {
            return new Matrix4D(new[,]
                {
                    {this[0, 0] - scalar, this[0, 1] - scalar, this[0, 2] - scalar, this[0, 3] - scalar},
                    {this[1, 0] - scalar, this[1, 1] - scalar, this[1, 2] - scalar, this[1, 3] - scalar},
                    {this[2, 0] - scalar, this[2, 1] - scalar, this[2, 2] - scalar, this[2, 3] - scalar},
                    {this[3, 0] - scalar, this[3, 1] - scalar, this[3, 2] - scalar, this[3, 3] - scalar}
                });
        }

        public Matrix4D Multiply(Matrix4D matrix)
        {
            return new Matrix4D(new[,]
                {
                    {
                        this[0, 0]*matrix[0, 0] + this[0, 1]*matrix[1, 0] + this[0, 2]*matrix[2, 0] +
                        this[0, 3]*matrix[3, 0],
                        this[0, 0]*matrix[0, 1] + this[0, 1]*matrix[1, 1] + this[0, 2]*matrix[2, 1] +
                        this[0, 3]*matrix[3, 1],
                        this[0, 0]*matrix[0, 2] + this[0, 1]*matrix[1, 2] + this[0, 2]*matrix[2, 2] +
                        this[0, 3]*matrix[3, 2],
                        this[0, 0]*matrix[0, 3] + this[0, 1]*matrix[1, 3] + this[0, 2]*matrix[2, 3] +
                        this[0, 3]*matrix[3, 3]
                    },
                    {
                        this[1, 0]*matrix[0, 0] + this[1, 1]*matrix[1, 0] + this[1, 2]*matrix[2, 0] +
                        this[1, 3]*matrix[3, 0],
                        this[1, 0]*matrix[0, 1] + this[1, 1]*matrix[1, 1] + this[1, 2]*matrix[2, 1] +
                        this[1, 3]*matrix[3, 1],
                        this[1, 0]*matrix[0, 2] + this[1, 1]*matrix[1, 2] + this[1, 2]*matrix[2, 2] +
                        this[1, 3]*matrix[3, 2],
                        this[1, 0]*matrix[0, 3] + this[1, 1]*matrix[1, 3] + this[1, 2]*matrix[2, 3] +
                        this[1, 3]*matrix[3, 3]
                    },
                    {
                        this[2, 0]*matrix[0, 0] + this[2, 1]*matrix[1, 0] + this[2, 2]*matrix[2, 0] +
                        this[2, 3]*matrix[3, 0],
                        this[2, 0]*matrix[0, 1] + this[2, 1]*matrix[1, 1] + this[2, 2]*matrix[2, 1] +
                        this[2, 3]*matrix[3, 1],
                        this[2, 0]*matrix[0, 2] + this[2, 1]*matrix[1, 2] + this[2, 2]*matrix[2, 2] +
                        this[2, 3]*matrix[3, 2],
                        this[2, 0]*matrix[0, 3] + this[2, 1]*matrix[1, 3] + this[2, 2]*matrix[2, 3] +
                        this[2, 3]*matrix[3, 3]
                    },
                    {
                        this[3, 0]*matrix[0, 0] + this[3, 1]*matrix[1, 0] + this[3, 2]*matrix[2, 0] +
                        this[3, 3]*matrix[3, 0],
                        this[3, 0]*matrix[0, 1] + this[3, 1]*matrix[1, 1] + this[3, 2]*matrix[2, 1] +
                        this[3, 3]*matrix[3, 1],
                        this[3, 0]*matrix[0, 2] + this[3, 1]*matrix[1, 2] + this[3, 2]*matrix[2, 2] +
                        this[3, 3]*matrix[3, 2],
                        this[3, 0]*matrix[0, 3] + this[3, 1]*matrix[1, 3] + this[3, 2]*matrix[2, 3] +
                        this[3, 3]*matrix[3, 3]
                    }
                });
        }

        public Matrix4D Multiply(double scalar)
        {
            return new Matrix4D(new[,]
                {
                    {this[0, 0]*scalar, this[0, 1]*scalar, this[0, 2]*scalar, this[0, 3]*scalar},
                    {this[1, 0]*scalar, this[1, 1]*scalar, this[1, 2]*scalar, this[1, 3]*scalar},
                    {this[2, 0]*scalar, this[2, 1]*scalar, this[2, 2]*scalar, this[2, 3]*scalar},
                    {this[3, 0]*scalar, this[3, 1]*scalar, this[3, 2]*scalar, this[3, 3]*scalar}
                });
        }

        public Matrix4D Divide(double scalar)
        {
            return Multiply(1.0 / scalar);
        }

        public static bool operator ==(Matrix4D a, Matrix4D b)
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

        public static bool operator !=(Matrix4D a, Matrix4D b)
        {
            return !(a == b);
        }

        public static Matrix4D operator +(Matrix4D m1, Matrix4D m2)
        {
            return m1.Add(m2);
        }

        public static Matrix4D operator +(Matrix4D m, double d)
        {
            return m.Add(d);
        }

        public static Matrix4D operator -(Matrix4D m1, Matrix4D m2)
        {
            return m1.Subtract(m2);
        }

        public static Matrix4D operator -(Matrix4D m, double d)
        {
            return m.Subtract(d);
        }

        public static Matrix4D operator *(Matrix4D m1, Matrix4D m2)
        {
            return m1.Multiply(m2);
        }

        public static Matrix4D operator *(Matrix4D m, double d)
        {
            return m.Multiply(d);
        }

        public static Matrix4D operator *(double d, Matrix4D m)
        {
            return m.Multiply(d);
        }

        public static Matrix4D operator /(Matrix4D m, double d)
        {
            return m.Divide(d);
        }

        public double LengthSquared
        {
            get { return Math.Pow(this[0, 3], 2.0) + Math.Pow(this[1, 3], 2.0) + Math.Pow(this[2, 3], 2.0); }
        }

        public double Length
        {
            get { return Math.Sqrt(LengthSquared); }
        }

        public Matrix4D Cofactor()
        {
            return new Matrix4D(new[,]{
                {
                    -(this[1, 3] * this[2, 2] * this[3, 1]) + this[1, 2] * this[2, 3] * this[3, 1] + this[1, 3] * this[2, 1] * this[3, 2] - this[1, 1] * this[2, 3] * this[3, 2] - this[1, 2] * this[2, 1] * this[3, 3] + this[1, 1] * this[2, 2] * this[3, 3], this[1, 3] * this[2, 2] * this[3, 0] - this[1, 2] * this[2, 3] * this[3, 0] - this[1, 3] * this[2, 0] * this[3, 2] + this[1, 0] * this[2, 3] * this[3, 2] + this[1, 2] * this[2, 0] * this[3, 3] - this[1, 0] * this[2, 2] * this[3, 3], -(this[1, 3] * this[2, 1] * this[3, 0]) + this[1, 1] * this[2, 3] * this[3, 0] + this[1, 3] * this[2, 0] * this[3, 1] - this[1, 0] * this[2, 3] * this[3, 1] - this[1, 1] * this[2, 0] * this[3, 3] + this[1, 0] * this[2, 1] * this[3, 3], this[1, 2] * this[2, 1] * this[3, 0] - this[1, 1] * this[2, 2] * this[3, 0] - this[1, 2] * this[2, 0] * this[3, 1] + this[1, 0] * this[2, 2] * this[3, 1] + this[1, 1] * this[2, 0] * this[3, 2] - this[1, 0] * this[2, 1] * this[3, 2]
                },
                {
                    this[0, 3] * this[2, 2] * this[3, 1] - this[0, 2] * this[2, 3] * this[3, 1] - this[0, 3] * this[2, 1] * this[3, 2] + this[0, 1] * this[2, 3] * this[3, 2] + this[0, 2] * this[2, 1] * this[3, 3] - this[0, 1] * this[2, 2] * this[3, 3], -(this[0, 3] * this[2, 2] * this[3, 0]) + this[0, 2] * this[2, 3] * this[3, 0] + this[0, 3] * this[2, 0] * this[3, 2] - this[0, 0] * this[2, 3] * this[3, 2] - this[0, 2] * this[2, 0] * this[3, 3] + this[0, 0] * this[2, 2] * this[3, 3], this[0, 3] * this[2, 1] * this[3, 0] - this[0, 1] * this[2, 3] * this[3, 0] - this[0, 3] * this[2, 0] * this[3, 1] + this[0, 0] * this[2, 3] * this[3, 1] + this[0, 1] * this[2, 0] * this[3, 3] - this[0, 0] * this[2, 1] * this[3, 3], -(this[0, 2] * this[2, 1] * this[3, 0]) + this[0, 1] * this[2, 2] * this[3, 0] + this[0, 2] * this[2, 0] * this[3, 1] - this[0, 0] * this[2, 2] * this[3, 1] - this[0, 1] * this[2, 0] * this[3, 2] + this[0, 0] * this[2, 1] * this[3, 2]
                },
                {
                    -(this[0, 3] * this[1, 2] * this[3, 1]) + this[0, 2] * this[1, 3] * this[3, 1] + this[0, 3] * this[1, 1] * this[3, 2] - this[0, 1] * this[1, 3] * this[3, 2] - this[0, 2] * this[1, 1] * this[3, 3] + this[0, 1] * this[1, 2] * this[3, 3], this[0, 3] * this[1, 2] * this[3, 0] - this[0, 2] * this[1, 3] * this[3, 0] - this[0, 3] * this[1, 0] * this[3, 2] + this[0, 0] * this[1, 3] * this[3, 2] + this[0, 2] * this[1, 0] * this[3, 3] - this[0, 0] * this[1, 2] * this[3, 3], -(this[0, 3] * this[1, 1] * this[3, 0]) + this[0, 1] * this[1, 3] * this[3, 0] + this[0, 3] * this[1, 0] * this[3, 1] - this[0, 0] * this[1, 3] * this[3, 1] - this[0, 1] * this[1, 0] * this[3, 3] + this[0, 0] * this[1, 1] * this[3, 3], this[0, 2] * this[1, 1] * this[3, 0] - this[0, 1] * this[1, 2] * this[3, 0] - this[0, 2] * this[1, 0] * this[3, 1] + this[0, 0] * this[1, 2] * this[3, 1] + this[0, 1] * this[1, 0] * this[3, 2] - this[0, 0] * this[1, 1] * this[3, 2]
                },
                {
                    this[0, 3] * this[1, 2] * this[2, 1] - this[0, 2] * this[1, 3] * this[2, 1] - this[0, 3] * this[1, 1] * this[2, 2] + this[0, 1] * this[1, 3] * this[2, 2] + this[0, 2] * this[1, 1] * this[2, 3] - this[0, 1] * this[1, 2] * this[2, 3], -(this[0, 3] * this[1, 2] * this[2, 0]) + this[0, 2] * this[1, 3] * this[2, 0] + this[0, 3] * this[1, 0] * this[2, 2] - this[0, 0] * this[1, 3] * this[2, 2] - this[0, 2] * this[1, 0] * this[2, 3] + this[0, 0] * this[1, 2] * this[2, 3], this[0, 3] * this[1, 1] * this[2, 0] - this[0, 1] * this[1, 3] * this[2, 0] - this[0, 3] * this[1, 0] * this[2, 1] + this[0, 0] * this[1, 3] * this[2, 1] + this[0, 1] * this[1, 0] * this[2, 3] - this[0, 0] * this[1, 1] * this[2, 3], -(this[0, 2] * this[1, 1] * this[2, 0]) + this[0, 1] * this[1, 2] * this[2, 0] + this[0, 2] * this[1, 0] * this[2, 1] - this[0, 0] * this[1, 2] * this[2, 1] - this[0, 1] * this[1, 0] * this[2, 2] + this[0, 0] * this[1, 1] * this[2, 2]
                }});
        }

        public Matrix4D Adjugate()
        {
            return Cofactor().Transpose();
        }

        public double Determinant()
        {
            return this[0, 0] * this[1, 1] * this[2, 2] * this[3, 3] + this[0, 0] * this[1, 2] * this[2, 3] * this[3, 1] + this[0, 0] * this[1, 3] * this[2, 1] * this[3, 2]
                   + this[0, 1] * this[1, 0] * this[2, 3] * this[3, 2] + this[0, 1] * this[1, 2] * this[2, 0] * this[3, 3] + this[0, 1] * this[1, 3] * this[2, 2] * this[3, 0]
                   + this[0, 2] * this[1, 0] * this[2, 1] * this[3, 3] + this[0, 2] * this[1, 1] * this[2, 3] * this[3, 0] + this[0, 2] * this[1, 3] * this[2, 0] * this[3, 0]
                   + this[0, 3] * this[1, 0] * this[2, 2] * this[3, 1] + this[0, 3] * this[1, 1] * this[2, 0] * this[3, 2] + this[0, 3] * this[1, 2] * this[2, 1] * this[3, 0]
                   - this[0, 0] * this[1, 1] * this[2, 3] * this[3, 2] - this[0, 0] * this[1, 2] * this[2, 1] * this[3, 3] - this[0, 0] * this[1, 3] * this[2, 2] * this[3, 1]
                   - this[0, 1] * this[1, 0] * this[2, 2] * this[3, 3] - this[0, 1] * this[1, 2] * this[2, 3] * this[3, 0] - this[0, 1] * this[1, 3] * this[2, 0] * this[3, 2]
                   - this[0, 2] * this[1, 0] * this[2, 3] * this[3, 1] - this[0, 2] * this[1, 1] * this[2, 0] * this[3, 3] - this[0, 2] * this[1, 3] * this[2, 1] * this[3, 0]
                   - this[0, 3] * this[1, 0] * this[2, 1] * this[3, 2] - this[0, 3] * this[1, 1] * this[2, 2] * this[3, 0] - this[0, 3] * this[1, 2] * this[2, 0] * this[3, 1];
        }

        public Matrix4D Inverse()
        {
            return Adjugate() / Determinant();
        }

        public Matrix4D Transpose()
        {
            return new Matrix4D(new[,]
                {
                    {this[0, 0], this[1, 0], this[2, 0], this[3, 0]},
                    {this[0, 1], this[1, 1], this[2, 1], this[3, 1]},
                    {this[0, 2], this[1, 2], this[2, 2], this[3, 2]},
                    {this[0, 3], this[1, 3], this[2, 3], this[3, 3]}
                });
        }

        public static Matrix4D CreateTranslation(double x, double y, double z)
        {
            return new Matrix4D(new[,]
                {
                    {1.0, 0.0, 0.0, x},
                    {0.0, 1.0, 0.0, y},
                    {0.0, 0.0, 1.0, z},
                    {0.0, 0.0, 0.0, 1.0}
                });
        }

        public static Matrix4D CreateTranslation(Vector3D v)
        {
            return CreateTranslation(v.X, v.Y, v.Z);
        }

        public static Matrix4D CreateRotationX(Angle theta)
        {
            return new Matrix4D(new[,]
                {
                    {1.0, 0.0, 0.0, 0.0},
                    {0.0, Trigonometry.Cos(theta), -Trigonometry.Sin(theta), 0.0},
                    {0.0, Trigonometry.Sin(theta), Trigonometry.Cos(theta), 0.0},
                    {0.0, 0.0, 0.0, 1.0}
                });
        }

        public static Matrix4D CreateRotationY(Angle theta)
        {
            return new Matrix4D(new[,]
                {
                    {Trigonometry.Cos(theta), 0.0, Trigonometry.Sin(theta), 0.0},
                    {0.0, 1.0, 0.0, 0.0},
                    {-Trigonometry.Sin(theta), 0.0, Trigonometry.Cos(theta), 0.0},
                    {0.0, 0.0, 0.0, 1.0}
                });
        }

        public static Matrix4D CreateRotationZ(Angle theta)
        {
            return new Matrix4D(new[,]
                {
                    {Trigonometry.Cos(theta), -Trigonometry.Sin(theta), 0.0, 0.0},
                    {Trigonometry.Sin(theta), Trigonometry.Cos(theta), 0.0, 0.0},
                    {0.0, 0.0, 1.0, 0.0},
                    {0.0, 0.0, 0.0, 1.0}
                });
        }

        public static Matrix4D CreateScale(double s)
        {
            return new Matrix4D(new[,]
            {
                {s, 0, 0, 0},
                {0, s, 0, 0},
                {0, 0, s, 0},
                {0, 0, 0, 1}
            });
        }


        /*
        public static Mat4 Rotate(Vect3 axis, Angle theta)
        {
            return new Mat4(new[,]
                {
                    {theta.Cos() + axis.X * axis.X * (1 - theta.Cos()), axis.X*axis.Y*(1 - theta.Cos()) - axis.Z * theta.Sin(), axis.X * axis.Z * (1 - theta.Cos()) + axis.Y*theta.Sin(), 0.0},
                    {axis.Y*axis.X*(1-theta.Cos())+axis.Z*theta.Sin(), theta.Cos() + axis.Y * axis.Y * (1 - theta.Cos()), axis.Y * axis.Z * (1 - theta.Cos()) - axis.X*theta.Sin(), 0.0},
                    {axis.Z*axis.X*(1-theta.Cos())-axis.Y*theta.Sin(), axis.Z*axis.Y*(1 - theta.Cos()) + axis.X * theta.Sin(), theta.Cos() + axis.Z * axis.Z * (1 - theta.Cos()), 0.0},
                    {0.0, 0.0, 0.0, 1.0}
                });
        }
        public static Mat4 RotateNormal(Vect3 normal)
        {
            var axis = Vect3.UnitZ.CrossProduct(normal);
            var angle = Vect3.UnitZ.DotProduct(normal)/(normal.Length);
            return Rotate(axis, Angle.FromRadians(angle));
        }*/



        public override int GetHashCode()
        {
            return (_data != null ? _data.GetHashCode() : 0);
        }

        public static Matrix4D CreateLookAt(Vector3D eye, Vector3D target, Vector3D up)
        {
            var Vector3D1 = (eye - target).Normalize();
            var right = up.CrossProduct(Vector3D1).Normalize();
            var Vector3D2 = Vector3D1.CrossProduct(right).Normalize();
            return new Matrix4D(new[,]
                {
                    {right.X, right.Y, right.Z, 0.0},
                    {Vector3D2.X, Vector3D2.Y, Vector3D2.Z, 0.0},
                    {Vector3D1.X, Vector3D1.Y, Vector3D1.Z, 0.0},
                    {0.0, 0.0, 0.0, 1.0}
                }) * CreateTranslation(-eye);
        }

        public static Matrix4D CreatePerspective(double fovy, double aspect, double zNear, double zFar)
        {
            if (fovy <= 0.0 || fovy > Math.PI)
                throw new ArgumentOutOfRangeException("fovy");
            if (aspect <= 0.0)
                throw new ArgumentOutOfRangeException("aspect");
            if (zNear <= 0.0)
                throw new ArgumentOutOfRangeException("zNear");
            if (zFar <= 0.0)
                throw new ArgumentOutOfRangeException("zFar");
            if (zNear >= zFar)
                throw new ArgumentOutOfRangeException("zNear");
            var top = zNear * Math.Tan(0.5 * fovy);
            var bottom = -top;
            var left = bottom * aspect;
            var right = top * aspect;
            return new Matrix4D(new[,]
                {
                    {2.0*zNear/(right - left), 0.0, (right + left)/(right - left), 0.0},
                    {0.0, 2.0*zNear/(top - bottom), (top + bottom)/(top - bottom), 0.0},
                    {0.0, 0.0, -(zFar + zNear)/(zFar - zNear), -(2.0*zFar*zNear)/(zFar - zNear)},
                    {0.0, 0.0, -1, 0.0}
                });
        }



        public static Matrix4D CreateOrthographic(double xMin, double xMax, double yMin, double yMax, double zMin, double zMax)
        {
            return new Matrix4D(new[,]
            {
                {2.0f / (xMax - xMin), 0, 0, -((xMax + xMin)/(xMax - xMin))},
                {0, 2.0f / (yMax - yMin), 0, -((yMax + yMin)/(yMax - yMin))},
                {0, 0, -2.0f / (zMax - zMin), -((zMax + zMin)/(zMax - zMin))},
                {0, 0, 0, 1}
            });
        }

        public static Matrix4D CreatePerspectiveFieldOfView(double fovy, double aspect, double zNear, double zFar)
        {
            if (fovy <= 0.0 || fovy > Math.PI)
                throw new ArgumentOutOfRangeException("fovy");
            if (aspect <= 0.0)
                throw new ArgumentOutOfRangeException("aspect");
            if (zNear <= 0.0)
                throw new ArgumentOutOfRangeException("zNear");
            if (zFar <= 0.0)
                throw new ArgumentOutOfRangeException("zFar");
            if (zNear >= zFar)
                throw new ArgumentOutOfRangeException("zNear");
            var top = zNear * Math.Tan(0.5 * fovy);
            var bottom = -top;
            var left = bottom * aspect;
            var right = top * aspect;
            return new Matrix4D(new[,]
            {
                {2.0*zNear/(right - left), 0.0, (right + left)/(right - left), 0.0},
                {0.0, 2.0*zNear/(top - bottom), (top + bottom)/(top - bottom), 0.0},
                {0.0, 0.0, -(zFar + zNear)/(zFar - zNear), -(2.0*zFar*zNear)/(zFar - zNear)},
                {0.0, 0.0, -1, 0.0}
            });
        }

        public override String ToString()
        {
            return "Matrix <4x4>:\n"
                   + "|" + this[0, 0] + "," + this[0, 1] + "," + this[0, 2] + "," + this[0, 3] + "|\n"
                   + "|" + this[1, 0] + "," + this[1, 1] + "," + this[1, 2] + "," + this[1, 3] + "|\n"
                   + "|" + this[2, 0] + "," + this[2, 1] + "," + this[2, 2] + "," + this[2, 3] + "|\n"
                   + "|" + this[3, 0] + "," + this[3, 1] + "," + this[3, 2] + "," + this[3, 3] + "|";
        }

        public Vector3D ToVect3()
        {
            return new Vector3D(X, Y, Z);
        }

    }
}
