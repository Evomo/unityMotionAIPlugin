# EvomoUnitySDK

## Requirements

- The EvomoUnityMotionAI is currently only available for iOS.
- The EvomoUnityMotionAI requires a minimum iOS target of 12.1.
- The EvomoUnityMotionAI only supports 64 bit builds.



## Installation

Simply Download and import the EvomoUnityMotionAI unity package.

## Usage

### Init the Evomo SDK

Before tracking movement events, you must init the Evomo SDK. Here is an example of how you could do that using the Awake method of a script:

```
private void Awake()
{
    Evomo.Init(EvomoReady);
}

private void EvomoReady()
{
    Debug.Log("Evomo ready to track movements!");
    //Continue with setting up your game stuff
}

```

### Track Movements

Before you start tracking movements, you should subscribe to the events that you are interested in.

here is a simple example:

```
private void SubscribeToEvomoEvents()
{
    Evomo.OnLeft.AddListener(OnLeft);
    Evomo.OnRight.AddListener(OnRight);
    Evomo.OnJump.AddListener(OnJump);
    Evomo.OnDuck.AddListener(OnDuck);
}

private void OnLeft()
{
    //move character left
}

private void OnRight()
{
    //move character right
}

private void OnJump()
{
    //make character jump
}

private void OnDuck()
{
    //make character duck
}
```



To start and stop tracking movements, use the following methods:

```
Evomo.StartTracking();
Evomo.StopTracking();
```

## Building

###1: Export the Xcode project from unity.

Be sure to set the minimum iOS version to 12.1 and the supported arch to ARM64 only in your unity player settings before exporting.

###2: Enable swift in the Xcode project.

Because the exported project is Objective-C based you must open the exported project and manually add the build setting 'SWIFT_VERSION = 5' to your targets build settings.

**IMPORTANT NOTE:** Unity have recently changed the way that the exported Xcode project is organized. If one of your targets is named 'UnityFramework' then you must add the setting to that target.

If you do not see a target named 'UnityFramework' then you must add the setting to the target named 'Unity-iPhone'.

###3: Add the Evomo cocoa pod

Run pod init in the project and add the following cocoa pod to your pod file:

```
pod 'EvomoUnityMotionAI'
```
**IMPORTANT NOTE:** Once again you must check if you have a target named 'UnityFramework' and if so add the pod to that target only. If you do not have that target in your podfile then you should add the pod to the target named 'Unity-iPhone' only.

Run pod install and from then on use the workspace generated and you should be good to go.

This process only needs to be done the first time you export from unity. For following builds you can use the 'append' option.

If you use the replace option then you will need to follow these steps again.
