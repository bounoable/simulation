using System.Collections;
using Simulation.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation.AI
{
    [System.Serializable]
    class Transition
    {
        public enum ConditionsType
        {
            ALL,
            EITHER
        }

        [SerializeField]
        StateCondition[] conditions;

        [SerializeField]
        State onSuccess;

        [SerializeField]
        State onFail;

        [SerializeField]
        ConditionsType conditionsType;

        public State GetNextState(StateController controller, GameManager game)
        {
            switch (conditionsType) {
                case ConditionsType.ALL:
                    for (int i = 0; i < conditions.Length; ++i)
                        if (!conditions[i].Check(controller, game))
                            return onFail;
                    
                    return onSuccess;

                case ConditionsType.EITHER:
                    for (int i = 0; i < conditions.Length; ++i)
                        if (conditions[i].Check(controller, game))
                            return onSuccess;
                    break;
            }

            return onFail;
        }
    }
}
