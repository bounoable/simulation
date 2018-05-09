using UnityEngine;
using Simulation.Player;

namespace Simulation.Player
{
    class ThirdPersonCamera: MonoBehaviour
    {
        public Player Player { get; set; }

        [SerializeField]
        float distance;

        void Update()
        {
            if (!Player)
                return;

            Transform head = Player.Head;

            transform.position = head.position - Player.Transform.forward * distance;
            transform.LookAt(head.position);
        }
    }
}