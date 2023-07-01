using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.TranslationLibrary.Grammars.Analyzers
{
    using GKYU.CollectionsLibrary.Collections.Graphs;
    using GKYU.TranslationLibrary.Grammars.Information;
    public class GrammarAnalysisPass2
        : AnalysisPass
    {
        public GrammarAnalysisInfo result;
        protected int changeCounter = 0;
        public GrammarAnalysisPass2(GrammarAnalysisInfo info = null)
        {
            result = (info == null) ? new GrammarAnalysisInfo() : info;
        }
        public void AddFIRSTs(int firstSymbolID, int symbolID)
        {
            if(!result.FIRST[symbolID].IsSubsetOf(result.FIRST[firstSymbolID]))
            {
                changeCounter++;
                result.FIRST[firstSymbolID].UnionWith(result.FIRST[symbolID]);
            }
        }
        protected void CalculateFIRSTSets(Syntax.SymbolTable symbolTable)
        {
            int counter = 0;
            do
            {
                counter++;
                changeCounter = 0;
                foreach (Syntax.SymbolDeclaration symbolDeclaration in symbolTable.symbolDeclarations.Values)
                {
                    foreach (int symbolID in result.startSymbols[symbolDeclaration.SymbolID])
                    {
                        AddFIRSTs(symbolDeclaration.SymbolID, symbolID);
                        if (!result.EPS[symbolID])
                            continue;
                        result.EPS[symbolDeclaration.SymbolID] = true;
                    }
                }

            } while (changeCounter != 0);
            Console.WriteLine("Calcuating FIRST Sets took {0} iterations", counter);
        }
        protected HashSet<int> stringFIRST(Syntax.SymbolDeclaration symbolDeclaration)
        {
            HashSet<int> result = new HashSet<int>();
            return result;
        }
        protected void CalculateFOLLOWSets(Syntax.SymbolTable symbolTable)
        {
            int counter = 0;
            foreach (Syntax.SymbolDeclaration symbolDeclaration in symbolTable.symbolDeclarations.Values)
            {
                result.FOLLOW.Add(symbolDeclaration.SymbolID, new HashSet<int>());
            }
            do
            {
                counter++;
                changeCounter = 0;
                foreach (Syntax.SymbolDeclaration symbolDeclaration in symbolTable.symbolDeclarations.Values)
                {
                    
                }

            } while (changeCounter != 0);
            Console.WriteLine("Calcuating FIRST Sets took {0} iterations", counter);
        }
        public override void Visit(Syntax.SymbolTable symbolTable)
        {
            CalculateFIRSTSets(symbolTable);
        }
        public override void Accept(IVisitAnalysisPass visitor)
        {
            visitor.Visit(this);
        }
    }
}
