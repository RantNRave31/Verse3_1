using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GKYU.CoreLibrary.Expressions
{
    public class InvalidQueryException
        : Exception
    {
        private string _message;

        public InvalidQueryException(string message)
        {
            this._message = message + " ";
        }

        public override string Message
        {
            get {
                return "The client query is invalid: " + _message;
            }
        }
    }
}
