using System.Linq;
using UnityEngine;
using Simulation.AI;
using Simulation.Player;
using Simulation.Support;
using Simulation.AI.AStar;
using Simulation.Procedural;
using Simulation.Environment;
using System.Collections.Generic;

namespace Simulation.Core
{
    [
        RequireComponent(typeof(PathFinder)),
        RequireComponent(typeof(AI.AStar.Grid))
    ]
    class GameManager: MonoBehaviour, ICollisionObserver
    {
        ThirdPersonCamera cam;

        public ICharacter[] Characters
        {
            get {
                var characters = new ICharacter[npcs.Count + 1];

                characters[0] = player;

                for (int i = 0; i < npcs.Count; ++i) {
                    characters[i + 1] = npcs[i];
                }

                return characters;
            }
        }

        public AI.AStar.Grid Grid { get; private set; }

        PathFinder pathFinder;

        NPCFactory npcFactory;
        BuildingFactory buildingFactory;
        PatrolWaypointFactory waypointFactory;
        BuildingManager buildingManager;

        PatrolWaypoint[] patrolWaypoints;
        List<NPC> npcs = new List<NPC>();

        [SerializeField]
        Prefabs prefabs;

        [SerializeField]
        Transform playerSpawn;

        Player.Player player;

        public void NotifyCollision(Collider collider, Collision collision)
        {
            if (collider.GetComponent<PressurePlateBall>()) {
                ReorderPatrolPoints();
            }
        }

        void SpawnBuildings()
        {
            buildingManager.SpawnBuildings(4);
            Grid.RecreateGrid();

            foreach (Building building in buildingManager.Buildings) {
                building.PressurePlate.Ball.ObserveCollisions(this);
            }
        }

        void SpawnWaypoints()
        {
            patrolWaypoints = waypointFactory.SpawnWaypoints();

            for (int i = 0; i < npcs.Count; ++i) {
                npcs[i].GetComponent<Patroler>().SetPatrolWaypoints(patrolWaypoints);
            }
        }

        void ReorderPatrolPoints()
        {
            for (int i = 0; i < npcs.Count; ++i) {
                npcs[i].GetComponent<Patroler>().SetPatrolWaypoints(new PatrolWaypoint[0]);
            }
            
            for (int i = 0; i < patrolWaypoints.Length; ++i) {
                Destroy(patrolWaypoints[i].gameObject);
            }

            SpawnWaypoints();
        }

        void SpawnNPCs()
        {
            Building[] buildings = buildingManager.Buildings.ToArray();

            for (int i = 0; i < buildings.Length; ++i) {
                SpawnNPCinBuilding(buildings[i]);
            }
        }

        void SpawnNPCinBuilding(Building building)
        {
            NPC npc = npcFactory.Spawn(building.Center);

            npcs.Add(npc);
        }

        void SpawnPlayer()
        {
            player = Instantiate(prefabs.Player, playerSpawn);

            cam.Player = player;
        }

        void Awake()
        {
            if (!(prefabs && prefabs.IsValid)) {
                Destroy(gameObject);
                return;
            }

            cam = Camera.main.GetComponent<ThirdPersonCamera>();

            if (!cam) {
                Destroy(gameObject);
                return;
            }

            pathFinder = GetComponent<PathFinder>();
            var pathRequestManager = new PathRequestManager(pathFinder);

            Grid = GetComponent<AI.AStar.Grid>();

            npcFactory = new NPCFactory(prefabs.NPC, pathRequestManager);
            buildingManager = new BuildingManager(new BuildingFactory(prefabs.Buildings, prefabs.PressurePlate, Grid));
            waypointFactory = new PatrolWaypointFactory(prefabs.PatrolWaypoint, Grid);
        }

        void Start()
        {
            SpawnBuildings();
            SpawnNPCs();
            SpawnWaypoints();
            SpawnPlayer();
        }

        void FixedUpdate()
        {
            buildingManager.UpdateBuildings(this);
        }
    }
}