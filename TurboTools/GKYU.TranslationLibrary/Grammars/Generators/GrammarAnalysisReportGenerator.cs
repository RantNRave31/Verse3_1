using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.TranslationLibrary.Grammars.Generators
{
    using GKYU.TranslationLibrary.Grammars.Analyzers;
    using GKYU.TranslationLibrary.Grammars.Information;

    public class GrammarAnalysisReportGenerator
        : AnalysisGenerator
    {
        protected Syntax.SymbolTable symbolTable;
        public GrammarAnalysisReportGenerator(Syntax.SymbolTable symbolTable)
        {
            this.symbolTable = symbolTable;
        }
        public void GrammarSetInfo(GrammarAnalysisInfo result)
        {
            StringBuilder sb;
            foreach(int n in result.recognizedSymbolsSet.Keys)
            {
                switch(symbolTable[n])
                {
                    case Syntax.CharacterDeclaration cd:
                        break;
                    case Syntax.CharacterSetDeclaration csd:
                        break;
                    case Syntax.TokenDeclaration td:
                        this.result.AppendLine(string.Format("{0}", symbolTable[n]));
                        sb = new StringBuilder();
                        foreach(int s in result.startSymbols[n])
                        {
                            sb.Append(string.Format("{0},", symbolTable[s]));
                        }
                        if (sb.Length > 0) sb.Length--;
                        this.result.AppendLine("    Starts With:  " + sb.ToString());
                        sb = new StringBuilder();
                        foreach (int m in result.recognizedSymbolsSet[n])
                        {
                            sb.Append(string.Format("{0},", symbolTable[m]));
                        }
                        if (sb.Length > 0) sb.Length--;
                        this.result.AppendLine("    Recognizes:  " + sb.ToString());
                        sb = new StringBuilder();
                        foreach (int m in result.FIRST[n])
                        {
                            sb.Append(string.Format("{0},", symbolTable[m]));
                        }
                        if (sb.Length > 0) sb.Length--;
                        this.result.AppendLine("    FIRST:  " + sb.ToString());
                        break;
                    case Syntax.SymbolDeclaration sd:
                        this.result.AppendLine(string.Format("{0}", symbolTable[n]));
                        sb = new StringBuilder();
                        foreach (int s in result.startSymbols[n])
                        {
                            sb.Append(string.Format("{0},", symbolTable[s]));
                        }
                        if (sb.Length > 0) sb.Length--;
                        this.result.AppendLine("    Starts With:  " + sb.ToString());
                        sb = new StringBuilder();
                        foreach (int m in result.recognizedSymbolsSet[n])
                        {
                            sb.Append(string.Format("{0},", symbolTable[m]));
                        }
                        if (sb.Length > 0) sb.Length--;
                        this.result.AppendLine("    Recognizes:  " + sb.ToString());
                        sb = new StringBuilder();
                        foreach (int m in result.FIRST[n])
                        {
                            sb.Append(string.Format("{0},", symbolTable[m]));
                        }
                        if (sb.Length > 0) sb.Length--;
                        this.result.AppendLine("    FIRST:  " + sb.ToString());
                        break;
                    default:
                        break;
                }
            }
        }
        public override void Visit(GrammarAnalysisPass pass)
        {
            GrammarSetInfo(pass.result);
        }
    }
}
