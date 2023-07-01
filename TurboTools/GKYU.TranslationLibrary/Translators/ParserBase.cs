using GKYU.CoreLibrary;
using GKYU.TranslationLibrary.Symbols;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GKYU.TranslationLibrary.Translators
{
    public abstract class ParserBase<INPUT_TYPE,OUTPUT_TYPE>
        : LexerBase<OUTPUT_TYPE>
        , IReadSymbols<OUTPUT_TYPE>
        where INPUT_TYPE : ISymbol
        where OUTPUT_TYPE : ISymbol
    {
        public static Type InputType { get { return typeof(INPUT_TYPE); } }
        protected IReadSymbols<INPUT_TYPE> _scanner;
        protected INPUT_TYPE _lastInput;
        protected INPUT_TYPE CurrentInput
        {
            get { return _scanner.Peek(0); }
        }
        public ParserBase(IReadSymbols<INPUT_TYPE> scanner)
            : base()
        {
            _scanner = scanner;
            _scanner.Peek();// always peek first, to ensure end of Input
            EndOfFile = EndOfInput = scanner.EndOfFile;// set to ensure bad things don't happen. ;-)
        }
        protected override bool NextInput()
        {
            _lastInput = _scanner.Read();
            //if (EndOfInput = _scanner.EndOfFile)
            //    return false;
            EndOfInput = _scanner.EndOfFile;
            return true;
        }
        protected bool Accept(int kind)
        {
            if (kind.Equals(CurrentInput.Kind))
            {
                NextInput();
                return true;
            }
            return false;
        }
        protected bool Accept(int kind, string value)
        {
            if (kind.Equals(CurrentInput.Kind) && (value == CurrentInput.Value))
            {
                NextInput();
                return true;
            }
            return false;
        }
        protected bool Accept(HashSet<int> kind)
        {
            if (kind.Contains(CurrentInput.Kind))
            {
                NextInput();
                return true;
            }
            return false;
        }
        protected void Expect(int kind, string errorMessage)
        {
            if (Accept(kind))
            {
                return;
            }
            throw new Exception(string.Format("ERROR:  {0}", errorMessage));
        }
        protected void Expect(int kind, string value, string errorMessage)
        {
            if (Accept(kind, value))
            {
                return;
            }
            throw new Exception(string.Format("ERROR:  {0}", errorMessage));
        }
        protected void Expect(HashSet<int> kind, string errorMessage)
        {
            if (Accept(kind))
            {
                return;
            }
            throw new Exception(string.Format("ERROR:  {0}", errorMessage));
        }
        protected bool CurrentIs(int kind)
        {
            if (kind.Equals(CurrentInput.Kind))
            {
                return true;
            }
            return false;
        }
        protected bool NextIs(int kind)
        {
            if (kind.Equals(_scanner.Peek(1).Kind))
            {
                return true;
            }
            return false;
        }
        protected bool CurrentIs(int kind, string value)
        {
            if (kind.Equals(CurrentInput.Kind) && (value == CurrentInput.Value))
            {
                return true;
            }
            return false;
        }
        protected bool NextIs(int kind, string value)
        {
            if (kind.Equals(_scanner.Peek(1).Kind) && (value == _scanner.Peek(1).Value))
            {
                return true;
            }
            return false;
        }
        protected bool CurrentIs(HashSet<int> kind)
        {
            if (kind.Contains(CurrentInput.Kind))
            {
                return true;
            }
            return false;
        }
        protected bool PeekIs(int k, int kind)
        {
            if (kind.Equals(_scanner.Peek(k).Kind))
            {
                return true;
            }
            return false;
        }
        protected bool PeekIs(int k, HashSet<int> kind)
        {
            if (kind.Contains(_scanner.Peek(k).Kind))
            {
                return true;
            }
            return false;
        }
        protected bool Accept(Type kind)
        {
            if (kind == CurrentInput.GetType())
            {
                NextInput();
                return true;
            }
            return false;
        }
        protected bool Accept(Type[] kind)
        {
            if (kind.Contains(CurrentInput.GetType()))
            {
                NextInput();
                return true;
            }
            return false;
        }
        protected void Expect(Type kind, string errorMessage)
        {
            if (Accept(kind))
            {
                return;
            }
            throw new Exception(string.Format("ERROR:  {0}", errorMessage));
        }
        protected void Expect(Type[] kind, string errorMessage)
        {
            if (Accept(kind))
            {
                return;
            }
            throw new Exception(string.Format("ERROR:  {0}", errorMessage));
        }
        protected bool CurrentIs(Type kind)
        {
            if (CurrentInput.GetType() == kind)
            {
                return true;
            }
            return false;
        }
        protected bool CurrentIs(Type[] kind)
        {
            if (kind.Contains(CurrentInput.GetType()))
            {
                return true;
            }
            return false;
        }
        protected bool PeekIs(int k, Type kind)
        {
            if (_scanner.Peek(k).GetType() == kind)
            {
                return true;
            }
            return false;
        }
        protected bool PeekIs(int k, Type[] kind)
        {
            if (kind.Contains(_scanner.Peek(k).GetType()))
            {
                return true;
            }
            return false;
        }
        public override void Skip(int count)
        {
            for(int n = 0; n < count; n++)
            {
                if (!this.NextInput())
                    break;
            }
        }
        public override void Skip2(int kind)
        {
            while (this.CurrentInput.Kind == kind)
            {
                if (!this.NextInput())
                    break;
            }
        }
        public override void Skip2(int[] kind)
        {
            while (kind.Contains(this.CurrentInput.Kind))
            {
                if (!this.NextInput())
                    break;
            }
        }
    }
}
