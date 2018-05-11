using UnityEngine;
using Simulation.Support;
using System.Collections;
using System.Collections.Generic;

namespace Simulation.Environment
{
    [RequireComponent(typeof(Collider))]
    class DoorHandle: MonoBehaviour, ICollisionObservable
    {
        public bool Triggered { get; private set; } = false;

        Collider col;
        HashSet<ICollisionObserver> observers = new HashSet<ICollisionObserver>();

        Vector3 pushedPos;

        public void Trigger(System.Action callback)
        {
            if (Triggered)
                return;
            
            Triggered = true;

            StartCoroutine(AnimateTrigger(callback));
        }

        IEnumerator AnimateTrigger(System.Action callback)
        {
            while (true) {
                if (transform.position == pushedPos)
                    break;
                
                transform.position = Vector3.MoveTowards(transform.position, pushedPos, Time.fixedDeltaTime);

                yield return new WaitForFixedUpdate();
            }

            if (callback != null)
                callback();
        }

        public void ObserveCollisions(ICollisionObserver observer) => observers.Add(observer);

        void OnCollisionEnter(Collision collision)
        {
            foreach (ICollisionObserver observer in observers) {
                observer.NotifyCollision(col, collision);
            }
        }

        void Awake()
        {
            col = GetComponent<Collider>();
            pushedPos = transform.position + transform.forward * 0.75f;
        }
    }
}