using UnityEngine;
using Simulation.AI;

namespace Simulation.Player
{
    class Player: MonoBehaviour, INPCTarget
    {
        public Vector3 Position => transform.position;
        public Transform Transform => transform;
    }
}