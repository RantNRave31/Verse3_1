using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.StateMachinesLibrary
{
    public class SimpleStateMachine
    {
        protected Dictionary<Tuple<int, int>, int> transitions;
        public int currentState;
        public List<Func<int, int>> actions;
        public SimpleStateMachine()
        {
            currentState = 0;
            transitions = new Dictionary<Tuple<int, int>, int>();
            actions = new List<Func<int, int>>();
            actions.Add(null);
        }
        public void AddTransition(int previousStateID, int command, int nextStateID)
        {
            transitions.Add(new Tuple<int, int>(previousStateID, command), nextStateID);
        }

        public void MoveNextState(int command)
        {
            int nextState;
            int result = -1;
            while (0 != result)
            {
                if (!transitions.TryGetValue(new Tuple<int, int>(currentState, command), out nextState))
                    throw new Exception("Transition Not Found");
                currentState = nextState;
                if (null != actions[currentState])
                {
                    result = actions[currentState](command);
                }
            }
        }

        public static void Test()
        {
            SimpleStateMachine stateMachine = new SimpleStateMachine();
            stateMachine.actions.Add((x) => { return 1; });
            stateMachine.actions.Add((x) => { return 0; });
            stateMachine.actions.Add((x) => { return 0; });
            stateMachine.actions.Add((x) => { return 0; });

            stateMachine.AddTransition(0, 1, 1);
            stateMachine.AddTransition(1, 1, 2);
            stateMachine.AddTransition(2, 1, 3);
            stateMachine.AddTransition(3, 1, 4);

            stateMachine.MoveNextState(1);
            Debug.Assert(stateMachine.currentState == 2);
            stateMachine.MoveNextState(1);
            Debug.Assert(stateMachine.currentState == 3);
            stateMachine.MoveNextState(1);
            Debug.Assert(stateMachine.currentState == 4);
        }
    }
}
