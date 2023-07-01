using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.TranslationLibrary.Grammars
{
    using GKYU.CollectionsLibrary.Collections;
    using GKYU.CollectionsLibrary.Collections.Graphs;
    using static GKYU.TranslationLibrary.Grammars.RegularExpressions.RegularExpression;

    public class LexerSwitchGenerator
    {
        protected static Dictionary<int, List<string>> semanticActionsMap = new Dictionary<int, List<string>>()
        {
            { 0, new List<string>(){
                "token = new Token(TOKEN.NoMatch);",
                "token.position = position;",
            } },
        };
        protected static string CharacterString(int characterCode)
        {
            if (characterCode == -1)
                return "-1";
            if (characterCode >= 1 && characterCode <= 31)
                return characterCode.ToString();
            if (characterCode == '\'')
                return "'\\''";
            else
                return "'" + ((char)characterCode).ToString() + "'";

        }
        public static string Conditions(CharacterSet characterSet)
        {
            string conditions = string.Empty;
            CharacterSet.Range currentRange = characterSet.head;
            while (null != currentRange)
            {
                if (conditions.Length > 0)
                    conditions += " || ";
                conditions += string.Format("(({0} <= currentInput) && (currentInput <= {1}))", CharacterString(currentRange.from), CharacterString(currentRange.to));
                currentRange = currentRange.next;
            }
            return conditions;
        }
        public static string CharacterAction(Syntax.CharacterDeclaration CharacterDeclaration, int targetStateID, bool bMatchState)
        {
            if(CharacterDeclaration.SymbolID == 0)
            {
                if (bMatchState)
                    return "break;";
                else
                    return "goto case " + targetStateID.ToString() + ";";
            }
            return "if ( currentInput == " + CharacterString(CharacterDeclaration.CharacterCode) + ") goto case " + targetStateID.ToString() + ";";
        }
        public static string CharacterSetAction(CharacterSet characterSet, int targetStateID, bool bMatchState)
        {
            string conditions = Conditions(characterSet);
            if (conditions.Length > 0)
                return "if (" + Conditions(characterSet) + ") goto case " + targetStateID.ToString() + ";";
            else
            {
                if (bMatchState)
                    return "break;";
                else
                    return "goto case " + targetStateID.ToString() + ";";
            }
        }
        public static string ActionStatement(Syntax.SymbolTable symbolTable, Edge<int,int> edge, bool bMatchState)
        {
            string result = string.Empty;
            int symbolID = 0;
            switch (edge.Weight)
            {
                case -1:
                    symbolID = symbolTable["eof"].SymbolID;
                    break;
                case 0:
                    symbolID = 0;
                    break;
                default:
                    Syntax.Primative primative = (Syntax.Primative)symbolTable[edge.Weight];
                    symbolID = primative.referenceID;
                    break;
            }
            switch (symbolTable[symbolID])
            {
                case Syntax.CharacterDeclaration cd:
                    result = CharacterAction(cd, edge.Target.nodeID, bMatchState);
                    break;
                case Syntax.CharacterSetDeclaration csd:
                    result = CharacterSetAction(csd.CharacterSet, edge.Target.nodeID, bMatchState);
                    break;
                default:
                    //throw new Exception("Unhandled Action for Primative");
                    break;
            }
            return result;
        }
        public static List<string> CaseStatement(Syntax.SymbolTable symbolTable, Graph<int,int>.Node stateNode)
        {
            List<string> result = new List<string>();
            List<string> semanticActions = new List<string>();
            List<string> actionStatements = new List<string>();
            string defaultStatement = string.Empty;
            string actionStatement;
            result.Add(string.Format("case {0}:", stateNode.nodeID.ToString()));
            if(semanticActionsMap.ContainsKey(stateNode.nodeID))
            {
                semanticActions.AddRange(semanticActionsMap[stateNode.nodeID]);
            }
            if (stateNode.Value != 0)
            {
                if (symbolTable[stateNode.Value] is Syntax.TypeDeclaration)
                {
                    semanticActions.Add(string.Format("    token.kind = TOKEN.{0};", ((Syntax.TypeDeclaration)symbolTable[stateNode.Value]).identifier.Name));
                    //semanticActions.Add("NextInput();");
                }
                else
                {
                    semanticActions.Add("// oh shit, not a type declaration");
                }
            }
            else
            {
                semanticActions.Add("    token.value += (char)currentInput;");
                semanticActions.Add("    NextInput();");
            }
            foreach (Edge<int,int> edge in stateNode.Edges)
            {
                switch(edge.Weight)
                {
                    case 0:
                        actionStatement = ActionStatement(symbolTable, edge, (stateNode.Value != 0));
                        if (actionStatement.Length > 0)
                        {
                            defaultStatement = actionStatement;
                        }
                        break;
                    default:
                        actionStatement = ActionStatement(symbolTable, edge, (stateNode.Value != 0));
                        if (actionStatement.Length > 0)
                        {
                            if (actionStatements.Count > 0)
                                actionStatement = "    else " + actionStatement;
                            else
                                actionStatement = "    " + actionStatement;
                            actionStatements.Add(actionStatement);
                        }
                        break;
                }
            }
            if (semanticActions.Count > 0)
                result.AddRange(semanticActions);
            if (actionStatements.Count > 0)
                result.AddRange(actionStatements);
            if (defaultStatement.Length == 0)
            {
                if (actionStatements.Count > 0)
                    result.Add("    else break;");
                else
                    result.Add("    break;");
            }
            else
            {
                if (actionStatements.Count > 0)
                    result.Add("    else " + defaultStatement);
                else
                    result.Add("    " + defaultStatement);
            }
            return result;
        }
        public static List<string> SwitchStatement(Syntax.SymbolTable symbolTable, Graph<int,int> fsm)
        {
            List<string> result = new List<string>();
            result.Add(string.Format("switch(currentState)"));
            result.Add("{");
            foreach (Graph<int,int>.Node stateNode in fsm.Verticies)
            {
                result.AddRange(CaseStatement(symbolTable, stateNode));
            }
            result.Add("}");
            return result;
        }
        public static List<string> Generate(Syntax.LexerDeclaration lexerDeclaration)
        {
            List<string> result = new List<string>();
            Graph<int, int> nfa;
            Graph<int, int> dfa;
            Graph<int, int> fsm;

            nfa = (Graph<int, int>)lexerDeclaration;
            nfa.WriteToGraphViz("C:\\Work\\NFA.dot", new string[] { "rankdir=LR" });
            dfa = nfa.ToDFA();
            dfa.WriteToGraphViz("C:\\Work\\DFA.dot", new string[] { "rankdir=LR" });
            fsm = dfa.ToFSM();
            fsm.WriteToGraphViz("C:\\Work\\FSM.dot", new string[] { "rankdir=LR" });

            result.AddRange(SwitchStatement((Syntax.SymbolTable)lexerDeclaration.SymbolTable, fsm));

            return result;
        }
    }
}
