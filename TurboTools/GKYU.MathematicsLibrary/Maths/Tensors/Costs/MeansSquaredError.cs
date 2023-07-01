using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.MathLibrary.Tensors.Layers.Costs
{
    /// <summary>
    /// This is the most commonly used loss function for the regression problem to check the
    /// fitness of data against the model. It’s easy to understand and implement and generally
    /// works pretty well. To calculate MSE, you take the difference between your predictions and
    /// the ground truth, square it, and average it out across the whole dataset. 
    /// </summary>
    public class MeanSquaredError : BaseCost
    {
        public MeanSquaredError() : base("mean_squared_error")
        {

        }

        public override NDArray Forward(NDArray preds, NDArray labels)
        {
            var error = preds - labels;
            return Mean(Square(error));
        }

        public override NDArray Backward(NDArray preds, NDArray labels)
        {
            double norm = 2 / (double)preds.Shape[0];
            return norm * (preds - labels);
        }
    }
}
