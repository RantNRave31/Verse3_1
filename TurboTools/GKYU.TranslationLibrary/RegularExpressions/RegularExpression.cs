using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.TranslationLibrary.Grammars.RegularExpressions
{
    using GKYU.CollectionsLibrary.Collections.Graphs;


    /// <summary>
    /// A regular expression is a term, a '|' and another regular expression
    /// A term is a possibly empty sequence of factors
    /// A factor is a base followed by possibly empty sequence of '*'
    /// A base is a Character, an escaped Character, or a parenthesized regular expression
    /// </summary>
    public abstract class RegularExpression
    {
        public enum SYMBOL : int
        {
            EOF = -1,
            DEFAULT = 0,
            INITIAL,

            DOLLAR_SIGN = '$',
            LEFT_PARENTHESIS = '(',
            RIGHT_PARENTHESIS = ')',

            RESERVED = 65536,
            SOURCE,
            TARGET,
            EMPTY,
            EXIT,
            IDENTIFIER,

            MACRO,
            TODAY,

            FINAL,
        }
        public interface ISyntaxNodeVisitor
        {
            void Visit(RegularExpression.Empty empty);
            void Visit(RegularExpression.Choice choice);
            void Visit(RegularExpression.Sequence sequence);
            void Visit(RegularExpression.Repitition repetition);
            void Visit(RegularExpression.Primative primative);
            void Visit(RegularExpression.Declaration declaration);
        }
        public abstract class SyntaxNode
        {
            public int symbolTypeID { get; set; }
            public int symbolID { get; set; }

            public SyntaxNode()
            {

            }
            public bool Equals(SyntaxNode other)
            {
                return (this.symbolID == other.symbolID);
            }
            public abstract void Accept(ISyntaxNodeVisitor visitor);
            public static explicit operator Graph<int, int>(SyntaxNode expression)
            {
                RegularExpression.RegularExpression2NFA visitor = new RegularExpression.RegularExpression2NFA();
                expression.Accept(visitor);
                return visitor.graph;
            }

        }
        public class Empty
            : SyntaxNode
        {
            public override string ToString()
            {
                return "ϵ";
            }
            public override void Accept(ISyntaxNodeVisitor visitor)
            {
                visitor.Visit(this);
            }
        }
        public class Choice
            : SyntaxNode
        {
            public SyntaxNode thisOne;
            public SyntaxNode thatOne;
            public Choice(SyntaxNode thisOne, SyntaxNode thatOne)
            {
                this.thisOne = thisOne;
                this.thatOne = thatOne;
            }
            public override string ToString()
            {
                return "Choice";
            }
            public override void Accept(ISyntaxNodeVisitor visitor)
            {
                visitor.Visit(this);
            }
        }
        public class Sequence
            : SyntaxNode
        {
            public SyntaxNode first;
            public SyntaxNode second;
            public Sequence(SyntaxNode first, SyntaxNode second)
            {
                this.first = first;
                this.second = second;
            }
            public override string ToString()
            {
                return "Sequence";
            }
            public override void Accept(ISyntaxNodeVisitor visitor)
            {
                visitor.Visit(this);
            }
        }
        public class Repitition
            : SyntaxNode
        {
            public SyntaxNode data;
            public Repitition(SyntaxNode data)
            {
                this.data = data;
            }
            public override string ToString()
            {
                return "Repitition";
            }
            public override void Accept(ISyntaxNodeVisitor visitor)
            {
                visitor.Visit(this);
            }
        }
        public class Primative
            : SyntaxNode
        {
            public char data;
            public Primative(char data)
            {
                this.data = data;
            }
            public override string ToString()
            {
                return data.ToString();
            }
            public override void Accept(ISyntaxNodeVisitor visitor)
            {
                visitor.Visit(this);
            }
        }
        public class Declaration
            : SyntaxNode
        {
            public SyntaxNode syntaxNode;
            public Declaration(SyntaxNode expressionNode)
            {
                this.syntaxNode = expressionNode;
            }
            public override string ToString()
            {
                return syntaxNode.ToString();
            }
            public override void Accept(ISyntaxNodeVisitor visitor)
            {
                visitor.Visit(this);
            }
        }
        /// <summary>
        /// Uses Thompson's Construction to build an NFA graph from a Regular Expression
        /// </summary>
        public class RegularExpression2NFA
            : ISyntaxNodeVisitor
        {
            public Graph<int, int> graph = new Graph<int, int>();
            protected Stack<Graph<int, int>.Node> stack = new Stack<Graph<int, int>.Node>();
            public RegularExpression2NFA()
            {
                graph.edgeLabels.Add(0, "ϵ");
            }
            public void Visit(RegularExpression.Empty empty)
            {
                Graph<int, int>.Node result = graph.CreateNode();
                stack.Push(result);
                stack.Push(result);
            }
            public void Visit(RegularExpression.Choice choice)
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
            public void Visit(RegularExpression.Sequence sequence)
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
            public void Visit(RegularExpression.Repitition repitition)
            {
                Graph<int, int>.Node startNode;
                Graph<int, int>.Node dataNodeStart;
                Graph<int, int>.Node dataNodeEnd;
                Graph<int, int>.Node endNode;

                startNode = graph.CreateNode();
                endNode = graph.CreateNode();
                graph.AddEdge(startNode, endNode, 0);

                repitition.data.Accept(this);
                dataNodeEnd = stack.Pop();
                dataNodeStart = stack.Pop();
                graph.AddEdge(startNode, dataNodeStart, 0);
                graph.AddEdge(dataNodeEnd, endNode, 0);
                graph.AddEdge(dataNodeEnd, dataNodeStart, 0);

                stack.Push(startNode);
                stack.Push(endNode);
            }
            public void Visit(RegularExpression.Primative primative)
            {
                Graph<int, int>.Node startNode;
                Graph<int, int>.Node endNode;
                startNode = graph.CreateNode();
                endNode = graph.CreateNode();
                stack.Push(startNode);
                stack.Push(endNode);
                if (!graph.edgeLabels.ContainsKey(primative.data))
                {
                    graph.edgeLabels.Add(primative.data, string.Format("{0}", (char)primative.data));
                }
                graph.AddEdge(startNode, endNode, primative.data);
            }
            public void Visit(RegularExpression.Declaration declaration)
            {
                Graph<int, int>.Node initialNode;
                Graph<int, int>.Node finalNode;
                //initialNode = graph.CreateNode();
                declaration.syntaxNode.Accept(this);
                finalNode = stack.Pop();
                initialNode = stack.Pop();
                finalNode.Value = 1;// non zero means this is an accepting state
                //graph.AddEdge(initialNode, expressionNode);
            }

        }
        public const string grammar = @"
<regex>  ::= <term> '|' <regex> 
          | <regex>
<term>   ::= <factor>

<factor> ::= <atom> {'*'}

<atom>   ::= <Character> 
          | '\\' <Character> 
          | '(' <regex> ')'
";

        public static SyntaxNode Parse(string expressionText)
        {
            SyntaxNode result = null;
            using (StreamReader sr = new StreamReader(new MemoryStream(Encoding.ASCII.GetBytes(expressionText))))
            {
                Parser parser = new Parser(sr);
                result = parser.Parse();
            }
            return result;
        }
    }
}
