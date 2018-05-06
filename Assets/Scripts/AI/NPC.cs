using UnityEngine;
using Simulation.Core;

namespace Simulation.AI
{
    [
        RequireComponent(typeof(StateController)),
        RequireComponent(typeof(Patroler))
    ]
    class NPC: MonoBehaviour
    {
        StateController stateController;

        [SerializeField]
        State initialState;

        void Awake()
        {
            stateController = GetComponent<StateController>();
        }

        void Start()
        {
            stateController.CurrentState = initialState;
        }
    }
}