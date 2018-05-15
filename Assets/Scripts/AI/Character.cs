using UnityEngine;
using Simulation.Core;
using System.Collections;
using Simulation.AI.AStar;

namespace Simulation.AI
{
    abstract class Character: MonoBehaviour, ICharacter
    {
        public Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }
        
        public PathRequestManager PathRequestManager { get; set; }
        public float MoveSpeed => moveSpeed;

        [SerializeField]
        protected float moveSpeed;

        Vector3[] waypoints = new Vector3[0];

        System.Action onTargetReached;
        System.Action onPathFail;

        public void MoveTo(Vector3 target, IGrid grid) => MoveTo(target, grid, () => {}, () => {});
        public void MoveTo(Vector3 target, IGrid grid, System.Action onTargetReached) => MoveTo(target, grid, onTargetReached, () => {});
        public void MoveTo(Vector3 target, IGrid grid, System.Action onTargetReached, System.Action onPathFail)
        {
            StopMoving();
            StartCoroutine(RealMoveTo(grid, target, onTargetReached, onPathFail));
        }

        IEnumerator RealMoveTo(IGrid grid, Vector3 target, System.Action onTargetReached, System.Action onPathFail)
        {
            if (PathRequestManager == null)
                yield break;

            this.onTargetReached = onTargetReached;
            this.onPathFail = onPathFail;
            PathRequestManager.RequestPath(grid, transform.position, target, OnPathFound);
        }

        void OnPathFound(Vector3[] waypoints, bool success)
        {
            if (!success) {
                if (onPathFail != null)
                    onPathFail();
                
                return;
            }

            this.waypoints = waypoints;

            StartCoroutine(FollowPath());
        }

        IEnumerator FollowPath()
        {
            if (waypoints.Length == 0)
                yield break;
            
            Vector3 endPos = waypoints[waypoints.Length - 1];
            int targetIndex = 0;

            while (true) {
                if (targetIndex >= waypoints.Length)
                    break;
                
                if (Vector3.Distance(transform.position, endPos) < 0.2f)
                    break;
                
                Vector3 targetPos = waypoints[targetIndex];

                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.fixedDeltaTime);
                transform.rotation = Quaternion.LookRotation(targetPos - transform.position, Vector3.up);

                if (transform.position == targetPos)
                    targetIndex++;

                yield return new WaitForFixedUpdate();
            }

            if (onTargetReached != null)
                onTargetReached();
            
            waypoints = new Vector3[0];
        }

        public void StopMoving()
        {
            StopCoroutine("FollowPath");
            waypoints = new Vector3[0];
        }
    }
}