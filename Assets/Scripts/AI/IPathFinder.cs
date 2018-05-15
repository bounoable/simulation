using System;
using UnityEngine;
using Simulation.AI;
using Simulation.AI.AStar;
using System.Collections.Generic;

namespace Simulation.AI
{
    interface IPathFinder
    {
        void FindPath(IGrid grid, Vector3 start, Vector3 target, PathRequestManager requestManager);
    }
}