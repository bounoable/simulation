using UnityEngine;

namespace Simulation.AI
{
    interface IPatroling
    {
        bool IsPatroling { get; }
        
        void Patrol();
        void StopPatrol();
    }
}