using System.Linq;
using UnityEngine;
using Simulation.Core;
using System.Collections;
using System.Collections.Generic;

namespace Simulation.AI
{
    [RequireComponent(typeof(ICharacter))]
    class Patroler: MonoBehaviour, IPatroling
    {
        Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        Transform[] patrolWaypoints;
        Transform currentPatrolPoint;

        Queue<Transform> patrolQueue = new Queue<Transform>();

        bool patroling = false;

        ICharacter character;

        public void Patrol()
        {
            if (patroling || patrolQueue.Count == 0)
                return;
            
            patroling = true;

            currentPatrolPoint = patrolQueue.Dequeue();
            patrolQueue.Enqueue(currentPatrolPoint);

            character.MoveTo(currentPatrolPoint.position, () => patroling = false, () => patroling = false);
        }

        public void StopPatrol()
        {
            // patroling = false;
            character.StopMoving();
        }

        public void SetPatrolWaypoints(PatrolWaypoint[] waypoints)
        {
            patrolWaypoints = waypoints.Select(waypoint => waypoint.transform).ToArray();
            InitWaypoints();
        }

        void InitWaypoints()
        {
            patrolQueue.Clear();
            
            for (int i = 0; i < patrolWaypoints.Length; ++i) {
                if (patrolWaypoints[i] == null)
                    continue;
                
                patrolQueue.Enqueue(patrolWaypoints[i]);
            }
        }

        void Awake()
        {
            character = GetComponent<ICharacter>();
        
            InitWaypoints();
        }
    }
}