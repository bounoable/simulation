using UnityEngine;
using Simulation.Support;

namespace Simulation.AI.AStar
{
    interface INode: IHeapItem<INode>
    {
        Vector3 Position { get; }
        bool Walkable { get; set; }
        float GCost { get; set; }
        float HCost { get; set; }
        float FCost { get; }

        Vector2Int GridPosition { get; }
        INode Parent { get; set; }
    }
}