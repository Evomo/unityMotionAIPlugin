using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MotionAI.Core.Editor.ModelGenerator.Builders;
using MotionAI.Core.Models;
using MotionAI.Core.Models.Generated;
using MotionAI.Core.POCO;
using MotionAI.Core.Util;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace MotionAI.Core.Editor.ModelGenerator {
	public class ModelCreationWindow : EditorWindow {
		public string modelJsonPath, outputPath;

		public ModelJsonDump mj;

		#region Unity GUI

		[MenuItem("Evomo/Tools/Class Generation")]
		static void Init() {
			GetWindow<ModelCreationWindow>("Model Class Creation");
		}

		void OnGUI() {
			if (outputPath != null && mj == null) {
				ReadModelJson();
			}

			CreateField("Model JSON", ref modelJsonPath);
			CreateField("Output Path", ref outputPath, true);

			if (mj != null) {
				if (GUILayout.Button("Create Constants")) {
					GenerateConstants();
				}

				if (GUILayout.Button("Create Models")) {
					GenerateModel();
				}
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

				ReadModelJson();
			}

			EditorGUILayout.EndHorizontal();
		}

		private void ReadModelJson() {
			try {
				StreamReader reader = new StreamReader(modelJsonPath);
				mj = JsonUtility.FromJson<ModelJsonDump>(reader.ReadToEnd());
			}
			catch (Exception) {
				Debug.LogError("Please choose a valid move json");
			}
		}

		#endregion


		#region Generators

		private void GenerateModel() {
			HashSet<string> checkedModels = new HashSet<string>();
			foreach (ModelSeriesJson modelSeries in mj.model_series) {
				if (checkedModels.Contains(modelSeries.name)) continue;

				checkedModels.Add(modelSeries.name);

				CustomClassBuilder ccb =
					new CustomClassBuilder(outputPath, modelSeries.name.CleanFromDB());


				ccb.WithImport("UnityEngine")
					.WithImport("System")
					.WithImport("System.Collections.Generic")
					.WithImport("System.Linq")
					.WithImport("MotionAI.Core.POCO");


				string moveHoldersnippet =
					@"moves.GetType().GetFields().Select(x => (MoveHolder)(x.GetValue(moves))).ToList();";

				string availableTypes =
					@"meta.GetType().GetFields().Select(x => (ModelBuildMeta)(x.GetValue(meta))).ToList();";

				ccb
					.InheritsFrom("AbstractModelComponent")
					.WithMethod("GetMoveHolders", moveHoldersnippet, typeof(List<MoveHolder>))
					.WithMethod("GetAvailableTypes", availableTypes, typeof(List<ModelBuildMeta>))
					.WithReadOnlyField("modelType", (ModelType) Enum.Parse(typeof(ModelType), modelSeries.model_type))
					.WithReadOnlyField("modelName", modelSeries.name)
					.WithObject("moves", "Movements")
					.WithObject("meta", "Metadata");

				CreateMovementClass(ccb, modelSeries);
				CreateMetaDataClasses(ccb, modelSeries.name);

				ccb.Build();
			}
		}

		private void CreateMetaDataClasses(CustomClassBuilder ccb, string modelName) {
			List<ModelBuildMeta> fullBuilds = mj.model_series
				.FindAll(x => x.name == modelName)
				.Select(x => new ModelBuildMeta(x.device_position, x.builds.prod, x.builds.beta))
				.ToList();

			CustomClassBuilder icb2 = ccb.WithInternalClass("Metadata").WithCustomAttribute("Serializable");
			foreach (ModelBuildMeta mbm in fullBuilds) {
				CodeExpression[] p = new CodeExpression[] {
					new CodePrimitiveExpression(mbm.devicePosition.ToString()),
					new CodePrimitiveExpression(mbm.prodID),
					new CodePrimitiveExpression(mbm.betaID),
				};
				var cobe = new CodeObjectCreateExpression(typeof(ModelBuildMeta), p);
				icb2.WithObject(mbm.devicePosition.ToString().ToClassCase(), "ModelBuildMeta", cobe);
			}
		}

		private void CreateMovementClass(CustomClassBuilder ccb, ModelSeriesJson modelSeries) {
			int modelNum = modelSeries.builds.prod == 0 ? modelSeries.builds.beta : modelSeries.builds.prod;

			ModelJson foundModelJson = mj.models.Find(x => x.test_run == modelNum);


			List<string> allElmos = new List<string>();
			if (foundModelJson != null) {
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
				Debug.LogError($"Model with name {modelSeries.name} not found");
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
		}


		private void GenerateConstants() {
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

			List<string> modelType = mj.model_series.Select(x => x.model_type).Distinct().ToList();

			new CustomClassBuilder(outputPath, "Constants")
				.WithEnum("ElmoEnum", elmos)
				.WithEnum("DevicePosition", device_positions)
				.WithEnum("MovementEnum", movements)
				.WithEnum("ModelType", modelType)
				.Build();
			string f = $"{outputPath}/Constants.cs";
			string[] lines = File.ReadAllLines(f);
			string[] newLines = RemoveUnnecessaryLine(lines);
			File.WriteAllLines(f, newLines);
		}

		#endregion

		#region Stuff I'm ashamed of

		//  I swear I didn't find a way for codedom to do this :( 
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

		#endregion
	}
}