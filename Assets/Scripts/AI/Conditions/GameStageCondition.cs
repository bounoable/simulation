using UnityEngine;
using Simulation.AI;
using Simulation.Core;
using System.Collections;
using System.Collections.Generic;

namespace Simulation.AI.Conditions
{
	[CreateAssetMenu(menuName="AI/Conditions/GameStageCondition")]
    class GameStageCondition: StateCondition
	{
		[SerializeField]
		GameStage.Stage stage;

		override public bool Check(StateController controller, GameManager game) => game.CurrentStage.StageName == stage;
	}
}
