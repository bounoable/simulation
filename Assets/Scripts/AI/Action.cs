using UnityEngine;
using Simulation.Core;

namespace Simulation.AI
{
    abstract class Action: ScriptableObject
    {
        virtual public void Run(StateController controller, GameManager game)
        {}

        virtual public void Stop(StateController controller, GameManager game)
        {}
    }
}