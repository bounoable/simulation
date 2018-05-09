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
        public bool IsApproaching { get; private set; } = false;

        [SerializeField]
        float LookRadius;

        int targetMask = 1 << 10;

        ICharacter character;

        public INPCTarget FindTarget()
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
            if (IsApproaching)
                return;

            IsApproaching = true;
            
            character.MoveTo(target.Position);
        }

        public void StopApproach()
        {
            if (!IsApproaching)
                return;
            
            IsApproaching = false;

            character.StopMoving();
        }

        void Awake()
        {
            character = GetComponent<ICharacter>();
        }
    }
}