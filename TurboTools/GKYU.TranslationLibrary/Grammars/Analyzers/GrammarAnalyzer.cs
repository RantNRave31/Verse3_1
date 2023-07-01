using System.Collections.Generic;

namespace GKYU.TranslationLibrary.Grammars.Analyzers
{
    public class GrammarAnalyzer
    {
        public GrammarAnalyzer()
        {
            
        }
        public void ExecutePass(Syntax.SymbolTable symbolTable, Syntax.IVisitSyntax pass)
        {
            pass.Visit(symbolTable);
        }
        public static void Analyze(Syntax.SymbolTable symbolTable, IEnumerable<Syntax.IVisitSyntax> passes)
        {
            GrammarAnalyzer analyzer = new GrammarAnalyzer();
            foreach(Syntax.IVisitSyntax pass in passes)
            {
                analyzer.ExecutePass(symbolTable, pass);

            }
        }
    }
}
