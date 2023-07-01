using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GKYU.CoreLibrary.ErrorHandling;

namespace GKYU.TranslationLibrary.ErrorHandling
{
    public partial class Error
        : ErrorBase
    {
        public object source;
        public string message { get; set; }
        public string Name
        {
            get
            {
                if (ErrorCodes.ContainsKey((Error.CODE)errorCodeID))
                    return "<UNDEFINED>";
                else
                    return ErrorCodes[(Error.CODE)errorCodeID].name;
            }
        }

        public int Progress
        {
            get
            {
                return _progress;
            }
            set
            {
                if (value == _progress)
                    return;
                _progress = value;
           }
        }

        public bool Handled
        {
            get
            {
                return _handled;
            }
            set
            {
                if (value == _handled)
                    return;
                _handled = value;
            }
        }
        public static event Action<Error> OnErrorHandled;
        public Error(object source, Error.CODE errorCodeID, string message)
            : base()
        {
            this.errorCodeID = (int)errorCodeID;
            this.message = message;
        }
        public static Error Report(object source, Error.CODE errorCodeID, string message)
        {
            Error result = new Error(source, errorCodeID, message);
            return result;
        }
    }
}
