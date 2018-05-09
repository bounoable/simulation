using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation.Environment
{
	class Building: MonoBehaviour
	{
		public Door Door => door;
		
		[SerializeField]
		Door door;

		public bool IsOpen => door.IsOpen;
	}
}
