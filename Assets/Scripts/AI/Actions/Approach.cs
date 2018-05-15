using UnityEngine;
using Simulation.Core;

namespace Simulation.AI.Actions
{
    [CreateAssetMenu(menuName="AI/Actions/Approach")]
    class Approach: Action
    {
        override public void Run(StateController controller, GameManager game)
        {
            var approacher = controller.GetComponent<IApproacher>();

            if (approacher == null)
                return;

            INPCTarget target = approacher.FindTargetInSight();

            if (target == null)
                return;
            
            approacher.Approach(target);
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