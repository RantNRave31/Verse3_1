using System;

namespace GKYU.StateMachinesLibrary
{
    public class SimpleOneTimeGate
        : SimpleStateMachine
    {
        internal enum STATE : int
        {
            INITIAL = 0,
            START,
            OCCUPIED,
            FINAL
        }
        internal enum COMMAND : int
        {
            HALT = 0,
            ENTER,
            BLOCK,
            EXIT,
        }
        private bool _parameterValue;
        public bool Closed
        {
            get
            {
                return _parameterValue;
            }
            set
            {
                _parameterValue = value;
            }
        }
        public string State
        {
            get
            {
                switch (currentState)
                {
                    case 0:
                        return "Initial";
                    case 1:
                        return "Start";
                    case 2:
                        return "Occupied";
                    case 3:
                        return "Blocked";
                    default:
                        return "Out of Range";
                }
            }
        }
        public SimpleOneTimeGate(bool parameterValue)
            : base()
        {
            _parameterValue = parameterValue;

            actions.Add((x) => { if (_parameterValue) return (int)COMMAND.ENTER; else return (int)COMMAND.BLOCK; });
            actions.Add((x) => { return 0; });
            actions.Add((x) => { return 0; });
            actions.Add((x) => { return 0; });

            AddTransition((int)STATE.INITIAL, (int)COMMAND.ENTER, (int)STATE.START);
            AddTransition((int)STATE.INITIAL, (int)COMMAND.BLOCK, (int)STATE.FINAL);
            AddTransition((int)STATE.START, (int)COMMAND.ENTER, (int)STATE.OCCUPIED);
            AddTransition((int)STATE.START, (int)COMMAND.BLOCK, (int)STATE.FINAL);
            AddTransition((int)STATE.START, (int)COMMAND.EXIT, (int)STATE.FINAL);
            AddTransition((int)STATE.OCCUPIED, (int)COMMAND.ENTER, (int)STATE.OCCUPIED);
            AddTransition((int)STATE.OCCUPIED, (int)COMMAND.EXIT, (int)STATE.FINAL);
            AddTransition((int)STATE.FINAL, (int)COMMAND.ENTER, (int)STATE.FINAL);
            AddTransition((int)STATE.FINAL, (int)COMMAND.BLOCK, (int)STATE.FINAL);
            AddTransition((int)STATE.FINAL, (int)COMMAND.EXIT, (int)STATE.FINAL);
        }
        public bool Enter()
        {
            bool result = _parameterValue && currentState == (int)STATE.INITIAL;
            MoveNextState((int)COMMAND.ENTER);
            return result;
        }
        public bool Exit()
        {
            MoveNextState((int)COMMAND.EXIT);
            return true;
        }
    }
}
