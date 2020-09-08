using System;
using MotionAI.Core.Models;
using TypeReferences;

namespace MotionAI.Core {
	[Serializable]
	public class SDKConfig {
		public string licenseID;
		public string username;
		[Inherits(typeof(MotionModel))] public TypeReference classificationModel;
		
	}
}