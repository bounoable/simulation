using System.Linq;
using UnityEngine;
using Simulation.Core;
using Simulation.Support;
using System.Collections;
using System.Collections.Generic;

namespace Simulation.AI
{
    [RequireComponent(typeof(ICharacter))]
    class Patroler: MonoBehaviour, IPatroling
    {
        public bool IsPatroling => patroling;

        Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value; }
        }

        Transform[] patrolWaypoints = new Transform[0];
        Transform currentPatrolPoint;

        Queue<Transform> patrolQueue = new Queue<Transform>();

        bool patroling = false;

        ICharacter character;

        public void Patrol()
        {
            if (patroling || patrolQueue.Count == 0)
                return;
            
            // var approacher = GetComponent<IApproacher>();

            // if (approacher != null && approacher.IsApproaching)
            //     return;
            
            patroling = true;

            currentPatrolPoint = patrolQueue.Dequeue();
            patrolQueue.Enqueue(currentPatrolPoint);

            character.MoveTo(currentPatrolPoint.position, () => patroling = false, () => patroling = false);
        }

        public void StopPatrol()
        {
            character.StopMoving();
            // patroling = false;
        }

        public void SetPatrolWaypoints(PatrolWaypoint[] waypoints)
        {
            var transforms = new List<Transform>();
            
            transforms.AddRange(waypoints.Select(waypoint => waypoint.transform).ToArray());
            transforms.Shuffle();

            patrolWaypoints = transforms.ToArray();

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