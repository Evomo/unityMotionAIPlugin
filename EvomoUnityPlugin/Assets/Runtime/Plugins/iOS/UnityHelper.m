
#import <Foundation/Foundation.h>
#if __has_include("EvomoUnitySDK-Swift.h")
    #import "EvomoUnitySDK-Swift.h"
#else
    #import <EvomoUnitySDK/EvomoUnitySDK-Swift.h>
#endif
#import "EvomounityBridge.h"

EvomounityBridge *bridge;

NSString* CreateNSString (const char* string)
{
  if (string)
    return [NSString stringWithUTF8String: string];
  else
        return [NSString stringWithUTF8String: ""];
}

void InitEvomoBridge(UnityCallback testCallback, const char* licenseID)
{
    bridge = [EvomounityBridge alloc];
    [bridge Init:testCallback licenseID:CreateNSString(licenseID)];
}

void StartEvomoBridge(const char* deviceOrientation, const char* classificationModel)
{
    [bridge Start:CreateNSString(deviceOrientation) classificationModel:CreateNSString(classificationModel)];
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
