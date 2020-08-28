using DataStructures;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MotionAI.GameHub {
	public class GameHubManager : Singleton<GameHubManager> {

		public void ChangeScene(SceneReference scene) {
			SceneManager.LoadScene(scene, LoadSceneMode.Single);
		}
	}
}