using System;
using System.Collections.Generic;

namespace GKYU.StateMachinesLibrary
{
    public class StateMachine
    {
        public enum TRANSITION_TYPE
        {
            NORMAL,
            CALL,
            RETURN,
        }
        public struct Transition
        {
            public TRANSITION_TYPE transitionType;
            public int commandID;
            public int targetStateID;
            public int actionID;
            public Transition(TRANSITION_TYPE transitionType, int commandID, int targetStateID, int actionID)
            {
                this.transitionType = transitionType;
                this.commandID = commandID;
                this.targetStateID = targetStateID;
                this.actionID = actionID;
            }
            public override string ToString()
            {
                return string.Format("{0} -> {1}", commandID, targetStateID);
            }
        }
        public struct State
        {
            public int stateID;
            public Transition[] transitions;
            public Dictionary<int, int> transitionMap;
            public State(int stateID, Transition[] transitions)
            {
                this.stateID = stateID;
                transitionMap = new Dictionary<int, int>();
                if (transitions == null)
                    this.transitions = new Transition[0];
                else
                {
                    this.transitions = transitions;
                    for (int index = 0; index < transitions.Length; index++)
                    {
                        transitionMap.Add(transitions[index].commandID, index);
                    }

                }
            }
            public void SetTransitions(Transition[] transitions)
            {
                this.transitions = transitions;
                transitionMap.Clear();
                if (null != transitions)
                {
                    for (int index = 0; index < transitions.Length; index++)
                    {
                        transitionMap.Add(transitions[index].commandID, index);
                    }
                }
            }
        }

        public State[] states;
        public int CurrentStateID = 0;
        public int FinalStateID = 0;
        public List<Func<int, int>> actions;
        public Stack<State> callStack = new Stack<State>();
        public StateMachine()
        {
        }
        public virtual void MoveNextState(int commandID)
        {
            while (commandID != 0)
            {
                State currentState = states[CurrentStateID];
                int transitionIndex = 0;
                if (currentState.transitionMap.ContainsKey(commandID))
                    transitionIndex = currentState.transitionMap[commandID];
                else
                    transitionIndex = currentState.transitionMap[0];
                ref Transition transition = ref currentState.transitions[transitionIndex];
                commandID = actions[transition.actionID](commandID);
                switch (transition.transitionType)
                {
                    case TRANSITION_TYPE.NORMAL:
                        currentState = states[transition.targetStateID];
                        CurrentStateID = currentState.stateID;
                        break;
                    case TRANSITION_TYPE.CALL:
                        callStack.Push(currentState);
                        currentState = states[transition.targetStateID];
                        CurrentStateID = currentState.stateID;
                        break;
                    case TRANSITION_TYPE.RETURN:
                        currentState = callStack.Pop();
                        CurrentStateID = currentState.stateID;
                        break;
                }
                //Console.WriteLine("State[{0}]", currentStateID);
            }
        }
    }
    public class StateMachineTest
        : StateMachine
    {
        protected readonly IEnumerator<int> inputSource;
        protected bool bEOF = false;
        public StateMachineTest(IEnumerable<int> inputSource)
        {
            this.inputSource = inputSource.GetEnumerator();
            int match = 6;
            int noMatch = 7;
            FinalStateID = 8;
            actions = new List<Func<int, int>>()
            {
                { (x) => { return 0; } },
                { (x) => { NextInput();  return Peek(); } },
                { (x) => { Console.WriteLine("Match");  return x; } },
                { (x) => { Console.WriteLine("No Match");  return x; } },
                { (x) => { Console.WriteLine("End");  return 0; } },
            };
            states = new State[]
            {
                new State(0, new Transition[]{
                    new Transition(TRANSITION_TYPE.NORMAL, -1, FinalStateID, 4),
                    new Transition(TRANSITION_TYPE.NORMAL, 'a', 1, 1),
                }),
                new State(1, new Transition[]{
                    new Transition(TRANSITION_TYPE.NORMAL, '=', 2, 1),
                }),
                new State(2, new Transition[]{
                    new Transition(TRANSITION_TYPE.CALL, '\'', 4, 1),
                    new Transition(TRANSITION_TYPE.NORMAL, 0, 0, 0),
                }),
                new State(3, new Transition[]{
                    new Transition(TRANSITION_TYPE.NORMAL, -1, FinalStateID, 1),
                    new Transition(TRANSITION_TYPE.NORMAL, 0, 5, 1),
                }),
                new State(4, new Transition[]{
                    new Transition(TRANSITION_TYPE.NORMAL, 0, noMatch, 1),
                    new Transition(TRANSITION_TYPE.NORMAL, 'a', 5, 1),
                }),
                new State(5, new Transition[]{
                    new Transition(TRANSITION_TYPE.NORMAL, 0, noMatch, 1),
                    new Transition(TRANSITION_TYPE.NORMAL, '\'', match, 1),
                }),
                new State(6, new Transition[]{
                    new Transition(TRANSITION_TYPE.RETURN, 0, 0, 2),
                }),
                new State(7, new Transition[]{
                    new Transition(TRANSITION_TYPE.RETURN, 0, 0, 3),
                }),
                new State(8, new Transition[]{
                }),
            };
        }
        public int Peek()
        {
            if (bEOF)
                return -1;
            else
                return inputSource.Current;
        }
        public void NextInput()
        {
            bEOF = !inputSource.MoveNext();
        }
        public void Scan()
        {
            NextInput();
            MoveNextState(Peek());
        }
        public static void Test()
        {
            int[] sequence = new int[] { 'a', '=', '\'', 'a', '\'' };
            StateMachineTest tester = new StateMachineTest(sequence);
            tester.Scan();
        }
    }
}
