using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using MotionAI.Core.Editor.ModelGenerator.Builders;
using MotionAI.Core.Models;
using MotionAI.Core.Util;
using NUnit.Framework.Internal;
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

			if (GUILayout.Button("Create Movements")) {
				GenerateMovements();
			}


			if (GUILayout.Button("Create Models")) {
				GenerateModel();
			}
		}

		private void GenerateModel() {
			StreamReader reader = new StreamReader(modelJsonPath);
			ModelJson mj = JsonUtility.FromJson<ModelJson>(reader.ReadToEnd());

			CustomClassBuilder ccb = new CustomClassBuilder(outputPath, "Evomodels");


			
			foreach (ModelSeries model_series in mj.model_series) {
				CustomClassBuilder icb = ccb.WithInternalClass(model_series.name);

				int modelNum = model_series.builds.prod == 0 ? model_series.builds.beta : model_series.builds.prod;
				Model foundModel = mj.models.Find(x => x.test_run == modelNum);
				icb
					.WithReadOnlyField("model_type", model_series.model_type)
					.WithReadOnlyField("beta_id", model_series.builds.beta)
					.WithReadOnlyField("prod_id", model_series.builds.prod)
					.WithReadOnlyField("name", model_series.name);

				if (foundModel != null) {
					CustomClassBuilder icb2 = icb.WithInternalClass("Movements");
					foreach (string moveName in foundModel.movement_types) {
						Movement mv = mj.movement_types.Find(m => m.name == moveName);
						if (mv != null) {
							icb2.WithInternalClass(moveName.CleanFromDB())
								.CreateMovement(mv);
						}
						else {
							Debug.LogError($"Move {moveName} not found");
						}
					}
				}
				else {
					Debug.LogError($"Model with {model_series.name} not found");
				}
			}

			ccb.Build();
		}

		private void GenerateMovements() {
			StreamReader reader = new StreamReader(modelJsonPath);
			ModelJson mj = JsonUtility.FromJson<ModelJson>(reader.ReadToEnd());

			CustomClassBuilder ccb = new CustomClassBuilder(outputPath, "Movements");

			List<Movement> filteredMoves = mj.movement_types.DistinctBy(x => x.name).ToList();
			foreach (Movement mv in filteredMoves) {
				ccb = ccb
					.WithInternalClass(mv.name)
					.CreateMovement(mv);
			}


			ccb.Build();
		}


		private void CreateField(string label, ref string text, bool output = false) {
			string directory = output ? "Assets/MotionAI/Core/Models" : "Assets/MotionAI/Core/Editor/ModelFiles";
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


			ModelJson mj = JsonUtility.FromJson<ModelJson>(reader.ReadToEnd());
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
			CustomClassBuilder cb = new CustomClassBuilder(outputPath, "Constants");

			cb.WithEnum("ElmoEnum", elmos)
				.WithEnum("DevicePosition", device_positions)
				
				.Build();
		}
	}
}