using System.Linq;
using UnityEngine;
using Simulation.Core;
using System.Collections.Generic;

namespace Simulation.Procedural
{
    class Maze
    {
        static System.Random rand = new System.Random();

        public Vector3 Position { get; private set; }
        public Vector3 Size { get; private set; }
        public Vector2Int GridSize { get; private set; }

        float NodeWidth => Size.x / GridSize.x;
        float NodeHeight => Size.y;
        float NodeDepth => Size.z / GridSize.y;

        GameManager game;
        MazeNode[][] nodes;

        public Maze(GameManager game, Vector2Int gridSize)
        {
            this.game = game;
            Position = game.Grid.Position;
            Size = new Vector3(game.Grid.WorldSize.x - 1f, 10, game.Grid.WorldSize.y - 1f);
            GridSize = gridSize;

            CreateNodes();
            CreateWays();
        }

        public void CreateMaze()
        {
            for (int x = 0; x < nodes.Length; ++x)
                for (int y = 0; y < nodes[x].Length; ++y)
                    nodes[x][y].CreateWalls();
        }

        public MazeNode NodeFromWorldPosition(Vector3 position)
        {
            position = position - Position;
            
            int x = Mathf.Clamp(
                Mathf.RoundToInt((position.x + Size.x / 2 - NodeWidth / 2) / NodeWidth),
                0, GridSize.x - 1
            );

            int y = Mathf.Clamp(
                Mathf.RoundToInt((position.z + Size.z / 2 - NodeDepth / 2) / NodeDepth),
                0, GridSize.y - 1
            );

            return nodes[x][y];
        }

        GameObject CreateWall(MazeNode node)
        {
            GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);

            wall.transform.position = node.WorldPos;
            wall.transform.localScale = new Vector3(NodeWidth, NodeHeight, NodeDepth);

            return wall;
        }

        void CreateNodes()
        {
            nodes = new MazeNode[GridSize.x][];

            for (int x = 0; x < GridSize.x; ++x) {
                nodes[x] = new MazeNode[GridSize.y];

                for (int y = 0; y < GridSize.y; ++y) {
                    nodes[x][y] = CreateNode(new Vector2Int(x, y));
                }
            }
        }

        MazeNode CreateNode(Vector2Int gridPos)
        {
            Vector3 worldBottomLeft = Position - new Vector3(Size.x / 2, 0, Size.z / 2);

            Vector3 worldPos = worldBottomLeft
                + Vector3.right * (gridPos.x * NodeWidth + NodeWidth * 0.5f)
                + Vector3.up * NodeHeight * 0.5f
                + Vector3.forward * (gridPos.y * NodeDepth + NodeDepth * 0.5f);

            var node = new MazeNode(game.Prefabs.MazeWall, gridPos, worldPos, new Vector3(NodeWidth, NodeHeight, NodeDepth), MazeNode.WALL_AROUND);

            node.Options &= ~MazeNode.WALL_LEFT;

            return node;
        }

        void CreateWays()
        {
            int prevSetNumber = 0;

            for (int x = 0; x < GridSize.x; ++x) {
                MazeNode[] row = nodes[x];
                MazeNode[] previousRow = x > 0 ? nodes[x - 1] : new MazeNode[0];

                SetSetNumbers(row, ref prevSetNumber);
                ApplySetNumbersFromPreviousRow(row, previousRow);
                EqualizeAdjacentCells(row);
                OpenWays(row);
            }
        }

        void SetSetNumbers(MazeNode[] row, ref int prevSetNumber)
        {
            for (int i = 0; i < row.Length; ++i) {
                int setNumber = prevSetNumber + 1;
                MazeNode node = row[i];

                node.SetNumber = setNumber;
                prevSetNumber++;
            }
        }

        void ApplySetNumbersFromPreviousRow(MazeNode[] row, MazeNode[] previousRow)
        {
            if (row == null || previousRow == null)
                return;

            for (int i = 0; i < previousRow.Length; ++i) {
                MazeNode prevRowNode = previousRow[i];

                if (prevRowNode.HasWallRight)
                    continue;
                
                row[i].SetNumber = prevRowNode.SetNumber;
                row[i].RemoveMask(MazeNode.WALL_LEFT);
            }
        }

        void EqualizeAdjacentCells(MazeNode[] row)
        {
            for (int i = 1; i < row.Length; ++i) {
                MazeNode node = row[i];
                MazeNode previousNode = row[i - 1];

                if (node.SetNumber != previousNode.SetNumber && rand.Next(0, 2) == 0) {
                    node.SetNumber = previousNode.SetNumber;
                    previousNode.RemoveMask(MazeNode.WALL_TOP);
                    node.RemoveMask(MazeNode.WALL_BOTTOM);
                }
            }
        }

        void OpenWays(MazeNode[] row)
        {
            if (row == null)
                return;
            
            var setNumbers = new HashSet<int>();
            var openSets = new HashSet<int>();

            for (int i = 0; i < row.Length; ++i) {
                MazeNode node = row[i];

                setNumbers.Add(node.SetNumber);

                if (node.HasWallRight && rand.Next(0, 5) == 0) {
                    node.RemoveMask(MazeNode.WALL_RIGHT);
                    openSets.Add(node.SetNumber);
                }
            }

            foreach (int setNumber in setNumbers) {
                if (openSets.Contains(setNumber))
                    continue;

                for (int i = 0; i < row.Length; ++i) {
                    MazeNode node = row[i];

                    if (node.SetNumber != setNumber)
                        continue;

                    node.RemoveMask(MazeNode.WALL_RIGHT);
                    openSets.Add(setNumber);

                    break;
                }
            }
        }
    }
}