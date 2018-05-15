using UnityEngine;
using Simulation.AI;
using Simulation.Core;
using Simulation.AI.Actions;

namespace Simulation.AI
{
    [CreateAssetMenu(menuName="AI/State")]
    class State: ScriptableObject
    {
        [SerializeField] Action[] actions = new Action[0];
        [SerializeField] Transition[] transitions = new Transition[0];

        public void UpdateState(StateController controller, GameManager game)
        {
            CheckTransitions(controller, game);
            RunActions(controller, game);
        }

        void RunActions(StateController controller, GameManager game)
        {
            for (int i = 0; i < actions.Length; ++i) {
                actions[i].Run(controller, game);
            }
        }

        void CheckTransitions(StateController controller, GameManager game)
        {
            for (int i = 0; i < transitions.Length; ++i) {
                if (transitions[i] == null)
                    continue;
                
                State nextState = transitions[i].GetNextState(controller, game);

                if (nextState) {
                    StopActions(controller, game);
                    controller.CurrentState = nextState;
                    return;
                }
            }
        }

        void StopActions(StateController controller, GameManager game)
        {
            for (int i = 0; i < actions.Length; ++i) {
                if (actions[i] == null)
                    continue;
                
                actions[i].Stop(controller, game);
            }
        }
    }
}