using System;
using UnityEngine;
using System.Linq;
using System.Diagnostics;
using Simulation.Support;
using System.Collections;
using System.Collections.Generic;

namespace Simulation.AI.AStar
{
    [RequireComponent(typeof(Grid))]
    class PathFinder: MonoBehaviour, IPathFinder
    {
        const float SQRT_OF_2 = 1.4142f;

        Grid grid;

        public void FindPath(Vector3 start, Vector3 target, PathRequestManager requestManager)
        {
            StartCoroutine(CalculatePath(start, target, requestManager));
        }

        public IEnumerator CalculatePath(Vector3 startPos, Vector3 targetPos, PathRequestManager requestManager)
        {
            var sw = new Stopwatch();
            sw.Start();

            Vector3[] waypoints = new Vector3[0];
            bool pathFound = false;

            Node startNode = grid.NodeFromWorldPosition(startPos);
            Node targetNode = grid.NodeFromWorldPosition(targetPos);

            var openSet = new Heap<Node>(grid.Size);
            var closedSet = new HashSet<Node>();

            openSet.Add(startNode);

            while (openSet.Count > 0) {
                Node currentNode = openSet.Unshift();
                closedSet.Add(currentNode);

                if (currentNode == targetNode) {
                    pathFound = true;
                    break;
                }

                AnalyzeNeighourNodes(currentNode, targetNode, openSet, closedSet);
            }

            yield return null;

            if (pathFound)
                waypoints = RetracePath(startNode, targetNode);

            if (requestManager != null)
                requestManager.OnProcessingFinished(waypoints, pathFound);
            
            sw.Stop();
            UnityEngine.Debug.Log(sw.ElapsedMilliseconds);
        }

        void AnalyzeNeighourNodes(Node currentNode, Node targetNode, Heap<Node> openSet, HashSet<Node> closedSet)
        {
            foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
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

        float GetNodeDistance(Node nodeA, Node nodeB)
        {
            int distanceX = Mathf.Abs(nodeA.GridPosition.x - nodeB.GridPosition.x);
            int distanceY = Mathf.Abs(nodeA.GridPosition.y - nodeB.GridPosition.y);

            return distanceX > distanceY
                ? SQRT_OF_2 * distanceY + distanceX - distanceY
                : SQRT_OF_2 * distanceX + distanceY - distanceX;
        }

        Vector3[] RetracePath(Node startNode, Node targetNode)
        {
            var path = new List<Node>();
            var currentNode = targetNode;

            while (currentNode != startNode) {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }

            Vector3[] waypoints = SimplifyPath(path);

            Array.Reverse(waypoints);

            return waypoints;
        }

        Vector3[] SimplifyPath(List<Node> path)
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

        void Awake()
        {
            grid = GetComponent<Grid>();
        }
    }
}