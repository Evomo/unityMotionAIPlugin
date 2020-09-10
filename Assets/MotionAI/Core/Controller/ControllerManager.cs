using System;
using System.Collections.Generic;
using System.Linq;
using MotionAI.Core.POCO;
using UnityEngine;

namespace MotionAI.Core.Controller {
	[Serializable]
	public class ControllerManager {
		public Dictionary<string, HashSet<MotionAiController>> controllers;

		public List<MotionAiController> unpairedAvailableControllers;


		public int AmountOfPairedControllers {
			get { return controllers.Values.Aggregate(0, (total, cList) => total + cList.Count()); }
		}

		public bool PairingController { get; private set; }

		[HideInInspector] public ControllerPairedEvent pairedEvent;
		[HideInInspector] public ControllerPairedEvent unpairedEvent;

		[HideInInspector] public OnMovementEvent onMovement;

		public ControllerManager() {
			controllers = new Dictionary<string, HashSet<MotionAiController>>();
			unpairedAvailableControllers = new List<MotionAiController>();
			pairedEvent = new ControllerPairedEvent();
			onMovement = new OnMovementEvent();
		}


		public void PairController(List<MotionAiController> availableControllers) {
			if (!PairingController) {
				controllers = new Dictionary<string, HashSet<MotionAiController>>();

				unpairedAvailableControllers = availableControllers
					.Select(c => {
						if (c.isGlobal) {
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
				MotionAiController controller = unpairedAvailableControllers.First();
				unpairedAvailableControllers.Remove(controller);
				PairController(deviceId, controller);
			}
		}

		private void PairController(string deviceId, MotionAiController controller) {
			controller.SetDevice(deviceId, onMovement);
			HashSet<MotionAiController> cSet;
			if (controllers.ContainsKey(deviceId)) {
				cSet = controllers[deviceId];
			}
			else {
				cSet = new HashSet<MotionAiController>();
				controllers.Add(deviceId, cSet);
			}

			cSet?.Add(controller);
			pairedEvent?.Invoke(controller);
		}

		public void UnpairControllers() {
			foreach (KeyValuePair<string, HashSet<MotionAiController>> entry in controllers) {
				List<MotionAiController> copy = entry.Value.ToList();
				foreach (MotionAiController c in copy) {
					c.Unpair();
					entry.Value.Remove(c);
					unpairedEvent?.Invoke(c);
				}
			}
		}

		public void ManageMotion(Movement msg) {
			// Debug.Log(JsonUtility.ToJson(msg, true));
			if (msg.elmos?.Count > 0) {
				string dID = msg.elmos.First().deviceIdent;
				if (PairingController || unpairedAvailableControllers.Count > 0) {
					PairController(dID);
				}

				onMovement.Invoke(msg);
			}
		}
	}
}