#if UNITY_IOS && !UNITY_EDITOR
using AOT;
using System.Runtime.InteropServices;

#endif

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MotionAI.Core.Models;
using MotionAI.Core.Models.Generated;
using MotionAI.Core.POCO;
using MotionAI.Core.Util;
using UnityEngine;
using static MotionAI.Core.POCO.UtilHelper;

namespace MotionAI.Core.Controller {
    public class MotionAIManager : Singleton<MotionAIManager> {
        #region Bridge Methods

        #region Internal Load

#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void InitEvomoBridge(UnityCallback callback, string licenseID, string debugging);

    [DllImport("__Internal")]
    private static extern void StartEvomoBridge(string deviceOrientation, string classificationModel, string gaming);

    [DllImport("__Internal")]
    private static extern void StopEvomoBridge();

    [DllImport("__Internal")]
    private static extern void LogEventBridge(string eventType, string note);

    [DllImport("__Internal")]
    private static extern void LogTargetMovementBridge(string movementType, string note);

    [DllImport("__Internal")]
    private static extern void LogFailureBridge(string source, string failureType, string movementType, string note);

    [DllImport("__Internal")]
    private static extern void SetUsernameBridge(string username);

    [DllImport("__Internal")]
    private static extern void SendGameHubMessageBridge(string message);

#endif

        #endregion

        #endregion

        #region Bridge Methods

#if UNITY_IOS && !UNITY_EDITOR
    [MonoPInvokeCallback(typeof(UnityCallback))]
    private static void MessageReceived(string message)
    {
        // MAIHelper.Log($"Message: {message}");
		ManageMotion(message);
    }

#endif


        public void StartTracking()
        {
            MAIHelper.Log("Try StartTracking");
            if (controllerManager.unpairedAvailableControllers.Count == 0)
            {
                MAIHelper.Log($"StartTracking Pairing");
                foreach (MotionAIController c in controllerManager.PairedControllers) {
                    AbstractModelComponent model = c.modelManager.model;

                    MAIHelper.Log($"StartTracking StartTracking ({c.deviceOrientation.ToString()}, {model.modelName},  {(model.modelType == ModelType.gaming).ToString()})");


#if UNITY_IOS && !UNITY_EDITOR
					StartEvomoBridge(c.deviceOrientation.ToString(), model.modelName,  (model.modelType == ModelType.gaming).ToString());
#endif

#if UNITY_EDITOR

                    // Simulate Receiving data from Bridge
                    // ManageMotion('{  "deviceID" : "50DC138D-C000-4C76-B13B-3FF3C771BAFC",  "elmo" : {    "typeLabel" : "hop_single_up",    "deviceIdent" : "50DC138D-C000-4C76-B13B-3FF3C771BAFC",    "rejected" : false,    "end" : "2020-12-11T13:28:29.989",    "typeID" : 645,    "start" : "2020-12-11T13:28:29.808"  }}');

#endif
                    isTracking = true;
                }
            }
            isTracking = true;
        }

        public void StopTracking() {
#if UNITY_IOS && !UNITY_EDITOR
        StopEvomoBridge();
#endif

            isTracking = false;
            MAIHelper.Log("StopTracking");
        }

        public static void LogEvent(string eventType, string note = "") {
#if UNITY_IOS && !UNITY_EDITOR
        LogEventBridge(eventType, note);
#endif
        }

        public static void LogTargetMovement(ElmoEnum movementType, string note = "") {
#if UNITY_IOS && !UNITY_EDITOR
        LogTargetMovementBridge(movementType.ToString(), note);
#endif
        }

        public void LogFailure(EventSource source, FailureType failureType, ElmoEnum movementType,
            string note = "") {
#if UNITY_IOS && !UNITY_EDITOR
        LogFailureBridge(source.ToString(), failureType.ToString(), movementType.ToString(), note);
#endif
        }

        public void SetUsername(String username) {
#if UNITY_IOS && !UNITY_EDITOR
        SetUsernameBridge(username);
#endif
        }

        public void SendGameHubMessage(String message) {
#if UNITY_IOS && !UNITY_EDITOR
        SendGameHubMessageBridge(message);
#endif
            MAIHelper.Log($"GameHubMessage: {message}");
        }

        #endregion

        #region Unity

        public delegate void UnityCallback(string value);

        private static readonly Queue<BridgeMessage> _executionQueue = new Queue<BridgeMessage>();
        public SDKConfig mySDKConfig;
        public ControllerManager controllerManager;

        public bool automaticPairing = true;
        public bool isTracking;

        [Tooltip("SDK will send some Debugging and Raw measurements to the server")]
        public bool isDebug = true;

        #region Lifecycle

        private void Awake() {

            MAIHelper.Log("Awake MotionAIManager");
            Debug.unityLogger.logEnabled = isDebug;

            var licenseID = "empty";
            if (mySDKConfig != null) {
                licenseID = mySDKConfig.licenseID;
            }
            else {
                MAIHelper.Log($"LicenseID- {licenseID}-Unknown");
            }

#if UNITY_IOS && !UNITY_EDITOR
        InitEvomoBridge(MessageReceived, licenseID, isDebug.ToString().ToLower());
#endif
            controllerManager = new ControllerManager();

            if (automaticPairing) {
                StartControlPairing();
                StartTracking();
            }
        }

        private void OnDestroy() {
            MAIHelper.Log("MotionAIManager Destroy");
            StopTracking();
        }

        public void Update() {
            lock (_executionQueue) {
                while (_executionQueue.Count > 0) {
                    BridgeMessage msg = _executionQueue.Dequeue();
                    // MAIHelper.Log($"Update - isTracking: {isTracking}");
                    if (isTracking) {
                        StartCoroutine("ProcessMotionMessage", msg);
                    }
                }
            }
        }

        #endregion


        public void Enqueue(BridgeMessage msg) {
            lock (_executionQueue) {
                _executionQueue.Enqueue(msg);
            }
        }

        public static void ManageMotion(string message) {
            // MAIHelper.Log($"BridgeMessage {message}");

            if (string.IsNullOrEmpty(message)) return;
            BridgeMessage msg = JsonUtility.FromJson<BridgeMessage>(message);
            // MAIHelper.Log($"BridgeMessage {msg.deviceID}");
            MotionAIManager.Instance.Enqueue(msg);
        }

        private IEnumerator ProcessMotionMessage(BridgeMessage msg) {

            if (msg.elmo.typeLabel != null) {
                EvoMovement mv = new EvoMovement();
                mv.deviceID = msg.deviceID;
                mv.typeLabel = msg.elmo.typeLabel;
                mv.elmos.Add(msg.elmo);
                controllerManager.ManageMotion(mv);
                yield break;
            }

            if (msg.movement.typeLabel != null) {
                msg.movement.deviceID = msg.deviceID;
                controllerManager.ManageMotion(msg.movement);
                yield break;
            }

            if (msg.message != null) MAIHelper.Log($"{msg.message.statusCode} - {msg.message.data}");

        }


        public void StartControlPairing() {
            controllerManager.PairController(FindObjectsOfType<MotionAIController>().ToList());
        }

        #endregion
    }
}
