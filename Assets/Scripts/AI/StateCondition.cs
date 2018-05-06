using UnityEngine;

namespace Simulation.AI
{
    abstract class StateCondition: ScriptableObject
    {
        abstract public bool Check(StateController controller);
    }
}