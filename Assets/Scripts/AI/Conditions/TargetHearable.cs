using Simulation.Core;

namespace Simulation.AI.Conditions
{
    class TargetHearable: StateCondition
    {
        override public bool Check(StateController controller, GameManager game)
        {
            var approacher = controller.GetComponent<IApproacher>();

            if (approacher == null)
                return false;
            
            return approacher.FindTargetByHearing() != null;
        }
    }
}