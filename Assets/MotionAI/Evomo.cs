#if UNITY_IOS && !UNITY_EDITOR
using AOT;
#endif
using System;
using UnityEngine.Events;
using DeviceOrientation = UnityEngine.DeviceOrientation;

namespace MotionAI {
    public class Evomo 
    {

        public delegate void UnityCallback(int value);

        public static UnityEvent OnJump = new UnityEvent();

        public static UnityEvent OnDuck = new UnityEvent();

        public static UnityEvent OnLeft = new UnityEvent();

        public static UnityEvent OnRight = new UnityEvent();

        private static Action _initCallback;


        #region Internal Load
#if UNITY_IOS && !UNITY_EDITOR

    [DllImport("__Internal")]
    private static extern void InitEvomoBridge(UnityCallback callback, string licenseID);

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

#if UNITY_IOS && !UNITY_EDITOR

    [MonoPInvokeCallback(typeof(UnityCallback))]
    private static void MessageRecived(int value)
    {
        var enumValue = (NativeMessageType)value;
        switch (enumValue)
        {
            case NativeMessageType.READY:
                _initCallback();
                break;
            case NativeMessageType.JUMP:
                OnJump.Invoke();
                break;
            case NativeMessageType.DUCK:
                OnDuck.Invoke();
                break;
            case NativeMessageType.LEFT:
                OnLeft.Invoke();
                break;
            case NativeMessageType.RIGHT:
                OnRight.Invoke();
                break;
        }
    }

#endif

        public static void Init(Action callback, string licenseID)
        {
#if UNITY_IOS && !UNITY_EDITOR
        _initCallback = callback;
        InitEvomoBridge(MessageRecived, licenseID);
#else
            callback();
#endif
        }

        public static void StartTracking(DeviceOrientation deviceOrientation = DeviceOrientation.Portrait, string classificationModel = "subway-surfer")
        {
#if UNITY_IOS && !UNITY_EDITOR
        StartEvomoBridge(deviceOrientation.ToString(), classificationModel);
#endif
        }

        public static void StopTracking()
        {
#if UNITY_IOS && !UNITY_EDITOR
        StopEvomoBridge();
#endif
        }

        public static void LogEvent(string eventType, string note = "")
        {
#if UNITY_IOS && !UNITY_EDITOR
        LogEventBridge(eventType, note);
#endif
        }

        public static void LogTargetMovement(MovementType movementType, string note = "")
        {
#if UNITY_IOS && !UNITY_EDITOR
        LogTargetMovementBridge(movementType.ToString(), note);
#endif
        }

        public static void LogFailure(EventSource source, FailureType failureType, MovementType movementType, string note = "")
        {

#if UNITY_IOS && !UNITY_EDITOR
        LogFailureBridge(source.ToString(), failureType.ToString(), movementType.ToString(), note);
#endif
        }

        public static void SetUsername(String username)
        {

#if UNITY_IOS && !UNITY_EDITOR
        SetUsernameBridge(username);
#endif
        }

    }
}
