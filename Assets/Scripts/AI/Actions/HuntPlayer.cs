using UnityEngine;
using Simulation.Core;

namespace Simulation.AI.Actions
{
    [CreateAssetMenu(menuName="AI/Actions/HuntPlayer")]
    class HuntPlayer: Action
    {
        override public void Run(StateController controller, GameManager game)
        {
            var approacher = controller.GetComponent<IApproacher>();

            if (approacher == null)
                return;
            
            approacher.Approach(game.Player);
        }

        override public void Stop(StateController controller, GameManager game)
        {
            var approacher = controller.GetComponent<IApproacher>();

            if (approacher == null)
                return;
            
            approacher.StopApproach();
        }
    }
}