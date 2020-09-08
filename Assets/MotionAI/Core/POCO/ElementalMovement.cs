using System;

namespace MotionAI.Core.POCO {
	[Serializable]
	public class ElementalMovement {
		public short typeID;
		public string typeLabel;
		public bool rejected;
		public DateTime start;
		public DateTime end;
		public string deviceIdent;
	}
}