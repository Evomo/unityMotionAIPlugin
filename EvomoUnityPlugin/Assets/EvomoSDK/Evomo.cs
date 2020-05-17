using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
#if UNITY_IOS && !UNITY_EDITOR
using AOT;
#endif
using UnityEngine;
using UnityEngine.Events;

public class Evomo : MonoBehaviour
{

    public delegate void UnityCallback(int value);

    public static UnityEvent OnJump = new UnityEvent();

    public static UnityEvent OnDuck = new UnityEvent();

    public static UnityEvent OnLeft = new UnityEvent();

    public static UnityEvent OnRight = new UnityEvent();

    private static Action _initCallback;

#if UNITY_IOS && !UNITY_EDITOR

    [DllImport("__Internal")]
    private static extern void InitEvomoBridge(UnityCallback callback, String licenseID);

    [DllImport("__Internal")]
    private static extern void StartEvomoBridge();

    [DllImport("__Internal")]
    private static extern void StopEvomoBridge();

    [DllImport("__Internal")]
    private static extern void LogEventBridge(String eventType, String note);

    [DllImport("__Internal")]
    private static extern void LogTargetMovementBridge(String movementType, String note);

    [DllImport("__Internal")]
    private static extern void LogFailureBridge(String source, String failureType, String movementType, String note);

#endif

    private enum NativeMessageType
    {
        READY = 0,
        JUMP = 1,
        DUCK = 2,
        LEFT = 3,
        RIGHT = 4
    }

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

    public static void Init(Action callback, String licenseID)
    {
#if UNITY_IOS && !UNITY_EDITOR
        _initCallback = callback;
        InitEvomoBridge(MessageRecived, licenseID);
#else
        callback();
#endif
    }

    public static void StartTracking()
    {
#if UNITY_IOS && !UNITY_EDITOR
        StartEvomoBridge();
#endif
    }

    public static void StopTracking()
    {
#if UNITY_IOS && !UNITY_EDITOR
        StopEvomoBridge();
#endif
    }

    public static void LogEvent(String eventType, String note)
    {
#if UNITY_IOS && !UNITY_EDITOR
        LogEventBridge(eventType, note);
#endif
    }

    public static void LogTargetMovement(String movementType, String note)
    {
#if UNITY_IOS && !UNITY_EDITOR
        LogTargetMovementBridge(movementType, note);
#endif
    }

    public static void LogFailure(String source, String failureType, String movementType, String note)
    {
#if UNITY_IOS && !UNITY_EDITOR
        LogFailureBridge(source, failureType, movementType, note);
#endif
    }

}


