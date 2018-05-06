using System;
using UnityEngine;

namespace Simulation.AI
{
    struct PathRequest
    {
        public Vector3 Start { get; private set; }
        public Vector3 Target { get; private set; }
        public Action<Vector3[], bool> Callback { get; private set; }

        public PathRequest(Vector3 start, Vector3 target, Action<Vector3[], bool> callback)
        {
            Start = start;
            Target = target;
            Callback = callback;
        }
    }
}