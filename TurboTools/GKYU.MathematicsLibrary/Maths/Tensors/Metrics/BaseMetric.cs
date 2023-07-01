using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.MathLibrary.Tensors.Metrics
{
    /// <summary>
    /// A metric is a function that is used to judge the performance of your model. It is
    /// similar to cost function, the difference is we don’t feed the performance back to
    /// the network for improvement. So we can use most of the cost functions as a metrics
    /// which makes it easy to reuse the code. We will define 2 metrics for our implementation
    /// and as I always says open the project to add more metrics.
    /// </summary>
    public abstract class BaseMetric : Operations
    {
        public string Name { get; set; }

        public BaseMetric(string name)
        {
            Name = name;
        }

        public abstract NDArray Calculate(NDArray preds, NDArray labels);
    }
}
