using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MotionAI.Core.Editor.ModelGenerator.Builders;
using MotionAI.Core.Models.Generated;
using MotionAI.Core.Util;
using UnityEditor;
using UnityEngine;

namespace MotionAI.Core.Editor.ModelGenerator {
	public class ModelCreationWindow : EditorWindow {
		public string modelJsonPath, outputPath;


		[MenuItem("Evomo/Class Generation")]
		static void Init() {
			// Get existing open window or if none, make a new one:
			GetWindow<ModelCreationWindow>("Model Class Creation");
		}

		void OnGUI() {
			CreateField("Model JSON", ref modelJsonPath);
			CreateField("Output Path", ref outputPath, true);

			if (GUILayout.Button("Create Enums")) {
				GenerateEnums();
			}


			if (GUILayout.Button("Create Models")) {
				GenerateModel();
			}
		}


		private void GenerateModel() {
			StreamReader reader = new StreamReader(modelJsonPath);
			ModelJsonDump mj = JsonUtility.FromJson<ModelJsonDump>(reader.ReadToEnd());


			foreach (ModelSeriesJson model_series in mj.model_series) {
				CustomClassBuilder ccb =
					new CustomClassBuilder(outputPath, model_series.name.CleanFromDB());


				ccb.WithImport("UnityEngine")
					.WithImport("System");

				int modelNum = model_series.builds.prod == 0 ? model_series.builds.beta : model_series.builds.prod;
				List<string> allElmos = new List<string>();

				ModelJson foundModelJson = mj.models.Find(x => x.test_run == modelNum);


				if (foundModelJson != null) {
					ccb
						.InheritsFrom("AbstractModelComponent")
						.WithReadOnlyField("modelType", model_series.model_type)
						.WithReadOnlyField("betaID", model_series.builds.beta)
						.WithReadOnlyField("productionID", model_series.builds.prod)
						.WithReadOnlyField("modelName", model_series.name)
						.WithObject("moves", "Movements");

					CustomClassBuilder icb2 = ccb.WithInternalClass("Movements").WithCustomAttribute("Serializable");
					foreach (string moveName in foundModelJson.movement_types) {
						MovementJson mv = mj.movement_types.Find(m => m.name == moveName);

						if (mv != null) {
							icb2.CreateMovement(mv);
							foreach (string mvElmo in mv.elmos) allElmos.Add(mvElmo);
						}
						else {
							Debug.LogError($"Move {moveName} not found");
						}
					}
				}
				else {
					Debug.LogError($"Model with name {model_series.name} not found");
				}

				Dictionary<string, ElmoEnum> elmoDict = new Dictionary<string, ElmoEnum>();
				foreach (string elmo in allElmos) {
					try {
						ElmoEnum v = (ElmoEnum) Enum.Parse(typeof(ElmoEnum), elmo.CleanFromDB());
						elmoDict.Add(elmo, v);
					}
					catch (ArgumentException) {
						Debug.LogError($"Elmo {elmo} not found");
					}
				}

				ccb.Build();
			}
		}

		private void CreateField(string label, ref string text, bool output = false) {
			string directory =
				output ? "Assets/MotionAI/Core/Models/Generated" : "Assets/MotionAI/Core/Editor/ModelFiles";
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField(label, GUILayout.Width(EditorGUIUtility.labelWidth - 4));
				EditorGUILayout.SelectableLabel(text, EditorStyles.textField,
					GUILayout.Height(EditorGUIUtility.singleLineHeight));
			}
			if (GUILayout.Button($"Set {label}")) {
				text = output
					? EditorUtility.OpenFolderPanel(label, directory, "")
					: EditorUtility.OpenFilePanel(label, directory, "json");
			}

			EditorGUILayout.EndHorizontal();
		}


		private void GenerateEnums() {
			StreamReader reader = new StreamReader(modelJsonPath);


			ModelJsonDump mj = JsonUtility.FromJson<ModelJsonDump>(reader.ReadToEnd());
			List<string> elmos = mj.movement_types
				.SelectMany(el => el.elmos)
				.Distinct()
				.OrderBy(e => e)
				.Select(elmo => elmo.Replace(" ", "_").Replace("-", "_"))
				.ToList();


			List<string> device_positions = mj.models
				.Select(ms => ms.device_position)
				.Distinct()
				.OrderBy(e => e)
				.Select(e => e.CleanFromDB())
				.ToList();

			Dictionary<string, int> movements = mj.movement_types
				.DistinctBy(x => x.name)
				.ToDictionary(x => x.name, x => x.id);


			List<string> models = mj.model_series.Select(x => x.name).Distinct().ToList();
			new CustomClassBuilder(outputPath, "Constants")
				.WithEnum("ElmoEnum", elmos)
				.WithEnum("DevicePosition", device_positions)
				.WithEnum("MovementEnum", movements)
				.WithEnum("MovementModel", models)
				.Build();

			string f = $"{outputPath}/Constants.cs";
			string[] lines = File.ReadAllLines(f);
			string[] newLines = RemoveUnnecessaryLine(lines);
			File.WriteAllLines(f, newLines);
		}

		private string[] RemoveUnnecessaryLine(string[] lines) {
			// Hardcoding goes bzzzzzzzzzzzzrt
			string toCheck = "    public class Constants {";
			List<string> l = new List<string>();
			foreach (string line in lines) {
				if (!line.Equals(toCheck)) {
					l.Add(line);
				}
			}

			l.RemoveAt(l.Count - 1);
			return l.ToArray();
		}
	}
}