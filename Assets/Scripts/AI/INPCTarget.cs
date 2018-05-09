using UnityEngine;

namespace Simulation.AI
{
    interface INPCTarget
    {
        Vector3 Position { get; }
        Transform Transform { get; }
    }
}