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
		private Dictionary<string, MotionAIController> _controllers;

		[SerializeField] private List<MotionAIController> _availableMotionControllers;


		public bool _pairingController { get; private set; }

		public ControllerPairedEvent pairedEvent;
		public OnMovementEvent onMovement;

		public ControllerManager() {
			_controllers = new Dictionary<string, MotionAIController>();
			_availableMotionControllers = new List<MotionAIController>();
			pairedEvent = new ControllerPairedEvent();
			onMovement = new OnMovementEvent();
			
		}

		public void PairController(List<MotionAIController> availableControllers) {
			if (!_pairingController) {
				_availableMotionControllers = availableControllers;
				_controllers = new Dictionary<string, MotionAIController>();
			}

			_pairingController = !_pairingController;
		}


		private void PairController(string deviceId) {
			if (!_controllers.ContainsKey(deviceId)) {
				MotionAIController controller = _availableMotionControllers.First();
				_availableMotionControllers.Remove(controller);
				controller.setDevice(deviceId, onMovement);
				_controllers.Add(deviceId, controller);
				pairedEvent.Invoke(deviceId);
			}
		}

		public void ManageMotion(Movement msg) {
			Debug.Log(JsonUtility.ToJson(msg, true));
			if (msg.elmos.Count > 0) {
				string dID = msg.elmos.First().deviceIdent;
				if (_pairingController || _availableMotionControllers.Count > 0) {
					PairController(dID);
				}
				
				onMovement.Invoke(msg);
			}
		}
	}
}