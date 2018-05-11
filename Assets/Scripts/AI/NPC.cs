using UnityEngine;
using Simulation.Core;
using System.Collections;

namespace Simulation.AI
{
    [
        RequireComponent(typeof(StateController)),
        RequireComponent(typeof(Patroler)),
        RequireComponent(typeof(Approacher))
    ]
    class NPC: Character
    {
    }
}