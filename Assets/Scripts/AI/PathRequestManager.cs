using System;
using UnityEngine;
using System.Collections.Generic;

namespace Simulation.AI
{
    class PathRequestManager
    {
        Queue<PathRequest> requestQueue = new Queue<PathRequest>();
        PathRequest currentRequest;
        IPathFinder pathFinder;
        bool isProcessing;

        public PathRequestManager(IPathFinder pathFinder)
        {
            this.pathFinder = pathFinder;
        }

        public void RequestPath(AStar.IGrid grid, Vector3 start, Vector3 target, Action<Vector3[], bool> callback)
        {
            RequestPath(new PathRequest(grid, start, target, callback));
        }

        public void RequestPath(PathRequest request)
        {
            requestQueue.Enqueue(request);
            TryProcessNext();
        }

        public void OnProcessingFinished(Vector3[] path, bool success)
        {
            currentRequest.Callback(path, success);
            isProcessing = false;
            TryProcessNext();
        }

        void TryProcessNext()
        {
            if (!isProcessing && requestQueue.Count > 0) {
                currentRequest = requestQueue.Dequeue();
                isProcessing = true;
                pathFinder.FindPath(currentRequest.Grid, currentRequest.Start, currentRequest.Target, this);
            }
        }
    }
}