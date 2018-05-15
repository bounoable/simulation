using System;
using UnityEngine;
using Simulation.AI.AStar;

namespace Simulation.AI
{
    struct PathRequest
    {
        public IGrid Grid { get; private set; }
        public Vector3 Start { get; private set; }
        public Vector3 Target { get; private set; }
        public Action<Vector3[], bool> Callback { get; private set; }

        public PathRequest(IGrid grid, Vector3 start, Vector3 target, Action<Vector3[], bool> callback)
        {
            Grid = grid;
            Start = start;
            Target = target;
            Callback = callback;
        }
    }
}