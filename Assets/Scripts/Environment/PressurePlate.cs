using System;
using UnityEngine;
using Simulation.Support;
using System.Collections;
using System.Collections.Generic;

namespace Simulation.Environment
{
	[RequireComponent(typeof(AudioSource))]
	class PressurePlate: MonoBehaviour, ICollisionObserver
	{
		public bool Triggered { get; private set; } = false;
		public PressurePlateBall Ball => ball;

		[SerializeField]
		PressurePlateBall ball;

		[SerializeField]
		float ballSpeed = 2f;

		Vector3 ballUp;
		Vector3 ballDown;

		AudioSource audioSource;

		public void Trigger()
		{
			if (Triggered)
				return;
			
			Triggered = true;
			audioSource.Play();
			StartCoroutine(AnimateBallDown());
		}

		public void NotifyCollision(Collider collider, Collision collision)
		{
			if (collider.GetComponent<PressurePlateBall>() == ball)
				Trigger();
		}

		IEnumerator AnimateBallDown()
		{
			while (true) {
				if (ball.transform.position == ballDown)
					yield break;
				
				ball.transform.position = Vector3.MoveTowards(ball.transform.position, ballDown, ballSpeed * Time.fixedDeltaTime);

				yield return new WaitForFixedUpdate();
			}
		}

		void Awake()
		{
			if (!ball)
				Destroy(gameObject);
			
			ballUp = ball.transform.position;
			ballDown = ball.transform.position - ball.transform.up * 0.25f;
			
			audioSource = GetComponent<AudioSource>();

			ball.ObserveCollisions(this);
		}
	}
}