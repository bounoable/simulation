using UnityEngine;

namespace Simulation.AI
{
    abstract class Action: ScriptableObject
    {
        abstract public void Run(StateController controller);

        virtual public void Stop(StateController controller)
        {}
    }
}