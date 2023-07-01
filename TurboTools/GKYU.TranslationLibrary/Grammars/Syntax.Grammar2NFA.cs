using System;
using System.Collections.Generic;

namespace GKYU.TranslationLibrary.Grammars
{
    using GKYU.CollectionsLibrary.Collections.Graphs;
    using GKYU.TranslationLibrary.Grammars.Analyzers;

    public partial class Syntax
    {
        public class Grammar2NFA
            : Visitor
        {
            public Graph<int, int> graph = new Graph<int, int>();
            protected Stack<Graph<int, int>.Node> stack = new Stack<Graph<int, int>.Node>();
            public Grammar2NFA()
            {
                graph.edgeLabels.Add(0, "ϵ");
            }

            public override void Accept(IVisitAnalysisPass visitor)
            {
                throw new NotImplementedException();
            }

            public override void Visit(Empty symbol)
            {
                Graph<int, int>.Node result = graph.CreateNode();
                stack.Push(result);
                stack.Push(result);
            }
            public override void Visit(Identifier symbol)
            {

            }
            public override void Visit(Number symbol)
            {

            }
            public override void Visit(Integer symbol)
            {

            }
            public override void Visit(Decimal symbol)
            {

            }
            public override void Visit(String symbol)
            {

            }
            public override void Visit(Variable symbol)
            {

            }
            public override void Visit(Reference symbol)
            {

            }
            public override void Visit(SyntaxNode symbol)
            {

            }
            public override void Visit(Choice choice)
            {
                Graph<int, int>.Node startNode;
                Graph<int, int>.Node thisNodeStart;
                Graph<int, int>.Node thisNodeEnd;
                Graph<int, int>.Node thatNodeStart;
                Graph<int, int>.Node thatNodeEnd;
                Graph<int, int>.Node endNode;

                startNode = graph.CreateNode();
                endNode = graph.CreateNode();

                choice.thisOne.Accept(this);
                thisNodeEnd = stack.Pop();
                thisNodeStart = stack.Pop();
                graph.AddEdge(startNode, thisNodeStart, 0);
                graph.AddEdge(thisNodeEnd, endNode, 0);

                choice.thatOne.Accept(this);
                thatNodeEnd = stack.Pop();
                thatNodeStart = stack.Pop();
                graph.AddEdge(startNode, thatNodeStart, 0);
                graph.AddEdge(thatNodeEnd, endNode, 0);

                stack.Push(startNode);
                stack.Push(endNode);
            }
            public override void Visit(Sequence sequence)
            {
                Graph<int, int>.Node firstStartNode;
                Graph<int, int>.Node firstEndNode;
                Graph<int, int>.Node secondStartNode;
                Graph<int, int>.Node secondEndNode;

                sequence.first.Accept(this);
                firstEndNode = stack.Pop();
                firstStartNode = stack.Pop();

                sequence.second.Accept(this);
                secondEndNode = stack.Pop();
                secondStartNode = stack.Pop();

                graph.AddEdge(firstEndNode, secondStartNode, 0);
                stack.Push(firstStartNode);
                stack.Push(secondEndNode);
            }
            public override void Visit(Repitition repitition)
            {
                Graph<int, int>.Node startNode;
                Graph<int, int>.Node dataNodeStart;
                Graph<int, int>.Node dataNodeEnd;
                Graph<int, int>.Node endNode;

                startNode = graph.CreateNode();
                endNode = graph.CreateNode();
                graph.AddEdge(startNode, endNode, 0);

                repitition.expression.Accept(this);
                dataNodeEnd = stack.Pop();
                dataNodeStart = stack.Pop();
                graph.AddEdge(startNode, dataNodeStart, 0);
                graph.AddEdge(dataNodeEnd, endNode, 0);
                graph.AddEdge(dataNodeEnd, dataNodeStart, 0);

                stack.Push(startNode);
                stack.Push(endNode);
            }
            public override void Visit(Primative reference)
            {
                Graph<int, int>.Node startNode;
                Graph<int, int>.Node endNode;
                startNode = graph.CreateNode();
                endNode = graph.CreateNode();
                stack.Push(startNode);
                stack.Push(endNode);
                string name = reference.SymbolID.ToString();
                if (!graph.edgeLabels.ContainsKey(reference.referenceID))
                {
                    TypeDeclaration t = (TypeDeclaration)reference.SymbolTable[reference.referenceID];
                    if (t.identifier.Name == "\"")
                        Console.WriteLine(t.identifier.Name);
                    graph.edgeLabels.Add(reference.SymbolID, t.identifier.Name);
                }
                graph.AddEdge(startNode, endNode, reference.SymbolID);
            }
            public override void Visit(TypeDeclaration declaration)
            {
                
            }
            public override void Visit(MacroExpression symbol)
            {
                this.Visit(symbol.expression);
            }
            public override void Visit(CharacterDeclaration symbol)
            {
                Graph<int, int>.Node result = graph.CreateNode();
            }
            public override void Visit(CharacterSetDeclaration symbol)
            {
                Graph<int, int>.Node result = graph.CreateNode();
            }
            public override void Visit(TokenDeclaration tokenDeclaration)
            {
                if (tokenDeclaration.expression != null) tokenDeclaration.expression.Accept(this);
            }
            public override void Visit(LexerDeclaration lexerDeclaration)
            {
                Graph<int, int>.Node initialNode;
                Graph<int, int>.Node finalNode;
                Graph<int, int>.Node startNode;
                Graph<int, int>.Node endNode;

                initialNode = graph.CreateNode();
                finalNode = graph.CreateNode();
                finalNode.Value = lexerDeclaration.SymbolID;// non zero means this is an accepting state for this grammar/language
                foreach (TokenDeclaration symbolDeclaration in ((Syntax.SymbolTable)lexerDeclaration.SymbolTable).tokenDeclarations.Values)
                {
                    if (symbolDeclaration.expression == null)
                        continue;
                    symbolDeclaration.Accept(this);
                    endNode = stack.Pop();
                    startNode = stack.Pop();

                    graph.AddEdge(initialNode, startNode, 0);
                    graph.AddEdge(endNode, finalNode, 0);

                    endNode.Value = symbolDeclaration.SymbolID;// non zero means this is an accepting state for this symbol
                }
            }
            public override void Visit(SymbolDeclaration symbolDeclaration)
            {
                Graph<int, int>.Node initialNode;
                Graph<int, int>.Node finalNode;
                symbolDeclaration.expression.Accept(this);
            }
            public override void Visit(ParserDeclaration parserDeclaration)
            {
                Graph<int, int>.Node initialNode;
                Graph<int, int>.Node finalNode;
                Graph<int, int>.Node startNode;
                Graph<int, int>.Node endNode;

                initialNode = graph.CreateNode();
                finalNode = graph.CreateNode();
                finalNode.Value = parserDeclaration.SymbolID;// non zero means this is an accepting state for this grammar/language
                foreach (SymbolDeclaration symbolDeclaration in ((Syntax.SymbolTable)parserDeclaration.SymbolTable).symbolDeclarations.Values)
                {
                    if (symbolDeclaration.expression == null)
                        continue;
                    symbolDeclaration.Accept(this);
                    endNode = stack.Pop();
                    startNode = stack.Pop();

                    graph.AddEdge(initialNode, startNode, 0);
                    graph.AddEdge(endNode, finalNode, 0);

                    endNode.Value = symbolDeclaration.SymbolID;// non zero means this is an accepting state for this symbol
                }
            }
            public override void Visit(SymbolTable symbolTable)
            {
                throw new NotImplementedException();
            }
        }

    }
}
