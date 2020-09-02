using System.Linq;
using System.Threading;
using MotionAI.GameHub;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using PackageInfo = UnityEditor.PackageInfo;

namespace MotionAI.Editor {
	public class GameLauncherMenu : MonoBehaviour {
		[MenuItem("Evomo/Create Game Asset")]
		public static void CreateSceneObjects() {
			GameHubGame gs = ScriptableObject.CreateInstance<GameHubGame>();
			gs.gameName = Application.productName;

			AssetDatabase.CreateAsset(gs, $"Assets/{gs.gameName}.asset");

			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
	}
}

