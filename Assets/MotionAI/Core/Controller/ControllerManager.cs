using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MotionAI.Core.POCO;
using UnityEngine;
using UnityEngine.Events;

namespace MotionAI.Core.Controller {
	[Serializable]
	public class ControllerManager {
		private Dictionary<string, MotionAIController> _controllerDict;

		[SerializeField] private List<MotionAIController> _availableMotionControllers;


		public bool _pairingController { get; private set; }

		public OnControllerPaired onPaired;
		public OnMotion onMovement;

		public ControllerManager() {
			_controllerDict = new Dictionary<string, MotionAIController>();
			_availableMotionControllers = new List<MotionAIController>();
			onPaired = new OnControllerPaired();
			onMovement = new OnMotion();
		}

		public void PairController(List<MotionAIController> availableControllers) {
			if (!_pairingController) {
				_availableMotionControllers = availableControllers;
				_controllerDict = new Dictionary<string, MotionAIController>();
			}

			_pairingController = !_pairingController;
		}


		private void PairController(string deviceId) {
			if (!_controllerDict.ContainsKey(deviceId)) {
				MotionAIController controller = _availableMotionControllers.First();
				_availableMotionControllers.Remove(controller);
				controller.setDeviceId(deviceId);
				_controllerDict.Add(deviceId, controller);
				onPaired.Invoke(deviceId);
			}
		}

		public void ManageMotion(Movement msg) {
			Debug.Log(msg.ToString());
			if (_pairingController || _availableMotionControllers.Count > 0) {
				if (msg.elmos.Count > 0) {
					PairController(msg.elmos.First().deviceIdent);
				}
			}

			onMovement.Invoke(msg);
		}
	}
}