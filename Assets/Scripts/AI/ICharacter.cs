using System;
using UnityEngine;
using Simulation.AI.AStar;

namespace Simulation.AI
{
    interface ICharacter
    {
        Vector3 Position { get; }
        float MoveSpeed { get; }
        
        void MoveTo(Vector3 target, IGrid grid);
        void MoveTo(Vector3 target, IGrid grid, System.Action onTargetReached);
        void MoveTo(Vector3 target, IGrid grid, System.Action onTargetReached, System.Action onPathFail);
        void StopMoving();
    }
}