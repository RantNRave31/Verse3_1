using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GKYU.TranslationLibrary.Translators
{
    using GKYU.CoreLibrary.Attributes;

    //using GKYU.CollectionsLibrary.Collections.Generic;

    public abstract class ScannerBase<OUTPUT_TYPE>
        : IReadObjects<OUTPUT_TYPE>
        , IEnumerable<OUTPUT_TYPE>
    {
        public class Enumerator
            : IEnumerator<OUTPUT_TYPE>
        {
            ScannerBase<OUTPUT_TYPE> _scanner;
            Stack<OUTPUT_TYPE> _current = new Stack<OUTPUT_TYPE>();
            object System.Collections.IEnumerator.Current
            {
                get
                {
                    return this.Current;
                }
            }
            public OUTPUT_TYPE Current
            {
                get
                {
                    return _current.Peek();
                }
            }
            public Enumerator(ScannerBase<OUTPUT_TYPE> scanner)
            {
                _scanner = scanner;
                _current.Push(default(OUTPUT_TYPE));// initialize stack with empty set/null/default
            }
            public void Dispose()
            {
                _scanner = null;
            }
            public void Reset()
            {
                _scanner = null;
            }
            public bool MoveNext()
            {
                _current.Pop();
                if(!_scanner.EndOfInput)
                    _current.Push(_scanner.Read());
                return _current.Count != 0;
            }
        }
        public Parameters Parameters { get; set; }
        public Metrics Metrics { get; set; }
        public static Type OutputType { get { return typeof(OUTPUT_TYPE); } }
        protected Queue<OUTPUT_TYPE> _outputQueue = new Queue<OUTPUT_TYPE>();
        public ScannerBase()
        {
            Parameters = new Parameters();
            Metrics = new Metrics();
        }
        public bool EndOfInput { get; protected set; }
        public bool EndOfFile { get; protected set; }
        protected abstract bool NextInput();
        protected abstract OUTPUT_TYPE Next();
        public abstract void Skip(int count);
        public abstract void Skip2(int kind);
        public abstract void Skip2(int[] kind);
        public OUTPUT_TYPE Peek(int k = 0)
        {
            if(!EndOfInput)
            for (int n = _outputQueue.Count; n < k + 1; n++)
            {
                _outputQueue.Enqueue(Next());
            }
            if(k < _outputQueue.Count)
                return _outputQueue.ToList()[k];
            EndOfInput = true;// added, so first peek of translator can set end of input
            return default(OUTPUT_TYPE);
        }
        public OUTPUT_TYPE Read()
        {
            OUTPUT_TYPE result;
            if (_outputQueue.Count > 0)
            {
                result = _outputQueue.Dequeue();
            }
            else
            {
                result = Next();
            }
            if (EndOfInput && _outputQueue.Count == 0)
                EndOfFile = true;
            return result;
        }
        public IEnumerator<OUTPUT_TYPE> GetEnumerator()
        {
            return new Enumerator(this);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        protected void Emit(OUTPUT_TYPE output)
        {
            this._outputQueue.Enqueue(output);
        }
    }
}
