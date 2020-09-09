using System;
using MotionAI.Core.Models;

namespace MotionAI.Core.POCO {
	[Serializable]
	public class ElementalMovement {
		public ElmoEnum typeID;
		public string typeLabel;
		public bool rejected;
		public DateTime start;
		public DateTime end;
		public string deviceIdent;
	}
}