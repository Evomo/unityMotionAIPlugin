using System;
using UnityEditor;
using UnityEngine;

namespace MotionAI.GameHub {
	public class GameLauncherData : ScriptableObject {
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
			catch (NullReferenceException e) {
			}
		}
	}
}