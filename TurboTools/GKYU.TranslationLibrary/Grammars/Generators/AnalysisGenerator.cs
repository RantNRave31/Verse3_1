using System.Text;

namespace GKYU.TranslationLibrary.Grammars.Generators
{
    using GKYU.TranslationLibrary.Grammars.Analyzers;
    public class AnalysisGenerator
        : IVisitAnalysisPass
    {
        protected StringBuilder result = new StringBuilder();
        public StringBuilder Result
        {
            get
            {
                return result;
            }
        }
        public virtual void Visit(TotalsPass pass)
        {
            
        }

        public virtual void Visit(GrammarAnalysisPass pass)
        {

        }

        public virtual void Visit(GrammarAnalysisPass2 pass)
        {

        }

        public virtual void Visit(ValidationPass pass)
        {
            
        }
    }
}
