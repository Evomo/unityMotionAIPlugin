using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MotionAI.Core.Controller {
	[Serializable]
	public class OnControllerPaired : UnityEvent<string> {
	}

	public class ControllerManager : MonoBehaviour {
		private Dictionary<string, MotionAIController> _controllerDict;

		[SerializeField] private Queue<MotionAIController> _availableMotionControllers;


		public bool _pairingController { get; private set; }

		public OnControllerPaired onPaired;

		private void Start() {
			_controllerDict = new Dictionary<string, MotionAIController>();
			_availableMotionControllers = new Queue<MotionAIController>();
		}

		public void PairController() {
			if (!_pairingController) {
				_availableMotionControllers = new Queue<MotionAIController>(FindObjectsOfType<MotionAIController>());
				_controllerDict = new Dictionary<string, MotionAIController>();
			}

			_pairingController = !_pairingController;
		}


		private void PairController(string deviceId) {
			if (!_controllerDict.ContainsKey(deviceId)) {
				MotionAIController controller = _availableMotionControllers.Dequeue();
				controller.setDeviceId(deviceId);
				_controllerDict.Add(deviceId, controller);
				onPaired.Invoke(deviceId);
			}
		}

		public void ManageMotion(BridgeMessage msg) {
			Debug.Log(msg.ToString());
			if (_pairingController || _availableMotionControllers.Count > 0) {
				PairController(msg.deviceId);
			}
		}
	}
}