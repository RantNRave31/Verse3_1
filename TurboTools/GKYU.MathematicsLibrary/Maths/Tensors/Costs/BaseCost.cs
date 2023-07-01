using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.MathLibrary.Tensors.Layers.Costs
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseCost : Operations
    {
        public string Name { get; set; }

        public BaseCost(string name)
        {
            Name = name;
        }

        public abstract NDArray Forward(NDArray preds, NDArray labels);

        public abstract NDArray Backward(NDArray preds, NDArray labels);
    }
}
