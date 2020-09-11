using System;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace MotionAI.GameHub {
	public class GameLauncherData {
		public SceneReference mainSceneReference;
		public string gameName;
		public Texture2D image;
		public string description;
		public string developerName;
	}

	public class GameHubGame : ScriptableObject {
		public SceneReference mainSceneReference;
		public string gameName;
		public Texture2D image;
		public string description;
		public string developerName;


		private void OnEnable() {
			try {
				TextureImporter ti = (TextureImporter) AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(image));

				ti.npotScale = TextureImporterNPOTScale.None;
				ti.isReadable = true;
			}
			catch (NullReferenceException) {
				
			}
		}

		public GameLauncherData GetGameLauncherData() {
			GameLauncherData gld = new GameLauncherData();

			gld.mainSceneReference = mainSceneReference;
			gld.gameName = gameName;
			gld.image = image;
			gld.description = description;
			gld.developerName = developerName;

			return gld;
		}
	}
}