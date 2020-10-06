using System.Collections;
using MotionAI.Core.Controller;
using MotionAI.Core.Models;
using MotionAI.Core.POCO;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Demos.CoreDemo {
	public class DemoController : MotionAIController {
		public ParticleSystem ps;


		public override void Start() {
			ps = GetComponent<ParticleSystem>();
			var mainModule = ps.main;
			mainModule.startColor = Random.ColorHSV();
		}

		protected override void HandleMovement(EvoMovement msg) {
			var velocityOverLifetimeModule = ps.velocityOverLifetime;
			velocityOverLifetimeModule.xMultiplier = msg.elmos.Count;
			velocityOverLifetimeModule.yMultiplier = msg.elmos.Count;
			ps.Play();
		}
	}
}