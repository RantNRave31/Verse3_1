namespace GKYU.TranslationLibrary.Grammars.Analyzers
{
    public interface IVisitAnalysisPass
    {
        void Visit(TotalsPass pass);
        void Visit(GrammarAnalysisPass pass);
        void Visit(GrammarAnalysisPass2 pass);
        void Visit(ValidationPass pass);
    }
    public abstract class AnalysisPass
        : Syntax.Visitor
    {
        
    }
}
