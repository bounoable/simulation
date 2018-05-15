using UnityEngine;
using Simulation.Core;

namespace Simulation.AI.Actions
{
    [CreateAssetMenu(menuName="AI/Actions/Patrol")]
    class Patrol: Action
    {
        override public void Run(StateController controller, GameManager game)
        {
            var patroler = controller.GetComponent<IPatroling>();

            if (patroler == null)
                return;
            
            patroler.Patrol(game.Grid);
        }

        override public void Stop(StateController controller, GameManager game)
        {
            var patroler = controller.GetComponent<IPatroling>();

            if (patroler == null)
                return;
            
            patroler.StopPatrol();
        }
    }
}