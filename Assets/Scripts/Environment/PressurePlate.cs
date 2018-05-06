using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Simulation.Environment
{
	[RequireComponent(typeof(AudioSource))]
	class PressurePlate: MonoBehaviour
	{
		[SerializeField]
		float timeout = 3f;
		float cooldown = 0f;

		AudioSource audioSource;

		public void Trigger()
		{
			if (cooldown > 0)
				return;
			
			cooldown = timeout;

			audioSource.Play();
		}

		void Awake()
		{
			audioSource = GetComponent<AudioSource>();
		}

		void Update()
		{
			if (cooldown > 0) {
				cooldown -= Time.deltaTime;
			}
		}
	}
}