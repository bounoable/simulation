using System;
using UnityEngine;
using System.Linq;
using System.Diagnostics;
using Simulation.Support;
using System.Collections;
using System.Collections.Generic;

namespace Simulation.AI.AStar
{
    class PathFinder: MonoBehaviour, IPathFinder
    {
        const float SQRT_OF_2 = 1.4142f;

        public void FindPath(IGrid grid, Vector3 start, Vector3 target, PathRequestManager requestManager)
        {
            StartCoroutine(CalculatePath(grid, start, target, requestManager));
        }

        public IEnumerator CalculatePath(IGrid grid, Vector3 startPos, Vector3 targetPos, PathRequestManager requestManager)
        {
            if (grid == null)
                yield break;
            
            Vector3[] waypoints = new Vector3[0];
            bool pathFound = false;

            INode startNode = grid.NodeFromWorldPosition(startPos);
            INode targetNode = grid.NodeFromWorldPosition(targetPos);

            var openSet = new Heap<INode>(grid.NodeCount);
            var closedSet = new HashSet<INode>();

            openSet.Add(startNode);

            while (openSet.Count > 0) {
                INode currentNode = openSet.Unshift();
                closedSet.Add(currentNode);

                if (currentNode == targetNode) {
                    pathFound = true;
                    break;
                }

                AnalyzeNeighbourNodes(grid, currentNode, targetNode, openSet, closedSet);
            }

            yield return null;

            if (pathFound)
                waypoints = RetracePath(startNode, targetNode);

            if (requestManager != null)
                requestManager.OnProcessingFinished(waypoints, pathFound);
        }

        void AnalyzeNeighbourNodes(IGrid grid, INode currentNode, INode targetNode, Heap<INode> openSet, HashSet<INode> closedSet)
        {
            foreach (INode neighbour in grid.GetNeighbours(currentNode)) {
                if (!neighbour.Walkable || closedSet.Contains(neighbour))
                    continue;
                
                float newGCost = currentNode.GCost + GetNodeDistance(currentNode, neighbour);

                if (newGCost < neighbour.GCost || !openSet.Contains(neighbour)) {
                    neighbour.GCost = newGCost;
                    neighbour.HCost = GetNodeDistance(neighbour, targetNode);
                    neighbour.Parent = currentNode;

                    if (!openSet.Contains(neighbour)) {
                        openSet.Add(neighbour);
                        openSet.UpdateItem(neighbour);
                    }
                }
            }
        }

        float GetNodeDistance(INode nodeA, INode nodeB)
        {
            int distanceX = Mathf.Abs(nodeA.GridPosition.x - nodeB.GridPosition.x);
            int distanceY = Mathf.Abs(nodeA.GridPosition.y - nodeB.GridPosition.y);

            return distanceX > distanceY
                ? SQRT_OF_2 * distanceY + distanceX - distanceY
                : SQRT_OF_2 * distanceX + distanceY - distanceX;
        }

        Vector3[] RetracePath(INode startNode, INode targetNode)
        {
            var path = new List<INode>();
            var currentNode = targetNode;

            while (currentNode != startNode) {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            Vector3[] waypoints = SimplifyPath(path);

            Array.Reverse(waypoints);

            return waypoints;
        }

        Vector3[] SimplifyPath(List<INode> path)
        {
            var waypoints = new List<Vector3>();
            var oldDirection = Vector2.zero;

            for (int i = 1; i < path.Count; ++i) {
                var newDirection = new Vector2(
                    path[i - 1].GridPosition.x - path[i].GridPosition.x,
                    path[i - 1].GridPosition.y - path[i].GridPosition.y
                );

                if (newDirection != oldDirection) {
                    waypoints.Add(path[i].Position);
                }
            }

            return waypoints.ToArray();
        }
    }
}