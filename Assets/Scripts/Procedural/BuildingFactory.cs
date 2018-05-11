using UnityEngine;
using Simulation.AI.AStar;
using Simulation.Environment;
using System.Collections.Generic;

namespace Simulation.Procedural
{
    class BuildingFactory
    {
        static System.Random rand = new System.Random();

        Building[] prefabs;
        PressurePlate pressurePlatePrefab;
        AI.AStar.Grid grid;

        public BuildingFactory(Building[] prefabs, PressurePlate pressurePlatePrefab, AI.AStar.Grid grid)
        {
            this.prefabs = prefabs;
            this.pressurePlatePrefab = pressurePlatePrefab;
            this.grid = grid;
        }

        public Building Spawn(Vector3 position, Quaternion rotation)
        {
            Building prefab = RandomPrefab();

            if (prefab == null)
                throw new System.NullReferenceException();
            
            var building = MonoBehaviour.Instantiate(prefab, position, rotation);
            building.PressurePlate = MonoBehaviour.Instantiate(pressurePlatePrefab, position, pressurePlatePrefab.transform.rotation);

            return building;
        }

        public Building[] SpawnBuildings(int count)
        {
            var buildings = new List<Building>();
            var positions = new List<Vector3>();

            for (int i = 0; i < count; ++i) {
                Building prefab = RandomPrefab();

                var position = Vector3.zero;
                int tries = 0;
                bool failed = false;

                do {
                    if (tries >= 50) {
                        failed = true;
                        break;
                    }

                    tries++;
                    position = grid.RandomInnerNode(0.15f).Position;
                } while (PositionIsInvalid(position, positions.ToArray(), prefab.Width));

                if (failed)
                    continue;

                positions.Add(position);
                buildings.Add(Spawn(position, RandomRotation()));
            }

            return buildings.ToArray();
        }

        static bool PositionIsInvalid(Vector3 position, Vector3[] positions, float buildingWidth)
        {
            for (int i = 0; i < positions.Length; ++i) {
                if (Vector3.Distance(position, positions[i]) <= buildingWidth * 2)
                    return true;
            }

            return false;
        }

        Building RandomPrefab()
        {
            if (prefabs.Length == 0)
                return null;
            
            return prefabs[rand.Next(0, prefabs.Length)];
        }

        Quaternion RandomRotation()
        {
            switch (rand.Next(0, 4)) {
                case 0:
                    return Quaternion.Euler(0, 0, 0);
                case 1:
                    return Quaternion.Euler(0, 90, 0);
                case 2:
                    return Quaternion.Euler(0, 180, 0);
                case 3:
                    return Quaternion.Euler(0, 270, 0);
            }

            return Quaternion.Euler(0, 0, 0);
        }
    }
}