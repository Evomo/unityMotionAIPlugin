#if UNITY_IOS && !UNITY_EDITOR
using AOT;
#endif
using System;
using System.Linq;
using MotionAI.Core.Models.Generated;
using MotionAI.Core.POCO;
using UnityEngine;
using static MotionAI.Core.POCO.UtilHelper;

namespace MotionAI.Core.Controller {
	public class MotionAIManager : MonoBehaviour {
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
		
		#region Bridge Methods
		
#if UNITY_IOS && !UNITY_EDITOR
    [MonoPInvokeCallback(typeof(UnityCallback))]
    private static void MessageRecived(string message)
    {
        Debug.Log($"Message: {message}");
		ManageMotion(message);
    }

#endif


		public void StartTracking() {
#if UNITY_IOS && !UNITY_EDITOR
// TODO: Add third parameter gaming - if model_type == gaming -> input_string = "true"
// TODO: Input classificationModel as string
		
        StartEvomoBridge("buttonDown", "subway-surfer", "true");
#endif
			IsTracking = true;
			StartControlPairing();
		}

		public void StopTracking() {
#if UNITY_IOS && !UNITY_EDITOR
        StopEvomoBridge();
#endif

			IsTracking = false;
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

		public static OnSDKMessage onSDKMessage = new OnSDKMessage();
		public SDKConfig mySDKConfig;
		public ControllerManager controllerManager;

		public bool automaticPairing = true;
		public bool IsTracking { get; private set; }

		#region Lifecycle

		private void Awake() {
#if UNITY_IOS && !UNITY_EDITOR 
// TODO add parameter to global Manager to define if debuggin is active (sdk will send some debugging and raw measurement data to the server)
// Enter the boolean as string like "true" and "false"
        InitEvomoBridge(MessageRecived, mySDKConfig.licenseID, "true");
        SetUsernameBridge(mySDKConfig.username);
#endif
			controllerManager = new ControllerManager();
			onSDKMessage.AddListener(ProcessMotionMessage);
			
			if (automaticPairing) {
				StartTracking();
			}
		}
		
		private void OnDestroy() {
			StopTracking();
		}

		#endregion


		public static void ManageMotion(string message) {
			onSDKMessage.Invoke(message);
		}

		private void ProcessMotionMessage(string movementStr) {
			BridgeMessage msg = JsonUtility.FromJson<BridgeMessage>(movementStr);


			if (msg.movementDto == null) {
				MovementDto mv = new MovementDto();
				mv.elmos.Add(msg.elmo);
				controllerManager.ManageMotion(mv);
			} 
			else if (msg.message != null) 
			{
				Debug.Log($"{msg.message.statusCode} - {msg.message.data}");
			}
			else {
				controllerManager.ManageMotion(msg.movementDto);
			}
		}


		public void StartControlPairing() {
			controllerManager.PairController(FindObjectsOfType<MotionAIController>().ToList());
		}

		#endregion
	}
}