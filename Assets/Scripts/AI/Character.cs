using UnityEngine;
using Simulation.Core;
using System.Collections;

namespace Simulation.AI
{
    abstract class Character: MonoBehaviour, ICharacter
    {
        public Vector3 Position => transform.position;
        public PathRequestManager PathRequestManager { get; set; }
        public float MoveSpeed => moveSpeed;

        [SerializeField]
        protected float moveSpeed;

        Vector3[] waypoints = new Vector3[0];

        System.Action onTargetReached;
        System.Action onPathFail;

        public void MoveTo(Vector3 target) => MoveTo(target, () => {}, () => {});
        public void MoveTo(Vector3 target, System.Action onTargetReached) => MoveTo(target, onTargetReached, () => {});
        public void MoveTo(Vector3 target, System.Action onTargetReached, System.Action onPathFail)
        {
            StopMoving();

            if (PathRequestManager == null)
                return;

            this.onTargetReached = onTargetReached;
            this.onPathFail = onPathFail;
            PathRequestManager.RequestPath(transform.position, target, OnPathFound);
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
            
            transform.position = waypoints[0];
            Vector3 endPos = waypoints[waypoints.Length - 1];
            int targetIndex = 0;

            while (true) {
                if (targetIndex >= waypoints.Length)
                    break;
                
                if (Vector3.Distance(transform.position, endPos) < 0.2f)
                    break;
                
                Vector3 targetPos = waypoints[targetIndex];

                transform.LookAt(targetPos);
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.fixedDeltaTime);

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