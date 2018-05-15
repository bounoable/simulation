using UnityEngine;
using Simulation.Core;

namespace Simulation.AI
{
    abstract class StateCondition: ScriptableObject
    {
        abstract public bool Check(StateController controller, GameManager game);
    }
}