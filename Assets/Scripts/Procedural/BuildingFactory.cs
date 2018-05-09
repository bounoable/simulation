using UnityEngine;
using Simulation.Environment;

namespace Simulation.Procedural
{
    class BuildingFactory
    {
        static System.Random rand = new System.Random();

        Building[] prefabs;

        public BuildingFactory(Building[] prefabs)
        {
            this.prefabs = prefabs;
        }

        public Building Spawn(Vector3 position)
        {
            Building prefab = RandomPrefab();

            if (prefab == null)
                throw new System.NullReferenceException();
            
            return MonoBehaviour.Instantiate<Building>(prefab, position, prefab.transform.rotation);
        }

        Building RandomPrefab()
        {
            if (prefabs.Length == 0)
                return null;
            
            return prefabs[rand.Next(0, prefabs.Length)];
        }
    }
}