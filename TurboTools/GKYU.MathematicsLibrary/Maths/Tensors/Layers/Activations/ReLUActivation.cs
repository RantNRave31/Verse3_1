using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.MathLibrary.Tensors.Layers.Activations
{
    public class ReLU : BaseActivation
    {
        public ReLU() : base("relu")
        {

        }

        public override void Forward(NDArray x)
        {
            base.Forward(x);

            NDArray matches = x > 0;
            Output = matches * x;
        }

        public override void Backward(NDArray grad)
        {
            InputGrad = grad * (Input > 0);
        }
    }
}
