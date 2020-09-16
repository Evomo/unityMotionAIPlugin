using System;
using MotionAI.Core.POCO;
using UnityEngine.Events;

namespace MotionAI.Core.Controller {
	[Serializable]
	public class ControllerPairedEvent : UnityEvent<MotionAIController> {
	}

	[Serializable]
	public class OnMovementEvent : UnityEvent<MovementDto> {
	}

	[Serializable]
	public class OnElmoEvent : UnityEvent<ElementalMovement> {
	}
}