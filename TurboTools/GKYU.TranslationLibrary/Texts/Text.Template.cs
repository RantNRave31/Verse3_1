using System.Collections.Generic;
using System.Text;

namespace GKYU.TranslationLibrary.Domains.Texts
{
    public partial class Text
    {
        public class Template
        {
            protected MacroProcessor _macroProcessor;
            protected string _templateText;
            public Template(MacroProcessor macroProcessor, string templateText)
            {
                _macroProcessor = macroProcessor;
                _templateText = templateText;
            }
            public override string ToString()
            {
                return string.Format(_templateText);
            }
            public string Generate()
            {
                string expandedText = _macroProcessor.Parse(_templateText);
                return expandedText;
            }
        }
    }
}
