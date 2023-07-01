using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.TranslationLibrary.Translators
{
    public class StreamLexer
        : StreamScanner<Token>
        , IReadSymbols<Token>

    {
        public StreamLexer(StreamReader streamReader)
            : base(streamReader)
        {

        }
        public override void Skip(int count)
        {
            for (int n = 0; n < count; n++)
            {
                if (!this.NextInput())
                    break;
            }
        }
        public override void Skip2(int kind)
        {
            while (_streamReader.Peek() == kind)
            {
                if (!this.NextInput())
                    break;
            }
        }
        public override void Skip2(int[] kind)
        {
            while (kind.Contains(_streamReader.Peek()))
            {
                if (!this.NextInput())
                    break;
            }
        }

        protected override Token Next()
        {
            Token token = new Token() { Kind = -1 };
            if (-1 == _streamReader.Peek())
            {
                EndOfInput = true;
                return token;
            }
            token.Kind = _streamReader.Peek();
            token.Value += (char)_streamReader.Read();
            return token;
        }
    }
}
