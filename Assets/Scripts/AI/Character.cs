using UnityEngine;
using Simulation.Core;
using System.Collections;

namespace Simulation.AI
{
    abstract class Character: MonoBehaviour, ICharacter
    {
        public Vector3 Position => transform.position;

        public PathRequestManager PathRequestManager { get; set; }

        [SerializeField]
        protected float moveSpeed;

        Vector3[] waypoints;

        System.Action onTargetReached;

        public void MoveTo(Vector3 target)
        {
            MoveTo(target, () => {});
        }

        public void MoveTo(Vector3 target, System.Action onTargetReached)
        {
            StopMoving();

            if (PathRequestManager == null)
                return;

            this.onTargetReached = onTargetReached;
            PathRequestManager.RequestPath(transform.position, target, OnPathFound);
        }

        void OnPathFound(Vector3[] waypoints, bool success)
        {
            this.waypoints = waypoints;

            StartCoroutine(FollowPath());
        }

        IEnumerator FollowPath()
        {
            bool endTargetReached = false;

            if (waypoints.Length == 0)
                yield break;
            
            transform.position = waypoints[0];
            Vector3 endPos = waypoints[waypoints.Length - 1];
            int targetIndex = 0;

            while (!endTargetReached) {
                targetIndex++;

                if (targetIndex >= waypoints.Length) {
                    endTargetReached = true;
                    break;
                }
                
                Vector3 targetPos = waypoints[targetIndex];
                
                MoveTowards(targetPos);

                bool targetReached = false;

                while (!targetReached) {
                    if (Vector3.Distance(transform.position, targetPos) < 0.5f) {
                        targetReached = true;
                        transform.position = targetPos;
                    }
                    
                    yield return new WaitForFixedUpdate();
                }
            }

            if (onTargetReached != null)
                onTargetReached();
        }

        public void StopMoving()
        {
            StopCoroutine("StartMoveTowards");
            StopCoroutine("FollowPath");
            this.waypoints = new Vector3[0];
        }

        public void MoveTowards(Vector3 target)
        {
            StopCoroutine("StartMoveTowards");
            StartCoroutine(StartMoveTowards(target));
        }

        IEnumerator StartMoveTowards(Vector3 target)
        {
            bool targetReached = false;
            Vector3 lastPos = transform.position;

            while (!targetReached) {
                transform.LookAt(target);
                transform.Translate(Vector3.forward * moveSpeed * Time.fixedDeltaTime);

                if (Vector3.Distance(transform.position, target) < 0.5f || Vector3.Distance(transform.position, lastPos) < 0.1f)
                    targetReached = true;
                
                lastPos = transform.position;

                yield return new WaitForFixedUpdate();
            }

            transform.position = target;

            yield return new WaitForFixedUpdate();
        }
    }
}