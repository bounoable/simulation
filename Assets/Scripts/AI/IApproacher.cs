using UnityEngine;

namespace Simulation.AI
{
    interface IApproacher
    {
        bool IsApproaching { get; }
        INPCTarget FindTarget();
        void Approach(INPCTarget target);
        void StopApproach();
    }
}