using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GKYU.StateMachinesLibrary
{
    using GKYU.CollectionsLibrary.Collections.Graphs;

    public static class StateMachineExtensions
    {
        public static Graph<int, int> ToGraph(this StateMachine stateMachine)
        {
            Graph<int, int> result = new Graph<int, int>();
            // First Pass: Create Nodes
            for (int stateID = 0; stateID < stateMachine.states.Length; stateID++)
            {
                ref StateMachine.State state = ref stateMachine.states[stateID];
                result.CreateNode(state.stateID);
            }
            // Second Pass: Generate Edges
            for (int stateID = 0; stateID < stateMachine.states.Length; stateID++)
            {
                ref StateMachine.State state = ref stateMachine.states[stateID];
                for (int transitionID = 0; transitionID < state.transitions.Length; transitionID++)
                {
                    ref StateMachine.Transition transition = ref state.transitions[transitionID];
                    switch (transition.transitionType)
                    {
                        case StateMachine.TRANSITION_TYPE.CALL:
                            result.AddEdge(state.stateID, transition.targetStateID, 1);
                            result.AddEdge(transition.targetStateID, state.stateID, 0);
                            break;
                        case StateMachine.TRANSITION_TYPE.RETURN:
                            break;
                        case StateMachine.TRANSITION_TYPE.NORMAL:
                            result.AddEdge(state.stateID, transition.targetStateID, transition.commandID);
                            break;
                    }
                }
            }
            return result;
        }
    }
}
