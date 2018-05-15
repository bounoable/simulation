using System.Linq;
using UnityEngine;
using Simulation.AI;
using Simulation.UI;
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
        enum Status
        {
            Active,
            Won,
            Lost,
        }

        ThirdPersonCamera cam;

        public ICharacter[] Characters
        {
            get {
                var characters = new ICharacter[npcs.Count + 1];

                characters[0] = Player;

                for (int i = 0; i < npcs.Count; ++i) {
                    characters[i + 1] = npcs[i];
                }

                return characters;
            }
        }

        public AI.AStar.Grid Grid { get; private set; }
        public Prefabs Prefabs => prefabs;
        public BuildingManager BuildingManager { get; private set; }
        public PathRequestManager PathRequestManager { get; private set; }
        public PressurePlate Goal { get; set; }

        PathFinder pathFinder;

        NPCFactory npcFactory;
        BuildingFactory buildingFactory;
        PatrolWaypointFactory waypointFactory;
        Maze maze;

        PatrolWaypoint[] patrolWaypoints;
        List<NPC> npcs = new List<NPC>();

        Dictionary<GameStage.Stage, GameStage> stages = new Dictionary<GameStage.Stage, GameStage>();
        public GameStage CurrentStage { get; private set; }

        Status status = Status.Active;

        [SerializeField]
        Prefabs prefabs;

        [SerializeField]
        Transform playerSpawn;

        [SerializeField]
        GameStage[] gameStages = new GameStage[0];

        [SerializeField]
        GameStage initialStage;

        [SerializeField]
        BloodyScreen bloodyScreen;

        [SerializeField]
        GameObject winScreen;

        [SerializeField]
        GameObject loseScreen;

        public Player.Player Player { get; private set; }

        public void NotifyCollision(Collider collider, Collision collision)
        {
            if (collider.GetComponent<PressurePlateBall>()) {
                ReorderPatrolPoints();
            }
        }

        public Maze CreateMaze()
        {
            RepositionNPCs();

            BuildingManager.DestroyBuildings();
            maze.CreateMaze();

            return maze;
        }

        void RepositionNPCs()
        {
            for (int i = 0; i < npcs.Count; ++i) {
                MazeNode node = maze.MazeNodeFromWorldPosition(npcs[i].Position);
                Vector3 position = node.Position + Vector3.down * node.Size.y * 0.5f;
                npcs[i].transform.position = position;
            }
        }

        public void SetStage(GameStage.Stage name)
        {
            GameStage stage;

            if (stages.TryGetValue(name, out stage)) {
                CurrentStage = stage;
                CurrentStage.Start();
            }
        }

        public void DestroyWaypoints()
        {
            for (int i = 0; i < patrolWaypoints.Length; ++i) {
                Destroy(patrolWaypoints[i].gameObject);
            }

            patrolWaypoints = new PatrolWaypoint[0];
        }

        public void Lose()
        {
            loseScreen.SetActive(true);
            Time.timeScale = 0;
            status = Status.Lost;
        }

        public void Win()
        {
            winScreen.SetActive(true);
            Time.timeScale = 0;
            status = Status.Won;
        }

        public void Restart()
        {
            loseScreen.SetActive(false);
            winScreen.SetActive(false);

            DestroyNPCs();
            BuildingManager.DestroyBuildings();
            DestroyWaypoints();
            Destroy(Player.gameObject);
            maze.Destroy();
            maze = null;

            if (Goal)
                Destroy(Goal.gameObject);
            
            Time.timeScale = 1f;
                
            StartGame();
        }

        public void StartGame()
        {
            SetStage(GameStage.Stage.AI);
            SpawnBuildings();
            SpawnNPCs();
            SpawnWaypoints();
            SpawnPlayer();
            InitMaze();
        }

        void DestroyNPCs()
        {
            for (int i = 0; i < npcs.Count; ++i) {
                npcs[i].StopMoving();
                Destroy(npcs[i].gameObject);
            }

            npcs.Clear();
        }

        void InitMaze()
        {
            maze = new Maze(this, new Vector2Int(10, 10));
        }

        void SpawnBuildings()
        {
            BuildingManager.SpawnBuildings();
            Grid.RecreateGrid();

            foreach (Building building in BuildingManager.Buildings) {
                building.PressurePlate.Ball.ObserveCollisions(this);
            }
        }

        void SpawnWaypoints()
        {
            patrolWaypoints = waypointFactory.SpawnWaypoints(20);

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
            Building[] buildings = BuildingManager.Buildings.ToArray();

            for (int i = 0; i < buildings.Length; ++i) {
                SpawnNPCinBuilding(buildings[i]);
            }
        }

        void SpawnNPCinBuilding(Building building)
        {
            NPC npc = npcFactory.Spawn(building.Center);

            npc.GetComponent<StateController>().Game = this;

            npcs.Add(npc);
        }

        void SpawnPlayer()
        {
            Player = Instantiate(prefabs.Player, playerSpawn);

            cam.Player = Player;
        }

        void Awake()
        {
            if (!(prefabs && prefabs.IsValid)) {
                Destroy(gameObject);
                return;
            }

            cam = Camera.main.GetComponent<ThirdPersonCamera>();

            if (!(cam && loseScreen && winScreen)) {
                Destroy(gameObject);
                return;
            }

            Grid = GetComponent<AI.AStar.Grid>();
            pathFinder = GetComponent<PathFinder>();
            
            PathRequestManager = new PathRequestManager(pathFinder);
            npcFactory = new NPCFactory(prefabs.NPC, PathRequestManager);
            BuildingManager = new BuildingManager(new BuildingFactory(prefabs.Buildings, prefabs.PressurePlate, Grid));
            waypointFactory = new PatrolWaypointFactory(prefabs.PatrolWaypoint, Grid);

            for (int i = 0; i < gameStages.Length; ++i) {
                if (gameStages[i] == null)
                    continue;

                gameStages[i].Game = this;
                stages.Add(gameStages[i].StageName, gameStages[i]);
            }

            if (initialStage != null) {
                CurrentStage = initialStage;
            }
        }

        void Start()
        {
            StartGame();
        }

        void Update()
        {
            if (CurrentStage)
                CurrentStage.Update();

            if (Player && bloodyScreen)
                bloodyScreen.UpdateScreen(Player);

            if (Player && Player.Health <= 0)
                Lose();

            if (status != Status.Active && Input.GetKeyDown(KeyCode.Space))
                Restart();
        }

        void FixedUpdate()
        {
            if (CurrentStage) {
                CurrentStage.FixedUpdate();
            }
        }
    }
}