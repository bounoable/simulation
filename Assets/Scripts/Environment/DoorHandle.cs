using UnityEngine;

namespace Simulation.Environment
{
    [RequireComponent(typeof(Collider))]
    class DoorHandle: MonoBehaviour
    {
        public float Range => range;

        [SerializeField]
        float range = 1f;
    }
}