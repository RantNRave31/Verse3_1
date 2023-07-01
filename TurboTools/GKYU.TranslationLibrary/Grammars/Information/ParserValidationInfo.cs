using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.TranslationLibrary.Grammars.Information
{
    using GKYU.CoreLibrary.ErrorHandling;
    using GKYU.TranslationLibrary.ErrorHandling;

    public class ParserValidationInfo
    {
        public bool Valid = true;
        public List<Error> errors = new List<Error>();
    }
}
