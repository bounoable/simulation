using System;
using UnityEngine;
using Simulation.AI;
using Simulation.Procedural;
using Simulation.Environment;
using System.Collections.Generic;

namespace Simulation.Core
{
    class BuildingManager
    {
        BuildingFactory factory;

        HashSet<Building> buildings = new HashSet<Building>();

        public BuildingManager(BuildingFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException();

            this.factory = factory;
        }

        public Building Spawn(Vector3 position)
        {
            Building building = factory.Spawn(position);

            buildings.Add(building);

            return building;
        }

        public void UpdateBuildings(GameManager game)
        {
            foreach (Building building in buildings) {
                if (building.Door is AutomaticDoor) {
                    var door = building.Door as AutomaticDoor;

                    door.OpenIfApproached(game);
                }
            }
        }
    }
}