using UnityEngine;
using Simulation.Support;

namespace Simulation.Environment
{
    class ManualDoor: Door, ICollisionObserver
    {
        public bool WasRecentlyOpened { get; set; } = false;

        [SerializeField]
        DoorHandle doorHandle;

        public void NotifyCollision(Collider collider, Collision collision)
        {
            var handle = collider.GetComponent<DoorHandle>();

            if (handle == doorHandle) {
                doorHandle.Trigger(() => Open(() => WasRecentlyOpened = true));
            }
        }

        override protected void Awake()
        {
            base.Awake();

            if (!doorHandle)
                Destroy(gameObject);
            
            doorHandle.ObserveCollisions(this);
        }
    }
}