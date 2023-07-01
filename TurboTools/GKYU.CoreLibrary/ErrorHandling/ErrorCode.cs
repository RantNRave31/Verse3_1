using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.CoreLibrary.ErrorHandling
{
    public class ErrorCode
    {
        public int errorID;
        public string name;
        public string message;
        public ErrorCode(int errorID, string name, string message)
        {
            this.errorID = errorID;
            this.name = name;
            this.message = message;
        }
    }
}
