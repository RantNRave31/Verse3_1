using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.TranslationLibrary
{
    public interface IWriteObject
    {
        void PushTab();
        void PopTab();
        void Tab();
        void Write(char c);
        void Write(string formatString, params object[] arguments);
        void WriteLine(char c);
        void WriteLine(string formatString, params object[] arguments);
    }
    public class ObjectWriter
        : IWriteObject
    {
        private readonly StreamWriter _streamWriter;
        private StringBuilder _tab;
        public ObjectWriter(StreamWriter streamWriter)
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
}
