using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation.Environment
{
	class Door: MonoBehaviour
	{
		public bool IsOpen { get; private set; } = false;
		public bool IsClosed => !IsOpen;

		public void Open()
		{
			IsOpen = true;
		}

		public void Close()
		{
			IsOpen = false;
		}

		public void Toggle()
		{
			if (IsOpen) {
				Close();
			} else {
				Open();
			}
		}
	}
}
