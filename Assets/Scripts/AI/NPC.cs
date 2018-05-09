using UnityEngine;
using Simulation.Core;
using System.Collections;

namespace Simulation.AI
{
    [
        RequireComponent(typeof(StateController)),
        RequireComponent(typeof(Patroler)),
        RequireComponent(typeof(Approacher))
    ]
    class NPC: MonoBehaviour, ICharacter
    {
        StateController stateController;

        [SerializeField]
        GameManager game;

        PathRequestManager pathRequestManager;

        [SerializeField]
        State initialState;

        [SerializeField]
        float moveSpeed;

        Vector3[] waypoints;

        public void MoveTo(Vector3 target)
        {
            StopMoving();

            pathRequestManager.RequestPath(transform.position, target, OnPathFound);
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
            
            transform.position = waypoints[0];
            Vector3 endPos = waypoints[waypoints.Length - 1];
            int targetIndex = 0;

            while (transform.position != endPos) {
                targetIndex++;

                if (targetIndex >= waypoints.Length)
                    break;
                
                Vector3 targetPos = waypoints[targetIndex];
                
                MoveTowards(targetPos);

                while (transform.position != targetPos) {
                    yield return new WaitForFixedUpdate();
                }
            }
        }

        public void StopMoving()
        {
            StopCoroutine("StartMoveTowards");
            StopCoroutine("FollowPath");
            this.waypoints = new Vector3[0];
        }

        public void MoveTowards(Vector3 target)
        {
            StartCoroutine(StartMoveTowards(target));
        }

        IEnumerator StartMoveTowards(Vector3 target)
        {
            while (Vector3.Distance(transform.position, target) > 0.5f) {
                transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.fixedDeltaTime);
                transform.LookAt(target);

                yield return new WaitForFixedUpdate();
            }

            transform.position = target;
        }

        void Awake()
        {
            if (!game) {
                Destroy(gameObject);
                return;
            }

            pathRequestManager = game.PathRequestManager;
            stateController = GetComponent<StateController>();
        }

        void Start()
        {
            stateController.CurrentState = initialState;
        }
    }
}