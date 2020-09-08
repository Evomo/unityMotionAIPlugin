using System.Collections.Generic;
using MotionAI;
using MotionAI.Core;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Demos.CoreDemo {
	[RequireComponent(typeof(MotionAIManager))]
	public class EvomoDemoManager : MonoBehaviour {
		public List<string> debugMovement;
		public Text DebugText;

		public MotionAIManager maim;

		
		private void Awake() {
			DebugText.text = "Waiting for Evomo Init";
		}

		private void Start() {
			maim = MotionAIManager.Instance;
			maim.LogFailure(UtilHelper.EventSource.app, UtilHelper.FailureType.toLess, UtilHelper.MovementType.Duck, "abc");
			maim.SetUsername("testUser");
			maim.controllerManager.onPaired.AddListener(onControllerPaired);
		}

		public void SendDebugMessage() {
			string sentText = debugMovement[Random.Range(0, debugMovement.Count)];
			maim.ManageMotion(sentText);
			DebugText.text = sentText;
		}


		public void StartPairController() {
			maim.controllerManager.PairController();
			
		}

		public void onControllerPaired(string deviceId) {
			Debug.Log(deviceId);
		}
	}
}