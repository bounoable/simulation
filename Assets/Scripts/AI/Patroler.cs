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

        [SerializeField]
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

            character.StopMoving();
            character.MoveTo(currentPatrolPoint.position, () => patroling = false);
        }

        public void StopPatrol()
        {   
            patroling = false;
            character.StopMoving();
        }

        void Awake()
        {
            character = GetComponent<ICharacter>();
            
            for (int i = 0; i < patrolWaypoints.Length; ++i) {
                if (patrolWaypoints[i] == null)
                    continue;
                
                patrolQueue.Enqueue(patrolWaypoints[i]);
            }
        }
    }
}