using System;
using UnityEditor;
using UnityEngine;

namespace MotionAI.Core.POCO {
	[Serializable]
	public class SDKConfig : ScriptableObject {
		public string licenseID;
		public string username;

#if UNITY_EDITOR
		
		[MenuItem("Evomo/MotionAI/Create SDK Settings ")]
		public static void CreateSDKSettings() {
			SDKConfig gs = ScriptableObject.CreateInstance<SDKConfig>();
			AssetDatabase.CreateAsset(gs, $"Assets/{Application.productName}-MotionAI.asset");
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
#endif

	}
}