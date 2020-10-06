using System;
using System.Collections.Generic;
using System.Linq;
using MotionAI.Core.POCO;
using UnityEngine;

namespace MotionAI.Core.Controller {
	[Serializable]
	public class ControllerManager {
		public Dictionary<string, HashSet<MotionAIController>> controllers;

		public List<MotionAIController> unpairedAvailableControllers;

		public List<MotionAIController> PairedControllers {
			get {
				List<MotionAIController> c = new List<MotionAIController>();

				foreach (HashSet<MotionAIController> hashController in controllers.Values) {
					c.AddRange(hashController);
				}

				return c;
			}
		}

		public bool PairingController { get; private set; }

		[HideInInspector] public ControllerPairedEvent pairedEvent;
		[HideInInspector] public ControllerPairedEvent unpairedEvent;
		[HideInInspector] public OnMovementEvent onMovement;

		public ControllerManager() {
			controllers = new Dictionary<string, HashSet<MotionAIController>>();
			unpairedAvailableControllers = new List<MotionAIController>();
			pairedEvent = new ControllerPairedEvent();
			onMovement = new OnMovementEvent();
		}


		public void PairController(List<MotionAIController> availableControllers) {
			if (!PairingController) {
				controllers = new Dictionary<string, HashSet<MotionAIController>>();

				unpairedAvailableControllers = availableControllers
					.Select(c => {
						if (c.IsGlobal) {
							PairController("global", c);
						}

						return c;
					})
					.Where(controller => controller.IsPaired == false).ToList();
			}

			PairingController = !PairingController;
		}


		private void PairController(string deviceId) {
			if (!controllers.ContainsKey(deviceId)) {
				MotionAIController controller = unpairedAvailableControllers.First();
				unpairedAvailableControllers.Remove(controller);
				PairController(deviceId, controller);
			}
		}

		private void PairController(string deviceId, MotionAIController controller) {
			HashSet<MotionAIController> cSet;
			if (controllers.ContainsKey(deviceId)) {
				cSet = controllers[deviceId];
			}
			else {
				cSet = new HashSet<MotionAIController>();
				controllers.Add(deviceId, cSet);
			}

			cSet?.Add(controller);
			controller.SetDevice(deviceId, onMovement);

			pairedEvent?.Invoke(controller);
		}

		public void UnpairControllers() {
			foreach (KeyValuePair<string, HashSet<MotionAIController>> entry in controllers) {
				List<MotionAIController> copy = entry.Value.ToList();
				foreach (MotionAIController c in copy) {
					c.Unpair();
					entry.Value.Remove(c);
					unpairedEvent?.Invoke(c);
				}
			}
		}

		public void ManageMotion(EvoMovement msg) {
			if (unpairedAvailableControllers.Count == 0) {
				PairingController = false;
			}

			if ((PairingController || unpairedAvailableControllers.Count > 0) && msg.deviceID != null) {
				PairController(msg.deviceID);
			}

			onMovement.Invoke(msg);
		}
	}
}