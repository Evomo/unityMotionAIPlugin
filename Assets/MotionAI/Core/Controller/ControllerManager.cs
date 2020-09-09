using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MotionAI.Core.POCO;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace MotionAI.Core.Controller {
	[Serializable]
	public class ControllerManager {
		private Dictionary<string, HashSet<MotionAiController>> _controllers;

		[SerializeField] private List<MotionAiController> availableMotionControllers;


		public bool _pairingController { get; private set; }

		[HideInInspector] public ControllerPairedEvent pairedEvent;
		[HideInInspector] public OnMovementEvent onMovement;

		public ControllerManager() {
			_controllers = new Dictionary<string, HashSet<MotionAiController>>();
			availableMotionControllers = new List<MotionAiController>();
			pairedEvent = new ControllerPairedEvent();
			onMovement = new OnMovementEvent();
		}


		public void PairController(List<MotionAiController> availableControllers) {
			if (!_pairingController) {
				_controllers = new Dictionary<string, HashSet<MotionAiController>>();

				availableMotionControllers = availableControllers
					.Select(c => {
						if (c.isGlobal) {
							PairController("global", c);
						}

						return c;
					})
					.Where(controller => controller.isPaired == false).ToList();
			}

			_pairingController = !_pairingController;
		}


		private void PairController(string deviceId) {
			if (!_controllers.ContainsKey(deviceId)) {
				MotionAiController controller = availableMotionControllers.First();
				availableMotionControllers.Remove(controller);
				PairController(deviceId, controller);
			}
		}

		private void PairController(string deviceId, MotionAiController controller) {
			controller.SetDevice(deviceId, onMovement);
			HashSet<MotionAiController> cSet;
			if (_controllers.ContainsKey(deviceId)) {
				cSet = _controllers[deviceId];
			}
			else {
				cSet = new HashSet<MotionAiController>();
				_controllers.Add(deviceId, cSet);
			}
			cSet?.Add(controller);
			pairedEvent.Invoke(deviceId);
		}

		public void ManageMotion(Movement msg) {
			Debug.Log(JsonUtility.ToJson(msg, true));
			if (msg.elmos.Count > 0) {
				string dID = msg.elmos.First().deviceIdent;
				if (_pairingController || availableMotionControllers.Count > 0) {
					PairController(dID);
				}

				onMovement.Invoke(msg);
			}
		}
	}
}