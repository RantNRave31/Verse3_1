using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.MathLibrary.Tensors.Metrics
{
    /// <summary>
    /// Binary accuracy fits well with Binary Cross-entropy cost function. The function
    /// just compare the predicted and expected result and give you a result of how much
    /// the model performed with its current training. Let’s implement this metric which
    /// will be very easy.
    /// </summary>
    public class BinaryAccuacy : BaseMetric
    {
        public BinaryAccuacy() : base("binary_accurary")
        {
        }

        public override NDArray Calculate(NDArray preds, NDArray labels)
        {
            var output = Round(Clip(preds, 0, 1));
            return Mean(output == labels);
        }
    }
}
