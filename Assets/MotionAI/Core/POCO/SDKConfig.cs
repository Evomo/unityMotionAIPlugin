using System;

namespace MotionAI.Core.POCO {
	[Serializable]
	public class SDKConfig {
		public string licenseID;
		public string username;

		public MovementModel classificationModel;
	}
}