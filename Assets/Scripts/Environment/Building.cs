using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation.Environment
{
	class Building: MonoBehaviour
	{
		public Vector3 Center => transform.position;
		
		public Door Door => door;
		public bool IsOpen => door.IsOpen;
		public float Width => width;
		public PressurePlate PressurePlate { get; set; }
		
		[SerializeField]
		Door door;

		[SerializeField]
		float width;
	}
}
