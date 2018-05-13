using UnityEngine;

namespace Simulation.AI
{
    interface IApproacher
    {
        bool IsApproaching { get; }
        
        INPCTarget FindTargetInSight();
        INPCTarget FindTargetByHearing();

        void Approach(INPCTarget target);
        void StopApproach();
    }
}