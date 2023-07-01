using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.TranslationLibrary.Grammars.Analyzers
{
    using GKYU.CollectionsLibrary.Collections.Graphs;
    using GKYU.CoreLibrary.ErrorHandling;
    using GKYU.TranslationLibrary.ErrorHandling;
    using GKYU.TranslationLibrary.Grammars.Information;

    public class ValidationPass
        : AnalysisPass
    {
        public GrammarValidationInfo result;
        public ValidationPass(GrammarValidationInfo info = null)
            : base()
        {
            this.result = (info == null)? new GrammarValidationInfo() : info;
        }
        public void ReportLexerError(int errorCodeID, string message)
        {
            result.lexerResult.Valid = false;
            //result.lexerResult.errors.Add(Error.Report(errorCodeID, message));
        }
        public void ReportParserError(int errorCodeID, string message)
        {
            result.parserResult.Valid = false;
            //result.parserResult.errors.Add(Error.Report(errorCodeID, message));
        }
        protected void TestLexerDeclarations(Syntax.SymbolTable symbolTable)
        {
            if (symbolTable.lexerDeclarations.Count == 0)
                ReportLexerError((int)Error.CODE.NoLexerDeclared, "There are no lexers declared");
            if (symbolTable.lexerDeclarations.Count > 1)
                ReportLexerError((int)Error.CODE.MultipleLexerDeclarations, "There are multiple lexers defined");
        }
        protected void TestLexerGraphs(Syntax.SymbolTable symbolTable)
        {
            Graph<int, int> nfa;
            Graph<int, int> dfa;

            Syntax.LexerDeclaration lexerDeclaration = symbolTable.lexerDeclarations.Values.FirstOrDefault();
            nfa = (Graph<int, int>)lexerDeclaration;
            dfa = nfa.ToDFA();

            HashSet<Graph<int, int>.Node> acceptStates = dfa.AcceptStates();
            if (acceptStates.Count <= 0)
                ReportLexerError((int)Error.CODE.NFA_NoAcceptStates, "lexer DFA contains NO Accept States");

            foreach (Syntax.TokenDeclaration tokenDeclaration in symbolTable.tokenDeclarations.Values)
            {

            }
        }
        protected void TestParserDeclarations(Syntax.SymbolTable symbolTable)
        {
            if (symbolTable.parserDeclarations.Count == 0)
                ReportParserError((int)Error.CODE.NoParserDeclared, "There are no parsers declared");
            if (symbolTable.lexerDeclarations.Count > 1)
                ReportParserError((int)Error.CODE.MultipleParserDeclarations, "There are multiple parsers defined");
        }
        public override void Visit(Syntax.SymbolTable symbolTable)
        {
            TestLexerDeclarations(symbolTable);
            TestLexerGraphs(symbolTable);
            TestParserDeclarations(symbolTable);
        }

        public override void Accept(IVisitAnalysisPass visitor)
        {
            visitor.Visit(this);
        }
    }
}
