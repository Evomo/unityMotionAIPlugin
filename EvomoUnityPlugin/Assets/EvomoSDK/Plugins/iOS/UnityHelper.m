
#import <Foundation/Foundation.h>
#if __has_include("EvomoUnitySDK-Swift.h")
    #import "EvomoUnitySDK-Swift.h"
#else
    #import <EvomoUnitySDK/EvomoUnitySDK-Swift.h>
#endif
#import "EvomounityBridge.h"

EvomounityBridge *bridge;

void InitEvomoBridge(UnityCallback testCallback)
{
    bridge = [EvomounityBridge alloc];
    [bridge Init:testCallback];
}

void StartEvomoBridge()
{
    [bridge Start];
}

void StopEvomoBridge()
{
    [bridge Stop];
}
