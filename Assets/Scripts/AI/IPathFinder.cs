using System;
using UnityEngine;
using Simulation.AI;
using System.Collections.Generic;

namespace Simulation.AI
{
    interface IPathFinder
    {
        void FindPath(Vector3 start, Vector3 target, PathRequestManager requestManager);
    }
}