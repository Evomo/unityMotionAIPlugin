
#import <Foundation/Foundation.h>
#if __has_include("EvomoUnitySDK-Swift.h")
    #import "EvomoUnitySDK-Swift.h"
#else
    #import <EvomoUnitySDK/EvomoUnitySDK-Swift.h>
#endif
#import "EvomounityBridge.h"

# note: c method declations executes objective-c function

EvomounityBridge *bridge;

NSString* CreateNSString (const char* string)
{
  if (string)
    return [NSString stringWithUTF8String: string];
  else
        return [NSString stringWithUTF8String: ""];
}

void InitEvomoBridge(UnityCallback evomoCallback, const char* licenseID, const char* debugging)
{
    bridge = [EvomounityBridge alloc];
    [bridge Init:evomoCallback licenseID:CreateNSString(licenseID) debugging:CreateNSString(debugging)];
}

void StartEvomoBridge(const char* deviceOrientation, const char* classificationModel, const char* gaming)
{
    [bridge Start:CreateNSString(deviceOrientation) classificationModel:CreateNSString(classificationModel) gaming:CreateNSString(gaming)];
}

void StopEvomoBridge()
{
    [bridge Stop];
}

void LogEventBridge(const char* eventType, const char* note)
{
    [bridge LogEvent:CreateNSString(eventType) note:CreateNSString(note)];
}

void LogTargetMovementBridge(const char* movementType, const char* note)
{
    [bridge LogTargetMovement:CreateNSString(movementType) note:CreateNSString(note)];
}

void LogFailureBridge(const char* source, const char* failureType, const char* movementType, const char* note)
{
    [bridge LogFailure:CreateNSString(source) failureType:CreateNSString(failureType) movementType:CreateNSString(movementType) note:CreateNSString(note)];
}

void SetUsernameBridge(const char* username)
{
    [bridge SetUsername:CreateNSString(username)];
}
