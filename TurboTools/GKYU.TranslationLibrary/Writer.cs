using GKYU.TranslationLibrary.Symbols;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.TranslationLibrary
{
    public interface IWriteSymbols<T>
        where T : ISymbol
    {
        void Write<VISITOR_TYPE>(T element) where VISITOR_TYPE : IVisitSymbol;
        
    }
    public class Writer
    {
        private readonly StreamWriter _streamWriter;
        private StringBuilder _tab;
        public Writer(StreamWriter streamWriter)
        {
            _streamWriter = streamWriter;
            _tab = new StringBuilder();
        }
        public void PushTab()
        {
            _tab.Append("    ");
        }
        public void PopTab()
        {
            _tab.Length -= 4;
        }
        public void Tab()
        {
            _streamWriter.Write(_tab);
        }
        public void Write(char c)
        {
            _streamWriter.Write(c);
        }
        public void Write(string formatString, params object[] arguments)
        {
            _streamWriter.Write(formatString, arguments);
        }
        public void WriteLine(char c)
        {
            _streamWriter.WriteLine(c);
        }
        public void WriteLine(string formatString, params object[] arguments)
        {
            _streamWriter.WriteLine(formatString, arguments);
        }
    }
    public class Writer<T>
        : Writer
        where T : ISymbol
    {
        static Writer()
        {

        }
        public Writer(StreamWriter streamWriter)
            : base(streamWriter)
        {

        }

        public void Write<VISITOR_TYPE>(T element)//(T)Activator.CreateInstance(typeof(T), default(T))
            where VISITOR_TYPE : IVisitSymbol
        {
            VISITOR_TYPE visitor = (VISITOR_TYPE)Activator.CreateInstance(typeof(VISITOR_TYPE), this);
            element.Accept(visitor);
        }
    }
}
