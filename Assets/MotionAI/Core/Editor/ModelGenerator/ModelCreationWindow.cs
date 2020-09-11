using System.IO;
using MotionAI.Core.Editor.ModelGenerator.POCO;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Windows;

namespace MotionAI.Core.Editor {
	public class ModelCreationWindow : EditorWindow {
		public string modelJsonPath;
		
		[MenuItem("Evomo/My Window")]
		static void Init() {
			// Get existing open window or if none, make a new one:
			GetWindow<ModelCreationWindow>("Model Class Creation");
		}

		void OnGUI() {
			CreateField("Model", ref modelJsonPath);


			if (GUILayout.Button("Generate Models")) {
				GenerateModels();
			}
		}


		private void CreateField(string label, ref string text) {
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField(label, GUILayout.Width(EditorGUIUtility.labelWidth - 4));
				EditorGUILayout.SelectableLabel(text, EditorStyles.textField,
					GUILayout.Height(EditorGUIUtility.singleLineHeight));
			}
			if (GUILayout.Button($"Set {label} JSON ")) {
				text = EditorUtility.OpenFilePanel($"{label} JSON", "Assets/MotionAI/Core/Editor/ModelFiles", "json");
			}

			EditorGUILayout.EndHorizontal();
		}

		private void GenerateModels() {
			StreamReader reader = new StreamReader(modelJsonPath);


			ModelJson mj = JsonUtility.FromJson<ModelJson>(reader.ReadToEnd());
			Debug.Log(mj);
		}
	}
}