using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GKYU.TranslationLibrary.Translators
{
    public class StreamCharacterScanner
        : StreamScanner<int>
        , IReadObjects<int>
    {
        public StreamCharacterScanner(StreamReader streamReader)
            : base(streamReader)
        {
            
        }

        public override void Skip(int count)
        {
            for (int n = 0; n < count; n++)
            {
                if (!NextInput())
                    break;
            }
        }
        public override void Skip2(int kind)
        {
            while (this.Peek() != kind)
            {
                if (!NextInput())
                    break;
            }
        }
        public override void Skip2(int[] kind)
        {
            while (!kind.Contains(this.Peek()))
            {
                if (!NextInput())
                    break;
            }
        }

        protected override int Next()
        {
            int result = _streamReader.Read();
            return result;
        }
    }
}
