using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.TranslationLibrary.Grammars.Information
{
    using GKYU.CollectionsLibrary.Collections;
    public class GrammarAnalysisInfo
    {
        public Dictionary<int, HashSet<int>> startSymbols = new Dictionary<int, HashSet<int>>();
        public Dictionary<int, HashSet<int>> recognizedSymbolsSet = new Dictionary<int, HashSet<int>>();

        public Dictionary<int, bool> EPS = new Dictionary<int, bool>();
        public Dictionary<int, HashSet<int>> FIRST = new Dictionary<int, HashSet<int>>();
        public Dictionary<int, HashSet<int>> FOLLOW = new Dictionary<int, HashSet<int>>();

        public void AddStartSymbol(int targetSymbolID, int symbolID)
        {
            if (!startSymbols.ContainsKey(targetSymbolID))
                startSymbols.Add(targetSymbolID, new HashSet<int>());
            startSymbols[targetSymbolID].Add(symbolID);

        }
        public void AddRecognizedSymbol(int targetSymbolID, int symbolID)
        {
            if (!recognizedSymbolsSet.ContainsKey(targetSymbolID))
                recognizedSymbolsSet.Add(targetSymbolID, new HashSet<int>());
            recognizedSymbolsSet[targetSymbolID].Add(symbolID);

        }
        public void Recognize(Syntax.CharacterDeclaration characterDeclaration)
        {
            if (!recognizedSymbolsSet.ContainsKey(characterDeclaration.SymbolID))
                recognizedSymbolsSet.Add(characterDeclaration.SymbolID, new HashSet<int>());
            recognizedSymbolsSet[characterDeclaration.SymbolID].Add(characterDeclaration.CharacterCode);
        }
        public void Recognize(Syntax.CharacterSetDeclaration characterSetDeclaration)
        {
            if (!recognizedSymbolsSet.ContainsKey(characterSetDeclaration.SymbolID))
                recognizedSymbolsSet.Add(characterSetDeclaration.SymbolID, new HashSet<int>());
            CharacterSet.Range currentRange = characterSetDeclaration.CharacterSet.head;
            while (currentRange != null)
            {
                for (int n = currentRange.from; n <= currentRange.to; n++)
                {
                    recognizedSymbolsSet[characterSetDeclaration.SymbolID].Add(n);
                }
                currentRange = currentRange.next;
            }

        }
    }
}
