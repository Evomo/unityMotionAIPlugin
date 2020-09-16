using System;
using JetBrains.Annotations;

namespace MotionAI.Core.POCO {
	[Serializable]
	public class BridgeMessage {
		[CanBeNull] public MovementDto movementDto;
		[CanBeNull] public ElementalMovement elmo; 
	}
}