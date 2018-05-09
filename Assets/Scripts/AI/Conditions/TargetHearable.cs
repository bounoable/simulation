namespace Simulation.AI.Conditions
{
    class TargetHearable: StateCondition
    {
        override public bool Check(StateController controller)
        {
            var approacher = controller.GetComponent<IApproacher>();

            if (approacher == null)
                return false;
            
            return approacher.FindTargetByHearing() != null;
        }
    }
}