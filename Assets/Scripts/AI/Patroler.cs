using UnityEngine;
using Simulation.Core;
using System.Collections;

namespace Simulation.AI
{
    class Patroler: MonoBehaviour, IPatroling
    {
        Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value;}
        }

        [SerializeField]
        GameManager game;

        PathRequestManager pathRequestManager;
        Vector3[] waypoints = new Vector3[0];
        bool patroling = false;

        [SerializeField]
        float patrolMoveSpeed;

        [SerializeField]
        Transform patrolStart;

        [SerializeField]
        Transform patrolEnd;

        public void Patrol()
        {
            if (patroling || !(patrolStart && patrolEnd))
                return;
            
            patroling = true;
            pathRequestManager.RequestPath(patrolStart.position, patrolEnd.position, OnPathFound);
        }

        public void StopPatrol()
        {
            if (!patroling)
                return;
            
            StopCoroutine("FollowPath");
        }

        void OnPathFound(Vector3[] waypoints, bool success)
        {
            this.waypoints = waypoints;

            StartCoroutine(FollowPath());
        }

        IEnumerator FollowPath()
        {
            if (waypoints.Length == 0)
                yield break;
            
            Position = waypoints[0];
            Vector3 endPos = waypoints[waypoints.Length - 1];
            int targetIndex = 0;

            while (Position != endPos) {
                targetIndex++;

                if (targetIndex >= waypoints.Length)
                    break;
                
                Vector3 targetPos = waypoints[targetIndex];

                while (Position != targetPos) {
                    Position = Vector3.MoveTowards(Position, targetPos, patrolMoveSpeed * Time.fixedDeltaTime);
                    transform.LookAt(targetPos);

                    yield return new WaitForFixedUpdate();
                }
            }
        }

        void Awake()
        {
            if (!game) {
                Destroy(gameObject);
                return;
            }
            
            pathRequestManager = game.PathRequestManager;
        }
    }
}