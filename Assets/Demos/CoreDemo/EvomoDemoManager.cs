using System.Collections.Generic;
using MotionAI;
using MotionAI.Core;
using MotionAI.Core.Controller;
using MotionAI.Core.POCO;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Demos.CoreDemo {
	[RequireComponent(typeof(MotionAIManager))]
	public class EvomoDemoManager : MonoBehaviour {
		public List<string> fakeDeviceIds;
		public Text DebugText;

		public Text startTrackingButton;


		public Movement lastMovement;
		public MotionAIManager maim;


		private void Start() {
			maim = GetComponent<MotionAIManager>();
			maim.LogFailure(UtilHelper.EventSource.app, UtilHelper.FailureType.toLess, ElmoEnum.duck_down,
				"abc");
			maim.SetUsername("testUser");
			maim.controllerManager.pairedEvent.AddListener(onControllerPaired);
			maim.controllerManager.onMovement.AddListener(onMovement);
		}


		public void TrackHandle() {
			if (!maim.IsTracking) {
				maim.StartTracking();
				startTrackingButton.text = "Stop Tracking";
			}
			else {
				maim.StopTracking();
				startTrackingButton.text = "Start Tracking";
			}
		}

		public void SendDebugMovementString() {
			string debugId = fakeDeviceIds[Random.Range(0, fakeDeviceIds.Count)];

			Movement m = new Movement {gVelAmplitudeNegative = Random.Range(0, 10), amplitude = Random.Range(0, 10)};


			int amountOfElmos = Random.Range(0, 5);
			List<ElementalMovement> fakeElmos = new List<ElementalMovement>();
			for (int i = 0; i < amountOfElmos; i++) {
				ElementalMovement e = new ElementalMovement {deviceIdent = debugId};
				fakeElmos.Add(e);
			}

			m.elmos = fakeElmos;
			maim.controllerManager.ManageMotion(m);
		}


		public void StartPairController() {
			maim.ControlPairing();
		}


		public void onMovement(Movement mv) {
			DebugText.text = JsonUtility.ToJson(mv, true);
			lastMovement = mv;
		}

		public void OnElmo(ElementalMovement mv) {
			DebugText.text = JsonUtility.ToJson(mv, true);
		}

		public void onControllerPaired(MotionAIController c) {
			DebugText.text = $"Device with id {c.DeviceId} was paired";
		}
	}
}