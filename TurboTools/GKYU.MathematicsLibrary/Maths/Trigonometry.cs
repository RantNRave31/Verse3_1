using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.MathLibrary.Tensors
{
    public class Trigonometry
    {
        public const double PI = Math.PI;
        public const double TwoPi = Math.PI * 2.0;
        public const double PiOverTwo = Math.PI / 2.0;

        public static double Sqrt(double d)
        {
            return Math.Pow(d, 0.5);
        }
        public static double Pow(double x, double y)
        {
            return Math.Pow(x, y);
        }

        public static double Sin(Angle angle)
        {
            return Math.Sin(angle.Radians);
        }
        public static double Cos(Angle angle)
        {
            return Math.Cos(angle.Radians);
        }
        public static double Tan(Angle angle)
        {
            return Math.Tan(angle.Radians);
        }

        public static Angle Acos(double d)
        {
            return Angle.FromRadians(Math.Acos(d));
        }
        public static Angle Asin(double d)
        {
            return Angle.FromRadians(Math.Asin(d));
        }
        public static Angle Atan(double d)
        {
            return Angle.FromRadians(Math.Asin(d));
        }

        public static Angle Atan2(double y, double x)
        {
            return Angle.FromRadians(Math.Atan2(y, x));
        }

        public static double Abs(double d)
        {
            if (d >= 0.0)
                return d;
            return -d;
        }
    }
}
