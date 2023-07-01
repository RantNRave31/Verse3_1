using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.MathLibrary.Tensors.Metrics
{
    /// <summary>
    /// Statistically, Mean Absolute Error (MAE) refers to a the results of measuring
    /// the difference between two continuous variables. This is simple an average sum
    /// of all absolute errors. The MAE is mostly used to check performance of model
    /// doing forecast prediction in time series. Below is the mathematical interpretation.
    /// </summary>
    public class MeanAbsoluteError : BaseMetric
    {
        public MeanAbsoluteError() : base("mean_absolute_error")
        {

        }

        public override NDArray Calculate(NDArray preds, NDArray labels)
        {
            var error = preds - labels;
            return Mean(Abs(error));
        }
    }
}
