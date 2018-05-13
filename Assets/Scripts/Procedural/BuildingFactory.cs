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

        public Building Spawn(Building prefab, Vector3 position, Quaternion rotation)
        {
            if (prefab == null)
                throw new System.NullReferenceException();
            
            var building = MonoBehaviour.Instantiate(prefab, position, rotation);
            building.PressurePlate = MonoBehaviour.Instantiate(pressurePlatePrefab, position, pressurePlatePrefab.transform.rotation);

            return building;
        }

        public Building[] SpawnBuildings()
        {
            if (prefabs.Length == 0)
                return new Building[0];
            
            var buildings = new Building[4];

            var corners = new Vector3[] {
                grid.WorldBottomLeft,
                grid.WorldBottomLeft + new Vector3(0, 0, grid.WorldSize.y),
                grid.WorldBottomLeft + new Vector3(grid.WorldSize.x, 0, grid.WorldSize.y),
                grid.WorldBottomLeft + new Vector3(grid.WorldSize.x, 0, 0),
            };

            var rotations = new Quaternion[] {
                Quaternion.Euler(0, 270, 0),
                Quaternion.Euler(0, 270, 0),
                Quaternion.Euler(0, 90, 0),
                Quaternion.Euler(0, 90, 0),
            };

            for (int i = 0; i < 4; ++i) {
                Building prefab = RandomPrefab();
                Vector3 corner = corners[i];
                Quaternion rotation = rotations[i];
                float buildingSize = prefab.Width;
                Vector3 position;

                switch (i) {
                    case 0:
                        position = corner + new Vector3(buildingSize, 0, buildingSize);
                        break;
                    case 1:
                        position = corner + new Vector3(buildingSize, 0, -buildingSize);
                        break;
                    case 2:
                        position = corner + new Vector3(-buildingSize, 0, -buildingSize);
                        break;
                    case 3:
                        position = corner + new Vector3(-buildingSize, 0, buildingSize);
                        break;
                    default:
                        position = Vector3.zero;
                        break;
                }

                buildings[i] = Spawn(prefab, position, rotation);
            }

            return buildings;
        }

        Building RandomPrefab()
        {
            if (prefabs.Length == 0)
                return null;
            
            return prefabs[rand.Next(0, prefabs.Length)];
        }
    }
}