using UnityEngine;
using Simulation.AI;
using Simulation.Player;
using Simulation.AI.AStar;
using Simulation.Procedural;
using Simulation.Environment;
using System.Collections.Generic;

namespace Simulation.Core
{
    [RequireComponent(typeof(PathFinder))]
    class GameManager: MonoBehaviour
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

        PathFinder pathFinder;

        NPCFactory npcFactory;
        BuildingFactory buildingFactory;

        BuildingManager buildingManager;
        List<NPC> npcs = new List<NPC>();

        [SerializeField]
        Building[] buildingPrefabs;

        [SerializeField]
        NPC npcPrefab;

        [SerializeField]
        Transform playerSpawn;

        [SerializeField]
        Player.Player playerPrefab;
        Player.Player player;

        void SpawnPlayer()
        {
            player = Instantiate(playerPrefab, playerSpawn);

            cam.Player = player;
        }

        void Awake()
        {
            if (!npcPrefab) {
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

            npcFactory = new NPCFactory(npcPrefab, pathRequestManager);
            buildingManager = new BuildingManager(new BuildingFactory(buildingPrefabs));
        }

        void Start()
        {
            SpawnPlayer();
        }

        void FixedUpdate()
        {
            buildingManager.UpdateBuildings(this);
        }
    }
}