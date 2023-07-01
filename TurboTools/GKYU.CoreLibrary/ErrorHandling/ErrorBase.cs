using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.CoreLibrary.ErrorHandling
{
    public class ErrorBase
    {
        public int errorCodeID;
        protected bool _handled;
        protected int _progress;
    }
}
