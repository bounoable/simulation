using UnityEngine;

namespace Simulation.AI.Actions
{
    [CreateAssetMenu(menuName="AI/Actions/Patrol")]
    class Patrol: Action
    {
        override public void Run(StateController controller)
        {
            var patroler = controller.GetComponent<IPatroling>();

            if (patroler == null)
                return;
            
            patroler.Patrol();
        }

        override public void Stop(StateController controller)
        {
            var patroler = controller.GetComponent<IPatroling>();

            if (patroler == null)
                return;
            
            patroler.StopPatrol();
        }
    }
}