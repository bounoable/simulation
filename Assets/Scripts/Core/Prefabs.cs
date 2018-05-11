using UnityEngine;
using Simulation.AI;
using Simulation.Player;
using Simulation.Environment;

namespace Simulation.Core
{
    [CreateAssetMenu(menuName="Core/Prefabs")]
    class Prefabs: ScriptableObject
    {
        public bool IsValid => Player && NPC && PatrolWaypoint && PressurePlate;

        public Player.Player Player => player;
        public NPC NPC => npc;
        public Building[] Buildings => buildings;
        public PatrolWaypoint PatrolWaypoint => patrolWaypoint;
        public PressurePlate PressurePlate => pressurePlate;

        [SerializeField]
        Player.Player player;

        [SerializeField]
        NPC npc;

        [SerializeField]
        Building[] buildings;

        [SerializeField]
        PatrolWaypoint patrolWaypoint;

        [SerializeField]
        PressurePlate pressurePlate;
    }
}