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

        public Building[] SpawnBuildings()
        {
            Building[] buildings = factory.SpawnBuildings();

            for (int i = 0; i < buildings.Length; ++i) {
                Buildings.Add(buildings[i]);
            }

            return buildings;
        }

        public void DestroyBuildings()
        {
            foreach (Building building in Buildings)
                MonoBehaviour.Destroy(building.gameObject);
            
            Buildings.Clear();
        }

        public void UpdateBuildings(GameManager game)
        {
            bool doorToggled = false;
            bool pressurePlatesTriggered = true;

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

                if (!building.PressurePlate.Triggered)
                    pressurePlatesTriggered = false;
            }

            if (doorToggled)
                game.Grid.RecreateGrid();
            
            if (pressurePlatesTriggered)
                game.CreateMaze();
        }
    }
}