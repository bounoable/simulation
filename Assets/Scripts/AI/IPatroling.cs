using UnityEngine;
using Simulation.AI.AStar;

namespace Simulation.AI
{
    interface IPatroling
    {
        bool IsPatroling { get; }
        
        void Patrol(IGrid grid);
        void StopPatrol();
    }
}