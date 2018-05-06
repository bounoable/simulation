using UnityEngine;
using Simulation.Support;

namespace Simulation.AI.AStar
{
    class Node: IHeapItem<Node>
    {
        public Vector3 Position { get; private set; }
        public Vector2Int GridPosition { get; private set; }
        public bool Walkable { get; private set; }
        public float GCost { get; set; }
        public float HCost { get; set; }
        public float FCost => GCost + HCost;
        public Node Parent { get; set; }
        public int HeapIndex { get; set; }

        public Node(Vector3 position, Vector2Int gridPos, bool walkable = true)
        {
            Position = position;
            GridPosition = gridPos;
            Walkable = walkable;
        }

        public int CompareTo(Node other)
        {
            int fCostCompare = -FCost.CompareTo(other.FCost);

            if (fCostCompare == 0)
                return -HCost.CompareTo(other.HCost);
            
            return fCostCompare;
        }
    }
}