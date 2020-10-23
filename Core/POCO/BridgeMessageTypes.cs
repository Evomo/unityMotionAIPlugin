using System;
using System.Collections.Generic;
using MotionAI.Core.Models.Generated;

namespace MotionAI.Core.POCO {
	[Serializable]
	public class BridgeMessage {
		public string deviceID;
		public EvoMovement movement;
		public ElementalMovement elmo;
		public Message message;
	}

	[Serializable]
	public class Message {
		public int statusCode;
		public string data;
	}

	[Serializable]
	public class ElementalMovement {
		public ElmoEnum typeID {
			get {
				ElmoEnum t;
				if (Enum.TryParse(typeLabel, true, out t)) return t;
				if (Enum.TryParse($"{typeLabel}up", true, out t)) return t;
				if (Enum.TryParse($"{typeLabel}_down", true, out t)) return t;

				return t;
			}
			set {
				if (Enum.IsDefined(typeof(ElmoEnum), value) && typeLabel == null) {
					typeLabel = Enum.GetName(typeof(ElmoEnum), value);
				}
			}
		}

		public string typeLabel;

		public bool rejected;
		// public DateTime start;
		// public DateTime end;
	}


	[Serializable]
	public class EvoMovement {
		public float gVelAmplitudePositive;
		public float gVelAmplitudeNegative;
		public float amplitude;
		public float durationPositive;
		public float durationNegative;
		public string deviceID;

		public List<ElementalMovement> elmos = new List<ElementalMovement>();
		public MovementEnum typeID;

		public string typeLabel;
		// public DateTime start;
		// public DateTime end;
	}
}