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
            Vector3 center = grid.WorldBottomLeft + new Vector3(grid.WorldSize.x / 2, 0, grid.WorldSize.y / 2);

            var waypoints = new PatrolWaypoint[count];

            float degrees = 0;

            for (int i = 1; i < count; ++i) {
                var rotation = Quaternion.Euler(0, degrees, 0);
                Vector3 direction = rotation * Vector3.forward;
                Vector3 position = center + direction * grid.WorldSize.x * 0.4f;

                degrees += 360f / (count - 1);

                waypoints[i] = MonoBehaviour.Instantiate(prefab, position, prefab.transform.rotation);
            }

            waypoints[0] = MonoBehaviour.Instantiate(prefab, center, prefab.transform.rotation);

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