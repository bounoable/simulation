using UnityEngine;

namespace Simulation.Core
{
    [CreateAssetMenu(menuName="Stages/AI")]
    class AIStage: GameStage
    {
        override public void Update()
        {
            if (!Game)
                return;
        }

        override public void FixedUpdate()
        {
            if (!Game)
                return;
            
            Game.BuildingManager.UpdateBuildings(Game);
        }
    }
}