using System;
using UnityEngine;

namespace Simulation.AI
{
    interface ICharacter
    {
        Vector3 Position { get; }
        float MoveSpeed { get; }
        
        void MoveTo(Vector3 target);
        void MoveTo(Vector3 target, System.Action onTargetReached);
        void MoveTo(Vector3 target, System.Action onTargetReached, System.Action onPathFail);
        void StopMoving();
    }
}