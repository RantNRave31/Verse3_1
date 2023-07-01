using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GKYU.TranslationLibrary.Translators
{
    public class StreamLineScanner
        : StreamScanner<string>
        , IReadObjects<string>
    {
        protected enum STATE : int
        {
            INITIAL,
            MATCH,
            NO_MATCH,
            LINE,
            EOL,
            EOF,
            FINAL,
        }
        public StreamLineScanner(StreamReader streamReader)
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
        protected override bool NextInput()
        {
            _streamReader.Read();
            return (!(EndOfInput != _streamReader.EndOfStream));
        }
        protected override string Next()
        {
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                switch (STATE.INITIAL)
                {
                    case STATE.INITIAL:
                        if (-1 == _streamReader.Peek())
                        {
                            EndOfInput = true;
                            goto case STATE.FINAL;
                        }
                        if ('\r' == (char)_streamReader.Peek())
                            goto case STATE.EOL;
                        goto case STATE.LINE;
                    case STATE.MATCH:// Return Match
                        if (-1 == _streamReader.Peek())
                        {
                            EndOfInput = true;
                            EndOfFile = true;
                            goto case STATE.FINAL;
                        }
                        goto case STATE.FINAL;// return it for real now
                    case STATE.NO_MATCH:
                        NextInput();
                        if (-1 == _streamReader.Peek())
                        {
                            EndOfInput = true;
                            EndOfFile = true;
                            goto case STATE.FINAL;
                        }
                        goto case STATE.INITIAL;// Skip No Match
                    case STATE.LINE:
                        sb.Append((char)_streamReader.Peek());
                        NextInput();
                        if (-1 == _streamReader.Peek())
                        {
                            EndOfInput = true;
                            EndOfFile = true;
                            goto case STATE.MATCH;
                        }
                        if ('\r' == (char)_streamReader.Peek() || '\n' == (char)_streamReader.Peek())
                            goto case STATE.EOL;
                        goto case STATE.LINE;
                    case STATE.EOL:
                        NextInput();
                        if (-1 == _streamReader.Peek())
                        {
                            EndOfInput = true;
                            EndOfFile = true;
                            goto case STATE.MATCH;
                        }
                        if ('\n' == _streamReader.Peek())
                            goto case STATE.EOL;
                        goto case STATE.MATCH;
                    //case STATE.EOF:
                    //    goto case STATE.FINAL;
                    case STATE.FINAL:
                        return sb.ToString();
                }
            }
        }
    }
}
