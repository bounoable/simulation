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

		public void Open() => Open(() => {});
		public void Open(System.Action callback)
		{
			Debug.Log(openedPosition);
			if (!IsOpen)
				StartCoroutine(MoveTo(openedPosition, () => {
					IsOpen = true;
					if (callback != null)
						callback();
				}));
		}

		public void Close() => Close(() => {});
		public void Close(System.Action callback)
		{
			if (!IsClosed)
				StartCoroutine(MoveTo(closedPosition, () => {
					IsClosed = true;
					if (callback != null)
						callback();
				}));
		}

		public void Toggle()
		{
			if (IsOpen) {
				Close();
			} else {
				Open();
			}
		}

		IEnumerator MoveTo(Vector3 position, System.Action callback)
		{
			StopCoroutine("MoveTo");
			
			while (true) {
				if (openSpeed == 0)
					yield break;

				transform.position = Vector3.MoveTowards(transform.position, position, openSpeed * Time.fixedDeltaTime);

				if (transform.position == position)
					break;

				yield return new WaitForFixedUpdate();
			}

			if (callback != null)
				callback();
		}

		virtual protected void Awake()
		{
			closedPosition = transform.position;
			openedPosition = transform.position - transform.right * transform.lossyScale.x;
		}
	}
}
