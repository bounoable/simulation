using UnityEngine;
using Simulation.AI;
using Simulation.AI.Actions;

namespace Simulation.AI
{
    [CreateAssetMenu(menuName="AI/State")]
    class State: ScriptableObject
    {
        [SerializeField] Action[] actions = new Action[0];
        [SerializeField] Transition[] transitions = new Transition[0];

        public void UpdateState(StateController controller)
        {
            CheckTransitions(controller);
            RunActions(controller);
        }

        void RunActions(StateController controller)
        {
            for (int i = 0; i < actions.Length; ++i) {
                actions[i].Run(controller);
            }
        }

        void CheckTransitions(StateController controller)
        {
            for (int i = 0; i < transitions.Length; ++i) {
                if (transitions[i] == null)
                    continue;
                
                State nextState = transitions[i].GetNextState(controller);

                if (nextState) {
                    StopActions(controller);
                    controller.CurrentState = nextState;
                    return;
                }
            }
        }

        void StopActions(StateController controller)
        {
            for (int i = 0; i < actions.Length; ++i) {
                if (actions[i] == null)
                    continue;
                
                actions[i].Stop(controller);
            }
        }
    }
}