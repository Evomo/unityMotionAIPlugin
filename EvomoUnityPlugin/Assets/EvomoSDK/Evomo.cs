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
    private static extern void InitEvomoBridge(UnityCallback callback);

    [DllImport("__Internal")]
    private static extern void StartEvomoBridge();

    [DllImport("__Internal")]
    private static extern void StopEvomoBridge();

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

    public static void Init(Action callback)
    {
#if UNITY_IOS && !UNITY_EDITOR
        _initCallback = callback;
        InitEvomoBridge(MessageRecived);
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

}


