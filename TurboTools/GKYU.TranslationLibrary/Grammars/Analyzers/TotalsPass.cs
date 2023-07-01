using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.TranslationLibrary.Grammars.Analyzers
{
    public class GrammarStatisticsInfo
    {
        public int emptySymbolID;
        public HashSet<int> setCharacterDeclarationIDs = new HashSet<int>();
        public HashSet<int> setCharacterSetDeclarationIDs = new HashSet<int>();
        public HashSet<int> setTokenDeclarationIDs = new HashSet<int>();
        public HashSet<int> setSymbolDeclarationIDs = new HashSet<int>();
    }
    public class TotalsPass
        : AnalysisPass
    {
        public GrammarStatisticsInfo result;
        public TotalsPass(GrammarStatisticsInfo info = null)
            : base()
        {
            this.result = (info == null) ? new GrammarStatisticsInfo() : info;
        }

        public override void Accept(IVisitAnalysisPass visitor)
        {
            visitor.Visit(this);
        }

        public override void Visit(Syntax.Empty symbol)
        {
            result.emptySymbolID = symbol.SymbolID;
        }
        public override void Visit(Syntax.CharacterDeclaration symbol)
        {
            result.setCharacterDeclarationIDs.Add(symbol.SymbolID);
        }
        public override void Visit(Syntax.CharacterSetDeclaration symbol)
        {
            result.setCharacterSetDeclarationIDs.Add(symbol.SymbolID);
        }
        public override void Visit(Syntax.TokenDeclaration symbol)
        {
            result.setTokenDeclarationIDs.Add(symbol.SymbolID);
        }
        public override void Visit(Syntax.SymbolDeclaration symbol)
        {
            result.setSymbolDeclarationIDs.Add(symbol.SymbolID);
        }
        public override void Visit(Syntax.SymbolTable symbolTable)
        {
            foreach (Syntax.CharacterDeclaration CharacterDeclaration in symbolTable.CharacterDeclarations.Values)
            {
                CharacterDeclaration.Accept(this);
            }
            foreach (Syntax.CharacterSetDeclaration CharacterSetDeclaration in symbolTable.CharacterSetDeclarations.Values)
            {
                CharacterSetDeclaration.Accept(this);
            }
            foreach (Syntax.TokenDeclaration tokenDeclaration in symbolTable.tokenDeclarations.Values)
            {
                tokenDeclaration.Accept(this);
            }
            foreach (Syntax.SymbolDeclaration symbolDeclaration in symbolTable.symbolDeclarations.Values)
            {
                symbolDeclaration.Accept(this);
            }
        }
    }
}
