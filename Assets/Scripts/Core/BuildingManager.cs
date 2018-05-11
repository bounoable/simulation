using System;
using UnityEngine;
using Simulation.AI;
using System.Collections;
using Simulation.Procedural;
using Simulation.Environment;
using System.Collections.Generic;

namespace Simulation.Core
{
    class BuildingManager
    {
        public HashSet<Building> Buildings { get; private set; } = new HashSet<Building>();

        BuildingFactory factory;

        public BuildingManager(BuildingFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException();

            this.factory = factory;
        }

        public Building Spawn(Vector3 position, Quaternion rotation)
        {
            Building building = factory.Spawn(position, rotation);

            Buildings.Add(building);

            return building;
        }

        public Building[] SpawnBuildings(int count)
        {
            Building[] buildings = factory.SpawnBuildings(count);

            for (int i = 0; i < buildings.Length; ++i) {
                Buildings.Add(buildings[i]);
            }

            return buildings;
        }

        public void UpdateBuildings(GameManager game)
        {
            bool doorToggled = false;

            foreach (Building building in Buildings) {
                if (building.Door is AutomaticDoor) {
                    var door = building.Door as AutomaticDoor;

                    door.AutoOpenClose(game, ref doorToggled);
                }

                if (building.Door is ManualDoor) {
                    var door = building.Door as ManualDoor;

                    if (door.WasRecentlyOpened) {
                        doorToggled = true;
                        door.WasRecentlyOpened = false;
                    }
                }
            }

            if (doorToggled)
                game.Grid.RecreateGrid();
        }
    }
}