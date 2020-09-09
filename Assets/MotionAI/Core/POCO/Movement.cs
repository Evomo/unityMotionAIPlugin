using System;
using System.Collections.Generic;

namespace MotionAI.Core.POCO {
	[Serializable]
	public class Movement {
		public float gVelAmplitudePositive;
		public float gVelAmplitudeNegative;
		public float amplitude;
		public float durationPositive;
		public float durationNegative;

		public List<ElementalMovement> elmos;
		public short typeID;
		public string typeLabel;
		public DateTime start;
		public DateTime end;
	}
}