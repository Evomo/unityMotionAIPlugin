using System;
using MotionAI.Core.Controller;
using UnityEngine.Events;

namespace MotionAI.Core.POCO {
	[Serializable]
	public class ControllerPairedEvent : UnityEvent<MotionAIController> { }


	[Serializable]
	public class OnSDKMessage : UnityEvent<string> { }

	[Serializable]
	public class OnMovementEvent : UnityEvent<EvoMovement> { }

	[Serializable]
	public class OnElmoEvent : UnityEvent<ElementalMovement> { }
}