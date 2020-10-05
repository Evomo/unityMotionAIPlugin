# MotionAI 
[![openupm](https://img.shields.io/npm/v/com.evomo.motionai?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.evomo.motionai/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

![Alt text](./docs/overview.svg)

## Requirements

- The EvomoUnityMotionAI is currently only available for iOS.
- The EvomoUnityMotionAI requires a minimum iOS target of 12.1.
- The EvomoUnityMotionAI only supports 64 bit builds.


## Install 
We recommend using the OpenUPM to install it by running:
```
openupm add com.evomo.motionai
```


But if you wish to use it without  installing an external dependency you can add the scope and dependencies using: 


```

{
  "scopedRegistries": [
    {
      "name": "package.openupm.com",
      "url": "https://package.openupm.com",
      "scopes": [
        "com.solidalloy.type.references",
        "com.evomo.motionai",
        "com.openupm"
      ]
    }
  ],
  "dependencies": {
    "com.evomo.motionai": "1.0.3"
  }
}
```


## Getting Started
First of all, you need to [register](https://subscriptions.zoho.eu/subscribe/a86776477592bad75f6bc8765d4c5c76a57851cb64dfe979651bdda4a1c7d344/beta) for a license key.

Create an SDK License Asset by clicking on `Evomo -> MotionAI -> Create SDK Settings` and add your license key. 

This asset is used to identify your profile and to get access to our multiple motion models.

The components needed for the MotionAI Plugin to work are: 

+ **MotionAIManager**
    
     + Receives and sends messages to and from the Native SDK
     + Pairs and communicates with MotionAIControllers in the scene
       
+ **MotionAIController**
  
     + Manages which motion model to use.
     + Invokes the motion events once a movement is confirmed  
     + It's overridable and you can either handle the events in the inspector or create your custom controller through code as shown in the [ModelEventController](https://github.com/Evomo/unityMotionAIPlugin/blob/master/Assets/MotionAI/Samples/CoreDemo/ModelEventController.cs) or forego all movements and work exclusively with the [ElementalMovements](https://github.com/Evomo/unityMotionAIPlugin/blob/master/Assets/MotionAI/Samples/ElmoDemo/ElmoController.cs) 
