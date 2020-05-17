using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        Evomo.LogEvent("init", "test");
        Evomo.LogTargetMovement("hop", "abc");
        Evomo.LogFailure("app", "toLess", "hop", "abc");
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
        Evomo.StartTracking();
    }

    private void OnLeft()
    {
        DebugText.text = "Move Left Detected";
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
