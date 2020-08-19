﻿using UnityEngine;
using UnityEngine.UI;

public class EvomoDemoManager : MonoBehaviour
{

    public GameObject StartTrackingButton;

    public Text DebugText;

    private void Awake()
    {
        StartTrackingButton.SetActive(false);
        SubscribeToEvomoEvents();
        DebugText.text = "Waiting for Evomo Init";
        Evomo.Init(EvomoReady, "");
        Evomo.LogFailure(Evomo.EventSource.app, Evomo.FailureType.toLess, Evomo.MovementType.Duck, "abc");
        Evomo.SetUsername("testUser");
    }

    private void SubscribeToEvomoEvents()
    {
        Evomo.OnLeft.AddListener(OnLeft);
        Evomo.OnRight.AddListener(OnRight);
        Evomo.OnJump.AddListener(OnJump);
        Evomo.OnDuck.AddListener(OnDuck);
    }

    private void EvomoReady()
    {
        StartTrackingButton.SetActive(true);
        DebugText.text = "Evomo Init Complete";
    }

    public void StartTracking()
    {
        Evomo.StartTracking(deviceOrientation: Evomo.DeviceOrientation.buttonDown);
        Evomo.LogEvent("Started");
    }

    private void OnLeft()
    {
        DebugText.text = "Move Left Detected";
        Evomo.LogTargetMovement(Evomo.MovementType.Left);
    }
    private void OnRight()
    {
        DebugText.text = "Move Right Detected";
    }
    private void OnJump()
    {
        DebugText.text = "Jump Detected";
    }
    private void OnDuck()
    {
        DebugText.text = "Duck Detected";
    }
}