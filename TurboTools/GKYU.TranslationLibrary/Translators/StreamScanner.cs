using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GKYU.TranslationLibrary.Translators
{
    public abstract class StreamScanner<T>
        : ScannerBase<T>
        , IReadObjects<T>
    {
        protected readonly StreamReader _streamReader;
        public StreamScanner(StreamReader streamReader)
            : base()
        {
            _streamReader = streamReader;
        }
        public void SkipLines(int count)
        {
            for (int n = 0; n < count; n++)
            {
                if (_streamReader.EndOfStream)
                    return;
                Read();
            }
        }
        protected override bool NextInput()
        {
            _streamReader.Read();
            return (!(EndOfInput != _streamReader.EndOfStream));
        }

    }
}
