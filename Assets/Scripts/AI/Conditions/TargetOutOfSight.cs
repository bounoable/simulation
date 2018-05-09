using UnityEngine;

namespace Simulation.AI.Conditions
{
    [CreateAssetMenu(menuName="AI/Conditions/TargetOutOfSight")]
    class TargetOutOfSight: StateCondition
    {
        override public bool Check(StateController controller)
        {
            var approacher = controller.GetComponent<IApproacher>();
            
            return approacher != null && approacher.FindTargetInSight() == null;
        }
    }
}