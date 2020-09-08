using System;
using MotionAI.Core.Controller;
using TypeReferences;
using TypeReferences.Deprecated;

namespace MotionAI.Core {
	[Serializable]
	public class SDKConfig {
		public string licenseID;
		public string username;

		[ClassExtends(typeof(MotionAIController))]
		public TypeReference classificationModel;
	}
}