
#import <Foundation/Foundation.h>
#if __has_include("EvomoUnitySDK-Swift.h")
    #import "EvomoUnitySDK-Swift.h"
#else
    #import <EvomoUnitySDK/EvomoUnitySDK-Swift.h>
#endif
#import "EvomounityBridge.h"

EvomounityBridge *bridge;

void InitEvomoBridge(UnityCallback testCallback, String licenseID)
{
    bridge = [EvomounityBridge alloc];
    [bridge Init:testCallback licenseID:licenseID];
}

void StartEvomoBridge()
{
    [bridge Start];
}

void StopEvomoBridge()
{
    [bridge Stop];
}

void LogEventBridge(String eventType, String note)
{
    [bridge LogEvent:eventType note:note];
}

void LogTargetMovementBridge(String movementType, String note)
{
    [bridge LogTargetMovement:movementType note:note];
}

void LogFailureBridge(String: source, String failureType, String movementType, String note)
{
    [bridge LogFailure:source failureType:failureType movementType:movementType note:note];
}