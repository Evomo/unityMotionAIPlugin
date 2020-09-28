using System;
using System.Collections.Generic;
using MotionAI.Core.Models.Generated;

namespace MotionAI.Core.POCO {
	[Serializable]
	public class BridgeMessage {
		public MovementDto movementDto;
		public ElementalMovement elmo;
	}

	[Serializable]
	public class ElementalMovement {
		public ElmoEnum typeID;
		public string typeLabel;
		public bool rejected;
		// public DateTime start;
		// public DateTime end;
		public string deviceIdent;
	}


	[Serializable]
	public class MovementDto {
		public float gVelAmplitudePositive;
		public float gVelAmplitudeNegative;
		public float amplitude;
		public float durationPositive;
		public float durationNegative;


		public List<ElementalMovement> elmos = new List<ElementalMovement>();
		public MovementEnum typeID;

		public string typeLabel;
		// public DateTime start;
		// public DateTime end;
	}
}