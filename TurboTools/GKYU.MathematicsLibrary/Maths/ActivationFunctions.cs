using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.MathLibrary.Tensors
{
    public static class ActivationFunctions
    {
        /// <summary>
        /// The Heavyside Step Function
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Step(double x)
        {
            if (x < 0.0) return 0.0;
            else return 1.0;
        }
        /// <summary>
        /// The Logistic Sigmoid Activation Function
        /// returns a constrained value between 0 and 1
        /// f(x) = 1.0 / (1.0 + e-x)
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double LogisticSigmoid(double x)
        {
            if (x < -45.0) return 0.0;
            else if (x > 45.0) return 1.0;
            else return 1.0 / (1.0 + Math.Exp(-x));
        }

        /// <summary>
        /// The Hyperbolic Tangent Activation Function
        /// returns a constrained value between -1 and +1
        /// f(x) = (ex - e-x) / (ex + e-x)
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double HyperbolicTangent(double x)
        {
            if (x < -45.0) return -1.0;
            else if (x > 45.0) return 1.0;
            else return Math.Tanh(x);
        }
        /// <summary>
        /// The Softmax Activation Function
        /// 
        /// The softmax activation function is designed so that a return value is in the range (0,1) and the sum
        /// of all return values for a particular layer is 1.0. For example, the demo program output values when 
        /// using the softmax activation function are 0.4725 and 0.5275 -- notice they sum to 1.0. The idea is 
        /// that output values can then be loosely interpreted as probability values, which is extremely useful when 
        /// dealing with categorical data.
        /// 
        /// Scale = Exp(0.37) + Exp(0.50) = 1.4477 + 1.6487 = 3.0965
        ///   where .37 and .50 are the preactivation sums of the hidden layer nodes
        /// Then the activation return values are computed like so:
        /// F(0.37) = Exp(0.37) / scale = 1.4477 / 3.0965 = 0.47
        /// F(0.50) = Exp(0.50) / scale = 1.6487 / 3.0965 = 0.53
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="layer"></param>
        /// <returns>a computed value between 0 and 1</returns>
        public static double ihSum0;
        public static double ihSum1;
        public static double hoSum0;
        public static double hoSum1;
        public static double SoftMax(double x, string layer)
        {
            // Naive version 
            double scale = 0.0;
            if (layer == "ih")
                scale = Math.Exp(ihSum0) + Math.Exp(ihSum1);
            else if (layer == "ho")
                scale = Math.Exp(hoSum0) + Math.Exp(hoSum1);
            else
                throw new Exception("Unknown layer");

            return Math.Exp(x) / scale;
        }
        /// <summary>
        /// The algebra is a bit tricky, but the main idea is to avoid arithmetic overflow. If you trace
        /// through the function, and use the fact that Exp(a - b) = Exp(a) / Exp(b), you'll see how the
        /// logic works.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static double SoftMax2(double x, string layer)
        {
            double max = double.MinValue;
            if (layer == "ih")
                max = (ihSum0 > ihSum1) ? ihSum0 : ihSum1;
            else if (layer == "ho")
                max = (hoSum0 > hoSum1) ? hoSum0 : hoSum1;

            double scale = 0.0;
            if (layer == "ih")
                scale = Math.Exp(ihSum0 - max) + Math.Exp(ihSum1 - max);
            else if (layer == "ho")
                scale = Math.Exp(hoSum0 - max) + Math.Exp(hoSum1 - max);

            return Math.Exp(x - max) / scale;
        }
    }
}
