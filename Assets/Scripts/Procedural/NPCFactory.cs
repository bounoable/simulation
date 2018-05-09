using UnityEngine;
using Simulation.AI;

namespace Simulation.Procedural
{
    class NPCFactory
    {
        NPC prefab;
        PathRequestManager pathRequestManager;

        public NPCFactory(NPC prefab, PathRequestManager pathRequestManager)
        {
            if (prefab == null || pathRequestManager == null)
                throw new System.ArgumentNullException();
            
            this.prefab = prefab;
            this.pathRequestManager = pathRequestManager;
        }

        public NPC Spawn(Vector3 position)
        {
            var npc = MonoBehaviour.Instantiate<NPC>(prefab, position, prefab.transform.rotation);

            npc.PathRequestManager = pathRequestManager;

            return npc;
        }
    }
}