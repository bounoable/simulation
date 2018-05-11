using UnityEngine;
using Simulation.AI;
using Simulation.AI.AStar;

namespace Simulation.Procedural
{
    class PatrolWaypointFactory
    {
        static System.Random rand = new System.Random();

        PatrolWaypoint prefab;
        AI.AStar.Grid grid;

        public PatrolWaypointFactory(PatrolWaypoint prefab, AI.AStar.Grid grid)
        {
            if (!(prefab && grid))
                throw new System.ArgumentNullException();
            
            this.prefab = prefab;
            this.grid = grid;
        }

        public PatrolWaypoint[] SpawnWaypoints(int count = 3)
        {
            var positions = new Vector3[] {
                new Vector3(5, 0, 5),
                new Vector3(grid.WorldSize.x - 5, 0, 5),
                new Vector3(grid.WorldSize.x - 5, 0, grid.WorldSize.y - 5),
                new Vector3(5, 0, grid.WorldSize.y - 5)
            };

            count = Mathf.Clamp(count, 1, positions.Length);

            var usedPositions = new Vector3[3];
            var waypoints = new PatrolWaypoint[3];

            for (int i = 0; i < 3; ++i) {
                Vector3 position = RandomPosition(positions, usedPositions);
                usedPositions[i] = position;
                waypoints[i] = SpawnWaypoint(position);
            }

            return waypoints;
        }

        Vector3 RandomPosition(Vector3[] positions, Vector3[] usedPositions)
        {
            if (positions.Length == 0)
                return Vector3.zero;
            
            if (positions.Length <= usedPositions.Length)
                return positions[0];
            
            Vector3 position = grid.WorldBottomLeft + positions[rand.Next(0, positions.Length)];

            for (int i = 0; i < usedPositions.Length; ++i)
                if (usedPositions[i] == position)
                    return RandomPosition(positions, usedPositions);

            return position;
        }

        PatrolWaypoint SpawnWaypoint(Vector3 position) => MonoBehaviour.Instantiate(prefab, position, prefab.transform.rotation);
    }
}