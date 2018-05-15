using UnityEngine;
using Simulation.Core;

namespace Simulation.AI
{
    class StateController: MonoBehaviour
    {
        public State CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }

        public GameManager Game { get; set; }

        [SerializeField]
        State currentState;

        void Update()
        {
            if (currentState && Game)
                currentState.UpdateState(this, Game);
        }
    }
}