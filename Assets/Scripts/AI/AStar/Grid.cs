using System;
using UnityEngine;
using System.Collections.Generic;

namespace Simulation.AI.AStar
{
    class Grid: MonoBehaviour, IGrid
    {
        static System.Random rand = new System.Random();

        public Vector3 Position => transform.position;
        public int Size => gridSize.x * gridSize.y;
        public int NodeCount => Size;
        public Vector2 WorldSize => worldSize;
        public Vector3 WorldBottomLeft => transform.position - Vector3.right * worldSize.x / 2 - Vector3.forward * worldSize.y / 2;
        public Vector2Int GridSize => gridSize;

        Vector2Int gridSize;

        [SerializeField]
        Vector2 worldSize;

        [SerializeField]
        float nodeRadius;
        float NodeRadius => nodeRadius;
        float NodeDiameter => NodeRadius * 2;

        Node[][] nodes;
        LayerMask walkableMask = (1 << 9) | (1 << 10) | (1 << 11);

        [SerializeField]
        bool drawGridGizmos = false;

        public INode NodeFromWorldPosition(Vector3 position)
        {
            position = position - transform.position;
            
            int x = Mathf.Clamp(
                Mathf.RoundToInt((position.x + worldSize.x / 2 - NodeRadius) / NodeDiameter),
                0, gridSize.x - 1
            );

            int y = Mathf.Clamp(
                Mathf.RoundToInt((position.z + worldSize.y / 2 - NodeRadius) / NodeDiameter),
                0, gridSize.y - 1
            );

            return nodes[x][y];
        }

        public Node RandomNode()
        {
            if (nodes == null)
                return null;
            
            return nodes[rand.Next(0, gridSize.x)][rand.Next(0, gridSize.y)];
        }

        public Node RandomInnerNode(float wallDistance)
        {
            if (nodes == null)
                return null;
            
            float areaPercentage = 1 - Mathf.Clamp01(wallDistance);
            
            Vector2Int innerGridSize = new Vector2Int((int)(gridSize.x * areaPercentage), ((int)(gridSize.y * areaPercentage)));
            int halfWidth = innerGridSize.x / 2;
            int halfHeight = innerGridSize.y / 2;

            return nodes[rand.Next(gridSize.x / 2 - halfWidth, innerGridSize.x)][rand.Next(gridSize.y / 2 - halfHeight, innerGridSize.y)];
        }

        public List<INode> GetNeighbours(INode node)
        {
            var neighbours = new List<INode>();

            for (int x = -1; x <= 1; ++x) {
                for (int y = -1; y <= 1; ++y) {
                    if (x == 0 && y == 0)
                        continue;
                    
                    Vector2Int nodePos = node.GridPosition;
                    var checkPoint = new Vector2Int(nodePos.x + x, nodePos.y + y);

                    if (checkPoint.x >= 0 && checkPoint.x < gridSize.x && checkPoint.y >= 0 && checkPoint.y < gridSize.y) {
                        neighbours.Add(nodes[checkPoint.x][checkPoint.y]);
                    }
                }
            }

            return neighbours;
        }

        public void RecreateGrid()
        {
            for (int x = 0; x < gridSize.x; ++x) {
                for (int y = 0; y < gridSize.y; ++y) {
                    Node node = nodes[x][y];
                    var gridPos = new Vector2Int(x, y);

                    Vector3 worldPoint = WorldBottomLeft
                        + Vector3.right * (gridPos.x * NodeDiameter + NodeRadius)
                        + Vector3.forward * (gridPos.y * NodeDiameter + NodeRadius);

                    node.Walkable = !UnityEngine.Physics.CheckSphere(worldPoint, NodeRadius, ~walkableMask);
                }
            }
        }

        void CreateGrid()
        {
            nodes = new Node[gridSize.x][];

            for (int x = 0; x < gridSize.x; ++x) {
                nodes[x] = new Node[gridSize.y];

                for (int y = 0; y < gridSize.y; ++y) {
                    nodes[x][y] = CreateNode(new Vector2Int(x, y));
                }
            }
        }

        Node CreateNode(Vector2Int gridPos)
        {
            Vector3 worldPoint = WorldBottomLeft
                + Vector3.right * (gridPos.x * NodeDiameter + NodeRadius)
                + Vector3.forward * (gridPos.y * NodeDiameter + NodeRadius);
            
            bool walkable = !UnityEngine.Physics.CheckSphere(worldPoint, NodeRadius, ~walkableMask);

            return new Node(worldPoint, gridPos, walkable);
        }

        void Awake()
        {
            gridSize = new Vector2Int(
                Mathf.RoundToInt(worldSize.x / NodeDiameter),
                Mathf.RoundToInt(worldSize.y / NodeDiameter)
            );

            CreateGrid();
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, new Vector3(worldSize.x, 0.2f, worldSize.y));

            if (nodes != null && drawGridGizmos) {
                for (int x = 0; x < nodes.Length; ++x) {
                    for (int y = 0; y < nodes[x].Length; ++y) {
                        Gizmos.color = nodes[x][y].Walkable ? Color.white : Color.red;

                        Gizmos.DrawCube(nodes[x][y].Position, Vector3.one * (NodeDiameter - 0.1f));
                    }
                }
            }
        }
    }
}