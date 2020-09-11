using System;
using JetBrains.Annotations;

namespace MotionAI.Core.POCO {
	[Serializable]
	public class BridgeMessage {
		[CanBeNull] public Movement movement;
		[CanBeNull] public ElementalMovement elmo; 
	}
}