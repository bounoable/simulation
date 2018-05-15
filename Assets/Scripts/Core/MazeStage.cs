using UnityEngine;
using Simulation.Procedural;
using Simulation.Environment;

namespace Simulation.Core
{
    [CreateAssetMenu(menuName="Stages/Maze")]
    class MazeStage: GameStage
    {
        override public void Start()
        {
            if (!Game)
                return;
            
            Game.DestroyWaypoints();
            Maze maze = Game.CreateMaze();

            SetPlayerPosition(maze);
            SpawnGoal(maze);
            maze.RecreateGrid();
            Game.Grid.RecreateGrid();
        }

        override public void Update()
        {
            if (!Game)
                return;
            
            if (Game.Goal && Game.Goal.Triggered)
                Game.Win();
        }

        override public void FixedUpdate()
        {
            if (!Game)
                return;
        }

        void SetPlayerPosition(Maze maze)
        {
            MazeNode node = maze.MazeNodeFromWorldPosition(Game.Player.Position);
            Vector3 position = node.Position + Vector3.down * node.Size.y * 0.5f;

            Game.Player.Position = position;
        }

        void SpawnGoal(Maze maze)
        {
            PressurePlate prefab = Game.Prefabs.PressurePlate;
            MazeNode node = maze.RandomNode();

            Vector3 position = node.WorldPos + Vector3.down * node.Size.y * 0.5f;

            Game.Goal = MonoBehaviour.Instantiate(prefab, position, prefab.transform.rotation);

            node.OpenRandomSide(true);

            foreach (MazeNode neighbour in maze.GetNeighbours(node)) {
                neighbour.OpenRandomSide(true);
            }

            Game.Grid.RecreateGrid();
        }
    }
}