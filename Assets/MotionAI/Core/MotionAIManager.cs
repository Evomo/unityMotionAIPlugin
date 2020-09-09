#if UNITY_IOS && !UNITY_EDITOR
using AOT;
#endif
using System;
using System.Linq;
using MotionAI.Core.Controller;
using MotionAI.Core.POCO;
using UnityEngine;
using static MotionAI.UtilHelper;

namespace MotionAI.Core {
	public class MotionAIManager : MonoBehaviour {
		#region Internal Load

#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void InitEvomoBridge(string licenseID);

    [DllImport("__Internal")]
    private static extern void StartEvomoBridge(string deviceOrientation, string classificationModel);

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


		#region Bridge Methods

#if UNITY_IOS && !UNITY_EDITOR
    [MonoPInvokeCallback(typeof(UnityCallback))]
    private static void MessageRecived(string message)
    {
        ManageMotion(message);
    }

#endif


		public void StartTracking() {
#if UNITY_IOS && !UNITY_EDITOR
        StartEvomoBridge(UtilHelper.ToCustomOrientation(Input.deviceOrientation), mySDKConfig.classificationModel.ToString;
#endif
		}

		public void StopTracking() {
#if UNITY_IOS && !UNITY_EDITOR
        StopEvomoBridge();
#endif
		}

		public static void LogEvent(string eventType, string note = "") {
#if UNITY_IOS && !UNITY_EDITOR
        LogEventBridge(eventType, note);
#endif
		}

		public static void LogTargetMovement(MovementType movementType, string note = "") {
#if UNITY_IOS && !UNITY_EDITOR
        LogTargetMovementBridge(movementType.ToString(), note);
#endif
		}

		public void LogFailure(EventSource source, FailureType failureType, MovementType movementType,
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

		public SDKConfig mySDKConfig;
		public ControllerManager controllerManager;

		#region Lifecycle

		private void Awake() {
#if UNITY_IOS && !UNITY_EDITOR
        SetUsernameBridge(mySDKConfig.username);
#endif
			controllerManager = new ControllerManager();
		}

		private void OnEnable() {
#if UNITY_IOS && !UNITY_EDITORz
        InitEvomoBridge(mySDKConfig.licenseID);
#endif
		}


		private void OnDisable() {
			StopTracking();
		}

		#endregion


		public void ManageMotion(string movementStr) {
			BridgeMessage msg = JsonUtility.FromJson<BridgeMessage>(movementStr);


			if (msg.movement == null) {
			
				Movement mv = new Movement();
				mv.elmos.Add(msg.elmo);
				controllerManager.ManageMotion(mv);
			}
			else {
				controllerManager.ManageMotion(msg.movement);
			}
		}


		public void ControlPairing() {
			controllerManager.PairController(FindObjectsOfType<MotionAIController>().ToList());
		}

		#endregion
	}
}