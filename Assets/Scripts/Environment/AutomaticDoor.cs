using UnityEngine;

namespace Simulation.Environment
{
    class AutomaticDoor: Door
    {
        [SerializeField]
        float sensorRange = 10f;

        void OpenIfPlayerApproaches()
        {
            if (IsClosed && PlayerApproaches())
                Open();
        }

        bool PlayerApproaches()
        {
            var player = FindObjectOfType<Player.Player>();

            return player && Vector3.Distance(transform.position, player.Position) <= sensorRange;
        }

        void Update()
        {
            OpenIfPlayerApproaches();
        }
    }
}