
#import <Foundation/Foundation.h>
#if __has_include("EvomoUnitySDK-Swift.h")
    #import "EvomoUnitySDK-Swift.h"
#else
    #import <EvomoUnitySDK/EvomoUnitySDK-Swift.h>
#endif
#import "EvomounityBridge.h"

EvomounityBridge *bridge;

void InitEvomoBridge(UnityCallback testCallback, NSString* licenseID)
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

void LogEventBridge(NSString* eventType, NSString* note)
{
    [bridge LogEvent:eventType note:note];
}

void LogTargetMovementBridge(NSString* movementType, NSString* note)
{
    [bridge LogTargetMovement:movementType note:note];
}

void LogFailureBridge(NSString* source, NSString* failureType, NSString* movementType, NSString* note)
{
    [bridge LogFailure:source failureType:failureType movementType:movementType note:note];
}
