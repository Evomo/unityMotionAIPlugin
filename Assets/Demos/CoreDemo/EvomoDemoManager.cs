using System.Collections.Generic;
using MotionAI;
using MotionAI.Core;
using MotionAI.Core.POCO;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Demos.CoreDemo {
	[RequireComponent(typeof(MotionAIManager))]
	public class EvomoDemoManager : MonoBehaviour {
		public List<string> debugMovement;
		public Text DebugText;

		public Text startTrackingButton;

		public MotionAIManager maim;

		private bool isTracking;

		private void Start() {
			maim = GetComponent<MotionAIManager>();
			maim.LogFailure(UtilHelper.EventSource.app, UtilHelper.FailureType.toLess, UtilHelper.MovementType.Duck,
				"abc");
			maim.SetUsername("testUser");
			maim.controllerManager.onPaired.AddListener(onControllerPaired);
			maim.controllerManager.onMovement.AddListener(onMovement);
		}


		public void TrackHandle() {
			isTracking = !isTracking;

			if (isTracking) {
				maim.StartTracking();
				startTrackingButton.text = "Stop Tracking";
			}
			else {
				maim.StopTracking();
				startTrackingButton.text = "Start Tracking";
			}
		}

		public void SendDebugMovementString() {
			string sentText = debugMovement[Random.Range(0, debugMovement.Count)];
			maim.ManageMotion(sentText);
		}


		public void StartPairController() {
			maim.ControlPairing();
		}


		public void onMovement(Movement mv) {
			DebugText.text = mv.ToString();
		}

		public void onControllerPaired(string deviceId) {
			Debug.Log(deviceId);
		}
	}
}