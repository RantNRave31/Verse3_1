using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.TranslationLibrary.Grammars.Analyzers
{
    using GKYU.CollectionsLibrary.Collections;
    using GKYU.CollectionsLibrary.Collections.Graphs;
    using GKYU.TranslationLibrary.Grammars.Information;

    public class GrammarAnalysisPass
        : AnalysisPass
    {
        public GrammarAnalysisInfo result;
        public GrammarAnalysisPass(GrammarAnalysisInfo info = null)
        {
            result = (info == null) ? new GrammarAnalysisInfo() : info;
        }
        public override void Visit(Syntax.CharacterDeclaration symbol)
        {
            result.Recognize(symbol);
            result.EPS.Add(symbol.SymbolID, false);// RULE:  for each terminal, EPS[symbolID] = false
            result.FIRST.Add(symbol.SymbolID, new HashSet<int>(new int[] { symbol.SymbolID }));// RULE: for each terminal, FIRST[c] = { c }
        }
        public override void Visit(Syntax.CharacterSetDeclaration symbol)
        {
            result.Recognize(symbol);
            result.EPS.Add(symbol.SymbolID, false);// RULE:  for each terminal, EPS[symbolID] = false
            result.FIRST.Add(symbol.SymbolID, new HashSet<int>(new int[] { symbol.SymbolID }));// RULE: for each terminal, FIRST[c] = { c }
        }
        protected bool IsTokenTerminal(Syntax.SyntaxNode node)
        {
            if (node.GetType() == typeof(Syntax.Empty)
                || node.GetType() == typeof(Syntax.CharacterDeclaration)
                || node.GetType() == typeof(Syntax.CharacterSetDeclaration)
                )
                return true;
            else
                return false;
        }
        protected bool IsSymbolTerminal(Syntax.SyntaxNode node)
        {
            if (node.GetType() == typeof(Syntax.Empty)
                || node.GetType() == typeof(Syntax.CharacterDeclaration)
                || node.GetType() == typeof(Syntax.CharacterSetDeclaration)
                || node.GetType() == typeof(Syntax.TokenDeclaration)
                )
                return true;
            else
                return false;
        }
        public override void Visit(Syntax.TokenDeclaration symbol)
        {
            Graph<int, int> nfa = (Graph<int, int>)symbol;
            Graph<int, int> dfa = nfa.ToDFA();
            if (dfa != null)
            {
                foreach(Edge<int,int> edge in ((Graph<int,int>.Node)dfa.Verticies[0]).Edges)
                {
                    Syntax.Primative p = (Syntax.Primative)symbol.SymbolTable[edge.Weight];
                    if (!IsTokenTerminal((Syntax.SyntaxNode)symbol.SymbolTable[p.referenceID]))
                        throw new Exception("LL(k) ERROR:  Token Start Terminals may only reference Token Non-Terminals (Character or CharacterSet)");
                    result.AddStartSymbol(symbol.SymbolID, p.referenceID);
                }
                foreach (Edge<int, int> edge in dfa.Edges)
                {
                    Syntax.Primative p = (Syntax.Primative)symbol.SymbolTable[edge.Weight];
                    if (!IsTokenTerminal((Syntax.SyntaxNode)symbol.SymbolTable[p.referenceID]))
                        throw new Exception("LL(k) ERROR:  Token Recognized Terminals may only reference Token Non-Terminals (Character or CharacterSet)");
                    result.AddRecognizedSymbol(symbol.SymbolID, p.referenceID);
                }
            }
            result.EPS.Add(symbol.SymbolID, false);// RULE:  for each terminal, EPS[symbolID] = false
            result.FIRST.Add(symbol.SymbolID, new HashSet<int>(new int[] { symbol.SymbolID }));// RULE: for each terminal, FIRST[c] = { c }
        }
        public override void Visit(Syntax.SymbolDeclaration symbol)
        {
            Graph<int, int> nfa = (Graph<int, int>)symbol;
            Graph<int, int> dfa = nfa.ToDFA();
            foreach (Edge<int, int> edge in ((Graph<int, int>.Node)dfa.Verticies[0]).Edges)
            {
                Syntax.Primative p = (Syntax.Primative)symbol.SymbolTable[edge.Weight];
                //if (!IsSymbolTerminal(symbol.symbolTable[p.referenceID]))
                //    throw new Exception("LL(k) ERROR:  Symbol Start Symbols may only reference Symbol Non-Terminals (Character, CharacterSet, or TokenDeclaration)");
                result.AddStartSymbol(symbol.SymbolID, p.referenceID);
            }
            foreach (Edge<int, int> edge in dfa.Edges)
            {
                Syntax.Primative p = (Syntax.Primative)symbol.SymbolTable[edge.Weight];
                //if (!IsSymbolTerminal(symbol.symbolTable[p.referenceID]))
                //    throw new Exception("LL(k) ERROR:  Symbol Recognized Symbols may only reference Symbol Non-Terminals (Character, CharacterSet, or TokenDeclaration)");
                result.AddRecognizedSymbol(symbol.SymbolID, p.referenceID);
            }
            if (result.recognizedSymbolsSet[symbol.SymbolID].Count == 0)// RULE:  if production can yield empty, then true, else false;
                result.EPS.Add(symbol.SymbolID, true);
            else
                result.EPS.Add(symbol.SymbolID, false);
            result.FIRST.Add(symbol.SymbolID, new HashSet<int>());// RULE: for each non terminal, FIRST[c] = { }
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

        public override void Accept(IVisitAnalysisPass visitor)
        {
            visitor.Visit(this);
        }
    }
}
