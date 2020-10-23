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

#endif

		#endregion

		#endregion

		#region Bridge Methods

#if UNITY_IOS && !UNITY_EDITOR
    [MonoPInvokeCallback(typeof(UnityCallback))]
    private static void MessageReceived(string message)
    {
        // Debug.Log($"Message: {message}");
		ManageMotion(message);
    }

#endif


		public void StartTracking() {
			if (controllerManager.unpairedAvailableControllers.Count == 0) {
				foreach (MotionAIController c in controllerManager.PairedControllers) {
					AbstractModelComponent model = c.modelManager.model;

#if UNITY_IOS && !UNITY_EDITOR
					StartEvomoBridge(c.deviceOrientation.ToString(), model.chosenBuild.modelName,  (model.modelType == ModelType.gaming).ToString())
#endif
					isTracking = true;
				}
			}
		}

		public void StopTracking() {
#if UNITY_IOS && !UNITY_EDITOR
        StopEvomoBridge();
#endif

			isTracking = false;
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
			Debug.unityLogger.logEnabled = isDebug;
#if UNITY_IOS && !UNITY_EDITOR
        InitEvomoBridge(MessageReceived, mySDKConfig.licenseID, isDebug.ToString().ToLower());
#endif
			controllerManager = new ControllerManager();

			if (automaticPairing) {
				StartControlPairing();
				StartTracking();
			}
		}

		private void OnDestroy() {
			StopTracking();
		}

		public void Update() {
			lock (_executionQueue) {
				while (_executionQueue.Count > 0) {
					if (isTracking) {
						BridgeMessage msg = _executionQueue.Dequeue();
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
			if (string.IsNullOrEmpty(message)) return;
			BridgeMessage msg = JsonUtility.FromJson<BridgeMessage>(message);
			MotionAIManager.Instance.Enqueue(msg);
		}

		private IEnumerator ProcessMotionMessage(BridgeMessage msg) {
			if (msg.elmo.typeLabel != null) {
				EvoMovement mv = new EvoMovement();
				mv.deviceID = msg.deviceID;
				mv.elmos.Add(msg.elmo);
				controllerManager.ManageMotion(mv);
				yield break;
			}

			if (msg.movement.typeLabel != null) {
				msg.movement.deviceID = msg.deviceID;
				controllerManager.ManageMotion(msg.movement);
				yield break;
			}

			Debug.Log($"EvomoUnitySDK-Message: {msg.message.statusCode} - {msg.message.data}");
		}


		public void StartControlPairing() {
			controllerManager.PairController(FindObjectsOfType<MotionAIController>().ToList());
		}

		#endregion
	}
}