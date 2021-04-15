using System;
using System.Linq;
using MotionAI.Core.Controller;
using MotionAI.Core.Models.Generated;
using MotionAI.Core.POCO;
using MotionAI.Core.Util;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace MotionAI.Samples.CoreDemo {
	[RequireComponent(typeof(MotionAIManager))]
	public class EvomoDemoManager : MonoBehaviour {
		public Text DebugText;

		public Text startTrackingButton;


		private EvoMovement lastEvoMovement;
		[HideInInspector] public MotionAIManager maim;

		public InputField textField;

		private void Start() {
			textField.onEndEdit.AddListener(MotionAIManager.ManageMotion);
			maim = GetComponent<MotionAIManager>();
			maim.LogFailure(UtilHelper.EventSource.app, UtilHelper.FailureType.toLess, ElmoEnum.duck_down,
				"abc");
			maim.SetUsername("testUser");
			// maim.controllerManager.pairedEvent.AddListener(onControllerPaired);
			// maim.controllerManager.onMovement.AddListener(onMovement);
		}


		public void TrackHandle() {
			if (!maim.isTracking) {
				maim.StartTracking();
				startTrackingButton.text = "Stop Tracking";
			}
			else {
				maim.StopTracking();
				startTrackingButton.text = "Start Tracking";
			}
		}

		public void SendDebugMovementString() {
			MotionAIController maic = maim.controllerManager.PairedControllers.First();

			MoveHolder mv = maic.modelManager.model.GetMoveHolders().RandomElement();

			EvoMovement dto = new EvoMovement();
			ElementalMovement elmo = new ElementalMovement();

			dto.amplitude = Random.Range(1, 10);
			dto.durationNegative = Random.Range(1, 10);
			dto.gVelAmplitudeNegative = Random.Range(1, 10);
			dto.gVelAmplitudePositive = Random.Range(1, 10);
			dto.durationPositive = Random.Range(1, 10);

			dto.typeLabel = Enum.GetNames(typeof(MovementEnum)).ToList().RandomElement();
			dto.typeID = (MovementEnum) Enum.Parse(typeof(MovementEnum), dto.typeLabel);
			dto.elmos.Add(elmo);

			BridgeMessage bm = new BridgeMessage();
			
			bm.elmo = new ElementalMovement();
			bm.movement = dto;
			bm.message = new Message();
			textField.SetTextWithoutNotify(JsonUtility.ToJson(bm,true));
			MotionAIManager.Instance.Enqueue(bm);
			// maim.controllerManager.ManageMotion(dto);
		}


		public void StartPairController() {
			maim.StartControlPairing();
		}


		public void onMovement(EvoMovement mv) {
			DebugText.text = JsonUtility.ToJson(mv, true);
			lastEvoMovement = mv;
		}
		
		public void onScanResult(EvoMovement mv) {
			DebugText.text = JsonUtility.ToJson(mv, true);
			lastEvoMovement = mv;
		}

		public void OnElmo(ElementalMovement mv) {
			
			DebugText.text = JsonUtility.ToJson(mv, true);
		}

		public void onControllerPaired(MotionAIController c) {
			DebugText.text = $"Device with id {c.DeviceId} was paired";
		}
	}
}