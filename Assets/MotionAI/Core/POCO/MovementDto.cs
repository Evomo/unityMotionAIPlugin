using System;
using System.Collections.Generic;
using MotionAI.Core.Models.Generated;

namespace MotionAI.Core.POCO {
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