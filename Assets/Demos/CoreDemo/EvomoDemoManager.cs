using System.Collections.Generic;
using System.Linq;
using MotionAI.Core;
using MotionAI.Core.Controller;
using MotionAI.Core.Models.Generated;
using MotionAI.Core.POCO;
using MotionAI.Core.Util;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Demos.CoreDemo {
	[RequireComponent(typeof(MotionAIManager))]
	public class EvomoDemoManager : MonoBehaviour {
		public List<string> fakeDeviceIds;
		public Text DebugText;

		public Text startTrackingButton;


		public MovementDto lastMovementDto;
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
			string fakeId = fakeDeviceIds.RandomElement();
			MotionAIController maic = maim.controllerManager.controllers[fakeId].First();

			MoveHolder mv = maic.modelManager.model.GetMoveHolders().RandomElement();

			MovementDto dto = new MovementDto();
			ElementalMovement elmo = new ElementalMovement();

			dto.typeID = mv.id;
			dto.elmos.Add(elmo);
			elmo.deviceIdent = fakeId;

			Debug.Log(mv.id.ToString());
			maim.controllerManager.ManageMotion(dto);
		}


		public void StartPairController() {
			maim.StartControlPairing();
		}


		public void onMovement(MovementDto mv) {
			DebugText.text = JsonUtility.ToJson(mv, true);
			lastMovementDto = mv;
		}

		public void OnElmo(ElementalMovement mv) {
			DebugText.text = JsonUtility.ToJson(mv, true);
		}

		public void onControllerPaired(MotionAIController c) {
			DebugText.text = $"Device with id {c.DeviceId} was paired";
		}
	}
}