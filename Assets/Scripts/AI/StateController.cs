using UnityEngine;

namespace Simulation.AI
{
    class StateController: MonoBehaviour
    {
        public State CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }

        [SerializeField]
        State currentState;

        void Update()
        {
            if (currentState)
                currentState.UpdateState(this);
        }
    }
}