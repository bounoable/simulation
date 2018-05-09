using System;
using UnityEngine;

namespace Simulation.AI
{
    interface ICharacter
    {
        Vector3 Position { get; }
        
        void MoveTo(Vector3 target);
        void MoveTo(Vector3 target, System.Action onTargetReached);
        void StopMoving();
    }
}