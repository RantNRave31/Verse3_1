using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GKYU.TranslationLibrary.Symbols;

namespace GKYU.TranslationLibrary
{
    public class Code
    {
        public class Variable
            : Symbol.Named
        {
            public Variable()
                : base()
            {

            }
            public Variable(Symbol.Table symbolTable)
                : base(symbolTable)
            {
            }
        }
    }
}
