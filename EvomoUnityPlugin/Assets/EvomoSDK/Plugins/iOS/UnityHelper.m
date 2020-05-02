
#import <Foundation/Foundation.h>
#import <EvomoUnitySDK/EvomounityBridge.h>

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
