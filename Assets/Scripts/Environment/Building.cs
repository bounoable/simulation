using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation.Environment
{
	class Building: MonoBehaviour
	{
		[SerializeField]
		Door[] doors = new Door[0];

		public bool IsOpen
		{
			get {
				for (int i = 0; i < doors.Length; ++i) {
					if (doors[i] == null)
						continue;
					
					if (doors[i].IsOpen)
						return true;
				}

				return false;
			}
		}
	}
}
