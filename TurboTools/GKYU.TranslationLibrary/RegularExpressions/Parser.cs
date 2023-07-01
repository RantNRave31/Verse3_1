using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.TranslationLibrary.Grammars.RegularExpressions
{
    public class Parser
    {
        private readonly StreamReader inputStream;
        private int lastInput;
        public Parser(StreamReader inputStream)
        {
            this.inputStream = inputStream;
        }
        public int Peek()
        {
            return inputStream.Peek();
        }
        public void Next()
        {
            lastInput = inputStream.Read();
        }
        public void Expect(int characterCode)
        {
            if (inputStream.Peek() != characterCode)
                throw new Exception("EXPECTS");
            Next();
        }
        private RegularExpression.SyntaxNode regex()
        {
            RegularExpression.SyntaxNode term = this.term();
            if (inputStream.Peek() != (int)RegularExpression.SYMBOL.EOF && inputStream.Peek() == '|')
            {
                Expect('|');
                RegularExpression.SyntaxNode regex = this.regex();
                return new RegularExpression.Choice(term, regex);
            }
            else
                return term;
        }
        private RegularExpression.SyntaxNode term()
        {
            RegularExpression.SyntaxNode factor = new RegularExpression.Empty();
            while (inputStream.Peek() != (int)RegularExpression.SYMBOL.EOF && inputStream.Peek() != ')' && inputStream.Peek() != '|')
            {
                RegularExpression.SyntaxNode nextFactor = this.factor();
                factor = new RegularExpression.Sequence(factor, nextFactor);
            }
            return factor;
        }
        private RegularExpression.SyntaxNode factor()
        {
            RegularExpression.SyntaxNode atom = this.atom();
            while (inputStream.Peek() != (int)RegularExpression.SYMBOL.EOF && inputStream.Peek() == '*')
            {
                Expect('*');
                atom = new RegularExpression.Repitition(atom);
            }
            return atom;
        }
        private RegularExpression.SyntaxNode atom()
        {
            switch (inputStream.Peek())
            {
                case -1:
                    return new RegularExpression.Empty();
                case '(':
                    Expect('(');
                    RegularExpression.SyntaxNode r = regex();
                    Expect(')');
                    return r;
                case '\\':
                    Expect('\\');
                    char esc = (char)inputStream.Peek();
                    this.Next();
                    return new RegularExpression.Primative(esc);
                default:
                    char p = (char)inputStream.Peek();
                    this.Next();
                    return new RegularExpression.Primative(p);
            }
        }
        public RegularExpression.Declaration Parse()
        {
            RegularExpression.Declaration declaration = new RegularExpression.Declaration(regex());
            return declaration;
        }
    }
}
