using UnityEngine;
using Simulation.Core;
using System.Collections;

namespace Simulation.AI
{
    [RequireComponent(typeof(ICharacter))]
    class Patroler: MonoBehaviour, IPatroling
    {
        Vector3 Position
        {
            get { return transform.position; }
            set { transform.position = value;}
        }

        bool patroling = false;

        [SerializeField]
        Transform patrolStart;

        [SerializeField]
        Transform patrolEnd;

        ICharacter character;

        public void Patrol()
        {
            if (patroling || !(patrolStart && patrolEnd))
                return;
            
            patroling = true;
            Position = patrolStart.position;
            
            character.MoveTo(patrolEnd.position);
        }

        public void StopPatrol()
        {
            if (!patroling)
                return;
            
            patroling = false;
            
            character.StopMoving();
        }

        void Awake()
        {
            character = GetComponent<ICharacter>();
        }
    }
}