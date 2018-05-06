using UnityEngine;

namespace Simulation.Physics
{
    class PhysicsVolume
    {
        Bounds bounds;

        public PhysicsVolume(Bounds bounds)
        {
            this.bounds = bounds;
        }

        bool Contains(Vector3 point) => this.bounds.Contains(point);
    }
}