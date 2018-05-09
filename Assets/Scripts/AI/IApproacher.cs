using UnityEngine;

namespace Simulation.AI
{
    interface IApproacher
    {
        INPCTarget FindTargetInSight();
        INPCTarget FindTargetByHearing();

        void Approach(INPCTarget target);
        void StopApproach();
    }
}