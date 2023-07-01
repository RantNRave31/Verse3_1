using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GKYU.TranslationLibrary.Translators;
using GKYU.TranslationLibrary.Symbols;

namespace GKYU.TranslationLibrary.Domains.Texts
{
    public partial class Text
    {
        public class Formatter<INPUT_TYPE>
            : FormatterBase<INPUT_TYPE>
            where INPUT_TYPE : ISymbol
        {
            protected readonly MacroProcessor _macroProcessor;
            protected readonly TextWriter _textWriter;
            public readonly Templates Templates = new Templates();
            public Formatter(MacroProcessor macroProcessor, TextWriter textWriter, Templates templates = null)
            {
                _macroProcessor = macroProcessor;
                _textWriter = textWriter;
                if (null != templates)
                {
                    foreach (KeyValuePair<Type, Text.Template> keyValuePair in templates)
                    {
                        Templates.Add(keyValuePair.Key, keyValuePair.Value);
                    }
                }
            }
            public override int Write<T>(T node)
            {
                _macroProcessor.PushContext();
                _macroProcessor.Add(node.GetType().Name, node);
                if (Templates.ContainsKey(node.GetType()))
                {
                    var template = Templates[node.GetType()];
                    string text = template.Generate();
                    _textWriter.Write(text);

                }
                else
                    _textWriter.Write(node.ToString());
                _macroProcessor.PopContext();
                return 0;
            }
        }
    }
}
