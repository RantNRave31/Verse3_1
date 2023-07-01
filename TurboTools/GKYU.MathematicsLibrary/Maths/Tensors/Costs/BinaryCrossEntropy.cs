using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.MathLibrary.Tensors.Layers.Costs
{
    /// <summary>
    /// This cost function is used for classification problem when we have to predict its the output
    /// of a problem is one of 2 values. It’s like predicting yes or no for a problem usually represented
    /// by binary value 1 and 0.
    /// </summary>
    public class BinaryCrossEntropy : BaseCost
    {
        public BinaryCrossEntropy() : base("binary_crossentropy")
        {

        }

        public override NDArray Forward(NDArray preds, NDArray labels)
        {
            var output = Clip(preds, Epsilon, 1 - Epsilon);

            output = Mean(-(labels * Log(output) + (1 - labels) * Log(1 - output)));
            return output;
        }

        public override NDArray Backward(NDArray preds, NDArray labels)
        {
            var output = Clip(preds, Epsilon, 1 - Epsilon);
            return (output - labels) / (output * (1 - output));
        }
    }
}
