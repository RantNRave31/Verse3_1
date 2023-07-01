using GKYU.TranslationLibrary.Symbols;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GKYU.TranslationLibrary.Translators
{
    public class EnumerableScanner<T>
        : ScannerBase<T>
        , IReadObjects<T>
    {
        protected readonly IEnumerator<T> _inputEnumerator;
        public EnumerableScanner(IEnumerable<T> input)
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
                if (!_inputEnumerator.MoveNext())
                    break;
            }
        }
        public override void Skip2(int kind)
        {
            while (((ISymbol)_inputEnumerator.Current).Kind != kind)
            {
                if (!_inputEnumerator.MoveNext())
                    break;
            }
        }
        public override void Skip2(int[] kind)
        {
            while (!kind.Contains(((ISymbol)_inputEnumerator.Current).Kind))
            {
                if (!_inputEnumerator.MoveNext())
                    break;
            }
        }
        protected override bool NextInput()
        {
            return !(EndOfInput = !_inputEnumerator.MoveNext());
        }
        protected override T Next()
        {
            T result = default(T);
            if (!EndOfInput && !EqualityComparer<T>.Default.Equals(_inputEnumerator.Current, default(T)))
            {
                result = _inputEnumerator.Current;
                EndOfFile = !NextInput();
            }
            return result;
        }
    }
}
