using System;
using MotionAI.Core.Controller;
using MotionAI.Core.POCO;
using TypeReferences;
using TypeReferences.Deprecated;

namespace MotionAI.Core {
	[Serializable]
	public class SDKConfig {
		public string licenseID;
		public string username;

		public MovementModel classificationModel;
	}
}