using System;
using System.Collections.Generic;

namespace GKYU.TranslationLibrary.Domains.Texts
{
    public partial class Text
    {
        public class Templates
            : Dictionary<Type,Text.Template>
        {
        }
    }
}
