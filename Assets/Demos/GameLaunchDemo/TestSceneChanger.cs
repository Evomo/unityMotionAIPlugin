using MotionAI.GameHub;
using UnityEngine;

namespace Demos.GameLaunchDemo {
	public class TestSceneChanger : MonoBehaviour {
		public SceneReference secondScene;


		public void LoadGame() {
			GameHubManager.Instance.LoadGameScene(secondScene);
		}


		public void UnloadGame() {
			GameHubManager.Instance.UnloadScene();
		}

	}
}