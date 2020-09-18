#if UNITY_IOS && !UNITY_EDITOR
using AOT;
#endif
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using MotionAI.Core.Controller;
using MotionAI.Core.Models.Constants;
using MotionAI.Core.POCO;
using UnityEngine;
using static MotionAI.UtilHelper;

namespace MotionAI.Core
{
    public class MotionAIManager : MonoBehaviour
    {
        #region Internal Load

        public delegate void UnityCallback(string value);

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
        ManageMotion(message);
    }

#endif


        public void StartTracking()
        {
#if UNITY_IOS && !UNITY_EDITOR
// TODO: Add third parameter gaming - if model_type == gaming -> input_string = "true"
// UtilHelper.ToCustomOrientation(Input.deviceOrientation)
        StartEvomoBridge("buttonDown", mySDKConfig.classificationModel.ToString(), "true");
#endif
            IsTracking = true;
            ControlPairing();
        }

        public void StopTracking()
        {
#if UNITY_IOS && !UNITY_EDITOR
        StopEvomoBridge();
#endif
            IsTracking = false;
        }

        public static void LogEvent(string eventType, string note = "")
        {
#if UNITY_IOS && !UNITY_EDITOR
        LogEventBridge(eventType, note);
#endif
        }

        public static void LogTargetMovement(ElmoEnum movementType, string note = "")
        {
#if UNITY_IOS && !UNITY_EDITOR
        LogTargetMovementBridge(movementType.ToString(), note);
#endif
        }

        public void LogFailure(EventSource source, FailureType failureType, ElmoEnum movementType,
            string note = "")
        {
#if UNITY_IOS && !UNITY_EDITOR
        LogFailureBridge(source.ToString(), failureType.ToString(), movementType.ToString(), note);
#endif
        }

        public void SetUsername(String username)
        {
#if UNITY_IOS && !UNITY_EDITOR
        SetUsernameBridge(username);
#endif
        }

        #endregion


        #region Unity

        public SDKConfig mySDKConfig;
        public ControllerManager controllerManager;

        public bool IsTracking { get; private set; }

        #region Lifecycle

        private void Awake()
        {
#if UNITY_IOS && !UNITY_EDITOR
        SetUsernameBridge(mySDKConfig.username);
#endif
            controllerManager = new ControllerManager();
        }

        private void OnEnable()
        {
#if UNITY_IOS && !UNITY_EDITOR
				// TODO add parameter to global Manager to define if debuggin is active (sdk will send some debugging and raw measurement data to the server)
				// Enter the boolean as string like "true" and "false"
        InitEvomoBridge(MessageRecived, mySDKConfig.licenseID, "true");
#endif
        }


        private void OnDisable()
        {
            StopTracking();
        }

        #endregion


        public void ManageMotion(string movementStr)
        {
            BridgeMessage msg = JsonUtility.FromJson<BridgeMessage>(movementStr);


            if (msg.movement == null)
            {
                Movement mv = new Movement();
                mv.elmos.Add(msg.elmo);
                controllerManager.ManageMotion(mv);
            }
            else
            {
                controllerManager.ManageMotion(msg.movement);
            }
        }


        public void ControlPairing()
        {
            controllerManager.PairController(FindObjectsOfType<MotionAIController>().ToList());
        }

        #endregion
    }
}