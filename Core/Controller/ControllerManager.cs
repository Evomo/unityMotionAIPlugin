using System;
using System.Collections.Generic;
using System.Linq;
using MotionAI.Core.POCO;
using MotionAI.Core.Util;
using UnityEngine;

namespace MotionAI.Core.Controller {
    [Serializable]
    public class ControllerManager {
        public Dictionary<string, HashSet<MotionAIController>> controllers;


        [ShowOnly] public List<MotionAIController> unpairedAvailableControllers;
        [SerializeField, ShowOnly] private List<MotionAIController> pairedControllers;

        public List<MotionAIController> PairedControllers => pairedControllers;

        public bool PairingController { get; private set; }

        [HideInInspector] public ControllerPairedEvent pairedEvent;
        [HideInInspector] public ControllerPairedEvent unpairedEvent;
        private OnMovementEvent _onMovement;

        public ControllerManager() {
            controllers = new Dictionary<string, HashSet<MotionAIController>>();
            unpairedAvailableControllers = new List<MotionAIController>();
            pairedEvent = new ControllerPairedEvent();
            _onMovement = new OnMovementEvent();
        }


        private void UpdatePairedControllerList() {
            pairedControllers = new List<MotionAIController>();

            foreach (HashSet<MotionAIController> hashController in controllers.Values) {
                pairedControllers.AddRange(hashController);
            }

            foreach (MotionAIController controller in pairedControllers) {
                MAIHelper.Log($"pairedControllersList: {controller.DeviceId}, {controller.DeviceOrientation}");
            }
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
                UpdatePairedControllerList();
            }

            PairingController = !PairingController;
        }


        private void PairController(string deviceId) {
            MAIHelper.Log($"Try to Pairing controller with deviceID: {deviceId} - controllers left: {unpairedAvailableControllers.Count}");
            if (!controllers.ContainsKey(deviceId)) {
                MotionAIController controller = unpairedAvailableControllers.First();
                unpairedAvailableControllers.Remove(controller);
                PairController(deviceId, controller);
                UpdatePairedControllerList();
            }
        }

        private void PairController(string deviceId, MotionAIController controller) {
            HashSet<MotionAIController> cSet;
            if (controllers.ContainsKey(deviceId)) {
                MAIHelper.Log($"Pairing (reuse controller) deviceID: {deviceId}");
                cSet = controllers[deviceId];
            }
            else {
                MAIHelper.Log($"Pairing (create new controller) deviceID: {deviceId}");
                cSet = new HashSet<MotionAIController>();
                controllers.Add(deviceId, cSet);
            }

            cSet?.Add(controller);
            controller.SetDevice(deviceId, _onMovement);

            pairedEvent?.Invoke(controller);
        }

        public void UnpairControllers() {
            MAIHelper.Log($"Unpairing Controllers");

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

            // MAIHelper.Log($"ControlManager - ManageMotion {msg.typeLabel} onMovementNotNull{onMovement != null}");
            _onMovement?.Invoke(msg);
        }
    }
}