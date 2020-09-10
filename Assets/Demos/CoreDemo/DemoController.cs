using MotionAI.Core.Controller;
using MotionAI.Core.POCO;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Demos.CoreDemo {
	public class DemoController : MotionAIController {

		public ParticleSystem ps;
		private void Start() {
			ps = GetComponent<ParticleSystem>();
			var mainModule = ps.main;
			mainModule.startColor = Random.ColorHSV();
		}

		protected override void HandleMovement(Movement msg) {
			var emissionModule = ps.emission;
			var velocityOverLifetimeModule = ps.velocityOverLifetime;
			emissionModule.burstCount = (int)msg.amplitude * 7;
			velocityOverLifetimeModule.xMultiplier = msg.elmos.Count;
			velocityOverLifetimeModule.yMultiplier = msg.elmos.Count;
			ps.Play();
		}
	}
}