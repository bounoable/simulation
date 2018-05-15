using UnityEngine;
using System.Collections.Generic;

namespace Simulation.AI.AStar
{
    interface IGrid
    {
        int NodeCount { get; }

        INode NodeFromWorldPosition(Vector3 position);
        List<INode> GetNeighbours(INode node);
    }
}