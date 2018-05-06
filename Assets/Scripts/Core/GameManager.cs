using UnityEngine;
using Simulation.AI;
using Simulation.AI.AStar;

namespace Simulation.Core
{
    [RequireComponent(typeof(PathFinder))]
    class GameManager: MonoBehaviour
    {
        public PathRequestManager PathRequestManager { get; private set; }
        PathFinder pathFinder;

        void Awake()
        {
            pathFinder = GetComponent<PathFinder>();
            PathRequestManager = new PathRequestManager(pathFinder);
        }
    }
}