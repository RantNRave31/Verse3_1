using GKYU.TranslationLibrary.Symbols;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GKYU.TranslationLibrary.Translators
{
    public class EnumerableLexer<INPUT_TYPE, OUTPUT_TYPE>
        : LexerBase<OUTPUT_TYPE>
        , IReadSymbols<OUTPUT_TYPE>
        where OUTPUT_TYPE : ISymbol
    {
        protected readonly IEnumerator<INPUT_TYPE> _inputEnumerator;
        public EnumerableLexer(IEnumerable<INPUT_TYPE> input)
            : base()
        {
            if (input != null)
            {
                _inputEnumerator = input.GetEnumerator();
                EndOfFile = EndOfInput = !_inputEnumerator.MoveNext();
            }
            else
            {
                EndOfFile = EndOfInput = true;
            }
        }
        public override void Skip(int count)
        {
            for (int n = 0; n < count; n++)
            {
                Read();
            }
        }
        public override void Skip2(int kind)
        {
            while (!EndOfFile && ((ISymbol)Peek()).Kind != kind)
            {
                Read();
            }
        }
        public override void Skip2(int[] kind)
        {
            while (!EndOfFile && !kind.Contains(((ISymbol)Peek()).Kind))
            {
                Read();
            }
        }
        protected override bool NextInput()
        {
            return !(EndOfInput = !_inputEnumerator.MoveNext());
        }
        protected override OUTPUT_TYPE Next()
        {
            OUTPUT_TYPE result = ((OUTPUT_TYPE)Activator.CreateInstance(typeof(OUTPUT_TYPE)));
            result.Kind = -1;
            if (!EndOfInput && !EqualityComparer<INPUT_TYPE>.Default.Equals(_inputEnumerator.Current, default(INPUT_TYPE)))
            {
                result.Kind = 0;
                result.Value = _inputEnumerator.Current.ToString();
                EndOfFile = !NextInput();
            }
            return result;
        }
    }
}
