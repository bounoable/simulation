using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Simulation.Environment
{
	class Door: MonoBehaviour
	{
		public bool IsOpen { get; private set; } = false;
		public bool IsClosed
		{
			get { return !IsOpen; }
			private set { IsOpen = !value; }
		}

		Vector3 openedPosition;
		Vector3 closedPosition;

		[SerializeField]
		float openSpeed;

		public void Open()
		{
			if (!IsOpen)
				StartCoroutine(MoveTo(openedPosition, () => IsOpen = true));
		}

		public void Close()
		{
			if (!IsClosed)
				StartCoroutine(MoveTo(closedPosition, () => IsClosed = true));
		}

		IEnumerator MoveTo(Vector3 position, System.Action callback)
		{
			while (true) {
				if (openSpeed == 0)
					break;

				transform.position = Vector3.MoveTowards(transform.position, position, openSpeed * Time.fixedDeltaTime);

				if (transform.position == position)
					break;

				yield return new WaitForFixedUpdate();
			}

			if (callback != null)
				callback();

			yield return null;
		}

		public void Toggle()
		{
			if (IsOpen) {
				Close();
			} else {
				Open();
			}
		}

		void Awake()
		{
			closedPosition = transform.position;
			openedPosition = transform.position + Vector3.left * transform.localScale.x;
		}
	}
}
