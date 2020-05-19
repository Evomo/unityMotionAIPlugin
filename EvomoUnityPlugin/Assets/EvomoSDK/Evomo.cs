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

#endif

    public enum DeviceOrientation
    {
        buttonDown,
        buttonRight,
        buttonLeft
    }

    public enum EventSource
    {
        app,
        manual
    }

    public enum FailureType
    {
        toLess,
        toMuch
    }

    public enum MovementType
    {
        Jump,
        Duck,
        Left,
        Right,
        Tab
    }

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

    public static void Init(Action callback, string licenseID)
    {
#if UNITY_IOS && !UNITY_EDITOR
        _initCallback = callback;
        InitEvomoBridge(MessageRecived, licenseID);
#else
        callback();
#endif
    }

    public static void StartTracking(DeviceOrientation deviceOrientation = DeviceOrientation.buttonDown, string classificationModel = "2115")
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

}
