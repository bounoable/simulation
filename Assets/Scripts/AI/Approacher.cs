using System;
using System.Linq;
using UnityEngine;
using Simulation.Core;
using System.Collections;

namespace Simulation.AI
{
    [RequireComponent(typeof(ICharacter))]
    class Approacher: MonoBehaviour, IApproacher
    {
        [SerializeField]
        float LookRadius;

        int targetMask = 1 << 10;

        ICharacter character;

        bool isApproaching = false;

        public INPCTarget FindTargetInSight()
        {
            INPCTarget[] targets = FindTargets();

            if (targets.Length == 0)
                return null;

            targets = SortTargetsByDistance(targets);

            return targets[0];
        }

        public INPCTarget[] FindTargets()
        {
            Collider[] colliders =  UnityEngine.Physics.OverlapSphere(transform.position, LookRadius, targetMask);

            colliders = Array.FindAll(colliders, collider => collider.GetComponent<INPCTarget>() != null);

            return colliders.Select(collider => collider.GetComponent<INPCTarget>()).ToArray();
        }

        public INPCTarget FindTargetByHearing()
        {
            return null;

            // INPCTarget[] targets = FindObjectsOfType(typeof(INPCTarget));
        }

        INPCTarget[] SortTargetsByDistance(INPCTarget[] targets)
        {
            Array.Sort(targets, (a, b) => 
                Vector3.Distance(
                    transform.position,
                    a.Position
                ) <= Vector3.Distance(
                    transform.position,
                    b.Position
                ) ? -1 : 1
            );

            return targets;
        }

        public void Approach(INPCTarget target)
        {
            isApproaching = true;
            
            character.MoveTo(target.Position);
        }

        public void StopApproach()
        {
            isApproaching = false;

            character.StopMoving();
        }

        void Awake()
        {
            character = GetComponent<ICharacter>();
        }
    }
}