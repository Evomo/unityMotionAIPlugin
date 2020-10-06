﻿using System.Linq;
using MotionAI.Core.Controller;
using MotionAI.Core.Models.Generated;
using MotionAI.Core.POCO;
using MotionAI.Core.Util;
using UnityEngine;
using UnityEngine.UI;

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
			MotionAIController maic = maim.controllerManager.PairedControllers.First();

			MoveHolder mv = maic.modelManager.model.GetMoveHolders().RandomElement();

			EvoMovement dto = new EvoMovement();
			ElementalMovement elmo = new ElementalMovement();

			dto.typeID = mv.id;
			dto.elmos.Add(elmo);
			// elmo.deviceIdent = "global";

			// Debug.Log(mv.id.ToString());
			BridgeMessage bm = new BridgeMessage();
			bm.movement = dto;
			MotionAIManager.ManageMotion(JsonUtility.ToJson(bm));
			// maim.controllerManager.ManageMotion(dto);
		}


		public void StartPairController() {
			maim.StartControlPairing();
		}


		public void onMovement(EvoMovement mv) {
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