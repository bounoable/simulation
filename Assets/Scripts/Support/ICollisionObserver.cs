using UnityEngine;

namespace Simulation.Support
{
    interface ICollisionObserver
    {
        void NotifyCollision(Collider collider, Collision collision);
    }
}