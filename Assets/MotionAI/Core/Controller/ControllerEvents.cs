using System;
using MotionAI.Core.POCO;
using UnityEngine.Events;

namespace MotionAI.Core.Controller {
	
	[Serializable]
	public class ControllerPairedEvent : UnityEvent<string> { }
	[Serializable]

	public class OnMovementEvent : UnityEvent<Movement> { }

[Serializable]
	public class OnElmoEvent : UnityEvent<ElementalMovement> { }
}