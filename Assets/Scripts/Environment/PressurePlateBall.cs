using UnityEngine;
using Simulation.Support;
using System.Collections.Generic;

namespace Simulation.Environment
{
    class PressurePlateBall: MonoBehaviour, ICollisionObservable
    {
        public PressurePlate Plate { get; set; }

        HashSet<ICollisionObserver> observers = new HashSet<ICollisionObserver>();

        public void ObserveCollisions(ICollisionObserver observer) => observers.Add(observer);

        void OnCollisionEnter(Collision collision)
        {
            Collider collider = collision.collider;
            var player = collider.GetComponent<Player.Player>();

            if (!player)
                return;
            
            collider = GetComponent<Collider>();
            
            foreach (ICollisionObserver observer in observers) {
                observer.NotifyCollision(collider, collision);
            }
        }
    }
}