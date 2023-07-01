using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.CollectionsLibrary.Collections.Graphs
{
    /// <summary>
    /// NFA and DFA graph notes:
    /// NFA and DFA graphs must be of value type int and edge weight type int.
    /// An NFA pr DFA graph's nodes represent states whose value should be the identity of the entity the state accepts.
    ///     a node with a non-zero value indicate an accept state
    ///     a node with a value of zero indicates a non-accepting state.
    /// An NFA or DFA graphs edges represent transitions whose weight value should be the identity of the input enitty to transit from one node to another.
    /// NFA graph's edges may contain epsilon transitions (i.e. with a weight value of zero, indicating the transition does not consume input)
    /// DFA graph's edges may NOT contain epsion transitions
    /// </summary>
    public static class GraphExtensions
    {
        /// <summary>
        /// Used to convert an NFA graph to a DFA graph
        /// </summary>
        /// <param name="nfaNodes"></param>
        /// <returns>the set of all states or nodes reachable from any state in nfaNodes in 1 or more ε-transitions</returns>
        public static HashSet<Graph<int, int>.Node> EpsilonClosure(HashSet<Graph<int, int>.Node> nfaNodes)
        {
            HashSet<Graph<int, int>.Node> result = nfaNodes;
            Stack<Graph<int, int>.Node> stack = new Stack<Graph<int, int>.Node>();
            foreach (Graph<int, int>.Node nfaNode in nfaNodes)
            {
                stack.Push(nfaNode);
            }
            while (stack.Count > 0)
            {
                Graph<int, int>.Node t = stack.Pop();
                foreach (Edge<int, int> edge in t.Edges)
                {
                    if (edge.Weight != 0)
                        continue;
                    if (!result.Contains(edge.Target))
                    {
                        result.Add((Graph<int, int>.Node)edge.Target);
                        stack.Push((Graph<int, int>.Node)edge.Target);
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// Used to convert an NFA graph to a DFA graph
        /// </summary>
        /// <param name="input">the input (usually a Character)</param>
        /// <param name="eplisonClosure">the set of all ε-closure states under consideration</param>
        /// <returns>the set of all states or nodes reachable from any state in nfaNodes on a given input</returns>
        public static HashSet<Graph<int, int>.Node> Move(int input, HashSet<Graph<int, int>.Node> eplisonClosure)
        {
            HashSet<Graph<int, int>.Node> result = new HashSet<Graph<int, int>.Node>();
            foreach (Graph<int, int>.Node nfaNode in eplisonClosure)
            {
                foreach (Edge<int, int> edge in nfaNode.Edges)
                {
                    if (edge.Weight == input)
                        result.Add((Graph<int, int>.Node)edge.Target);
                }
            }
            return result;
        }
        /// <summary>
        /// AcceptStates is used on an NFA or DFA graph.
        /// AcceptStates returns the set of all nodes(states) in a graph whose value is non zero
        /// An Accept State's (or node's) value should be the identity of the entity it matches.
        /// Accepting states of a DFA are all states which contain at least one accepting states of an NFA
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static HashSet<Graph<int, int>.Node> AcceptStates(this Graph<int, int> graph)
        {
            HashSet<Graph<int, int>.Node> result = new HashSet<Graph<int, int>.Node>();
            foreach (Graph<int, int>.Node vertex in graph.Verticies)
            {
                if (vertex.Value != 0)
                {
                    result.Add(vertex);
                }
            }
            return result;
        }
        /// <summary>
        /// Creates a new DFA Node that represents a set of one or more NFA states
        /// </summary>
        /// <param name="dfa">the target DFA graph</param>
        /// <param name="set">a set of source NFA graph nodes</param>
        /// <returns></returns>
        public static Graph<int, int>.Node CreateDFANode(this Graph<int, int> dfa, HashSet<Graph<int, int>.Node> set)
        {
            Graph<int, int>.Node newDFANode = dfa.CreateNode();
            foreach (Graph<int, int>.Node node in set)// are one of the states a Match state?  then it is an accepting state unless it's the final state
            {
                if (node.Value != 0 && node.Value != 2)
                {
                    newDFANode.Value = node.Value;
                }
            }
            return newDFANode;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nfa">the source NFA graph</param>
        /// <returns>a DFA graph with NO ε-transitions</returns>
        public static Graph<int, int> ToDFA(this Graph<int, int> nfa)
        {
            Graph<int, int> resultGraph = null;
            List<HashSet<Graph<int, int>.Node>> setStates;
            List<Graph<int, int>.Node> setAcceptStates;
            Dictionary<HashSet<Graph<int, int>.Node>, Graph<int, int>.Node> dfaStates;
            if (nfa.Verticies.Count > 0)
            {
                HashSet<int> symbolSet = new HashSet<int>();
                resultGraph = new Graph<int, int>();
                setStates = new List<HashSet<Graph<int, int>.Node>>();
                setAcceptStates = new List<Graph<int, int>.Node>();
                dfaStates = new Dictionary<HashSet<Graph<int, int>.Node>, Graph<int, int>.Node>(HashSet<Graph<int, int>.Node>.CreateSetComparer());
                resultGraph.edgeLabels = nfa.edgeLabels;
                // Build list of non-epsilon input symbols
                foreach (Edge<int, int> edge in nfa.Edges)
                {
                    if (edge.Weight != 0 && !symbolSet.Contains(edge.Weight))
                        symbolSet.Add(edge.Weight);
                }
                // Create new DFA Node:  start DFA Node is the Epsilon Closure of the start NFA Node
                HashSet<Graph<int, int>.Node> startNodes = new HashSet<Graph<int, int>.Node>();
                startNodes.Add((Graph<int, int>.Node)nfa.Verticies[0]);
                HashSet<Graph<int, int>.Node> epsilonClosureSet;
                Graph<int, int>.Node startDFANode = resultGraph.CreateDFANode(epsilonClosureSet = EpsilonClosure(startNodes));
                setStates.Add(epsilonClosureSet);
                dfaStates.Add(epsilonClosureSet, startDFANode);
                if (startDFANode.Value != 0)
                    setAcceptStates.Add(startDFANode);
                // Add start DFA Node to the list of unprocessed DFA Nodes
                Stack<Graph<int, int>.Node> unprocessedNodes = new Stack<Graph<int, int>.Node>();
                unprocessedNodes.Push(startDFANode);
                // Build DFA
                while (unprocessedNodes.Count > 0)
                {
                    Graph<int, int>.Node processingDFANode = unprocessedNodes.Pop();
                    foreach (int inputSymbol in symbolSet)
                    {
                        HashSet<Graph<int, int>.Node> moveResult = Move(inputSymbol, setStates[processingDFANode.nodeID]);
                        epsilonClosureSet = EpsilonClosure(moveResult);
                        if (epsilonClosureSet.Count <= 0)
                            continue;
                        if (!dfaStates.ContainsKey(epsilonClosureSet))
                        {
                            Graph<int, int>.Node U = resultGraph.CreateDFANode(epsilonClosureSet);
                            setStates.Add(epsilonClosureSet);
                            dfaStates.Add(epsilonClosureSet, U);
                            if (U.Value != 0)
                                setAcceptStates.Add(U);
                            unprocessedNodes.Push(U);
                            resultGraph.AddEdge(processingDFANode, U, inputSymbol);
                        }
                        else
                        {
                            resultGraph.AddEdge(processingDFANode, dfaStates[epsilonClosureSet], inputSymbol);
                        }
                    }
                }
            }
            return resultGraph;
        }
        public static Graph<int, int> ToFSM(this Graph<int, int> dfa)
        {
            Graph<int, int> resultGraph = null;
            Dictionary<int, int> mapDFA2FSM;
            HashSet<Graph<int, int>.Node> setAcceptStates;
            Dictionary<int, Graph<int, int>.Node> mapMatchStates;
            if (dfa.Verticies.Count > 0)
            {
                resultGraph = new Graph<int, int>();
                mapDFA2FSM = new Dictionary<int, int>();
                setAcceptStates = dfa.AcceptStates();
                mapMatchStates = new Dictionary<int, Graph<int, int>.Node>();
                // Load Initial and NoMatch states
                Graph<int, int>.Node initialState = resultGraph.CreateNode(0);// Load start state
                mapDFA2FSM.Add(0, 0);
                Graph<int, int>.Node noMatchState = resultGraph.CreateNode(1);// load no match state
                // Load all dfaNodes as stateNodes except dfa Start Node
                for (int nodeIndex = 1; nodeIndex < dfa.Verticies.Count; nodeIndex++)
                {
                    Graph<int, int>.Node dfaNode = (Graph<int, int>.Node)dfa.Verticies[nodeIndex];
                    Graph<int, int>.Node stateNode = resultGraph.CreateNode(0);
                    mapDFA2FSM.Add(dfaNode.nodeID, stateNode.nodeID);
                }
                // Load all matchStates
                foreach (Graph<int, int>.Node acceptState in setAcceptStates)
                {
                    if (!mapMatchStates.ContainsKey(acceptState.Value))
                    {
                        Graph<int, int>.Node stateNode = resultGraph.CreateNode(acceptState.Value);
                        mapMatchStates.Add(acceptState.Value, stateNode);
                        resultGraph.AddEdge(stateNode, initialState, 0);
                    }
                }
                // map transitions
                resultGraph.AddEdge(initialState, noMatchState, 0);
                resultGraph.AddEdge(noMatchState, initialState, 0);
                foreach (Graph<int, int>.Node dfaNode in dfa.Verticies)
                {
                    Graph<int, int>.Node fsaNode = (Graph<int, int>.Node)resultGraph.Verticies[mapDFA2FSM[dfaNode.nodeID]];
                    if (dfaNode.Value != 0) // Match State
                    {
                        Graph<int, int>.Node matchState = mapMatchStates[dfaNode.Value];
                        resultGraph.AddEdge(fsaNode, matchState, 0);
                    }
                    else
                    {
                        resultGraph.AddEdge(fsaNode, noMatchState, 0);
                    }
                    foreach (Edge<int, int> edge in dfaNode.Edges)
                    {
                        Graph<int, int>.Node fromState = (Graph<int, int>.Node)resultGraph.Verticies[mapDFA2FSM[edge.Source.nodeID]];
                        Graph<int, int>.Node toState = (Graph<int, int>.Node)resultGraph.Verticies[mapDFA2FSM[edge.Target.nodeID]];
                        resultGraph.AddEdge(fromState, toState, edge.Weight);
                    }
                }
            }
            return resultGraph;
        }
        public static void WriteToGraphViz<T>(this Graph<T, int> graph, string outputFileName, string[] options)
            where T : IEquatable<T>
        {
            using (StreamWriter sw = new StreamWriter(outputFileName))
            {
                sw.WriteLine("digraph G {");
                if (options != null)
                {
                    foreach (string option in options)
                    {
                        sw.WriteLine("\t{0}", option);

                    }
                }
                foreach (Graph<T, int>.Node node in graph.Verticies)
                {
                    if (node.Value.Equals(default))
                        sw.WriteLine("\tn{0}{1}", node.nodeID, graph.edgeLabels.ContainsKey(node.nodeID) ? string.Format("[label=\"{0}\"]", graph.vertexLabels[node.nodeID]) : string.Empty);
                    else
                        sw.WriteLine("\tn{0}[label=\"{1}\"]", node.nodeID, graph.vertexLabels.ContainsKey(node.nodeID) ? graph.vertexLabels[node.nodeID] : node.Value.ToString());
                }
                foreach (Edge<T, int> edge in graph.Edges)
                {
                    sw.WriteLine("\tn{0} -> n{1}{2}", edge.Source.nodeID, edge.Target.nodeID, graph.edgeLabels.ContainsKey(edge.Weight) ? string.Format("[label=\"{0}\"]", graph.edgeLabels[edge.Weight]) : string.Empty);
                }
                sw.WriteLine("}");
            }
        }
    }
}
