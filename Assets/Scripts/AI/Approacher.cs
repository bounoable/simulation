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

        IEnumerator approach;

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
            Collider[] colliders = UnityEngine.Physics.OverlapSphere(transform.position, LookRadius);

            colliders = Array.FindAll(colliders, collider => collider.GetComponent<Player.Player>() != null);

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
            if (isApproaching)
                return;
            
            isApproaching = true;

            StartCoroutine(approach = StartApproach(target));
        }

        IEnumerator StartApproach(INPCTarget target)
        {
            while (true) {
                if (Vector3.Distance(transform.position, target.Position) < 0.5f)
                    yield break;

                transform.LookAt(target.Position);
                transform.position += transform.forward * character.MoveSpeed * Time.fixedDeltaTime;

                yield return new WaitForFixedUpdate();
            }
        }

        public void StopApproach()
        {   
            StopCoroutine(approach);
            isApproaching = false;
        }

        void Awake()
        {
            character = GetComponent<ICharacter>();
        }
    }
}