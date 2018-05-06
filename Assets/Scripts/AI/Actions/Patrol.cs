using UnityEngine;

namespace Simulation.AI.Actions
{
    [CreateAssetMenu(menuName="AI/Actions/Patrol")]
    class Patrol: Action
    {
        override public void Run(StateController controller)
        {
            var patroling = controller.GetComponent<IPatroling>();

            if (patroling == null)
                return;
            
            patroling.Patrol();
        }

        override public void Stop(StateController controller)
        {
            var patroling = controller.GetComponent<IPatroling>();

            if (patroling == null)
                return;
            
            patroling.StopPatrol();
        }
    }
}