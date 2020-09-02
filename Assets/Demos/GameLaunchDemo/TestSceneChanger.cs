using System;
using System.Collections;
using System.Collections.Generic;
using MotionAI.GameHub;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestSceneChanger : MonoBehaviour {
	public SceneReference secondScene;


	public void LoadGame() {
		GameHubManager.Instance.LoadGameScene(secondScene);
	}


	public void UnloadGame() {
		GameHubManager.Instance.UnloadScene();
	}

}