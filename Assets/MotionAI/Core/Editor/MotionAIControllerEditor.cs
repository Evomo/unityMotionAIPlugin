using System;
using System.Collections.Generic;
using System.Linq;
using MotionAI.Core.Controller;
using MotionAI.Core.POCO;
using UnityEditor;
using UnityEngine;

namespace MotionAI.Core.Editor {
	[CustomEditor(typeof(MotionAIController), true)]
	public class MotionAIControllerEditor : UnityEditor.Editor {
		public int index = 0;

		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			MotionAIController controller = target as MotionAIController;


			//I love me some ifs <3 
			if (controller != null) {
				if (controller.modelManager.model != null) {
					List<ModelBuildMeta> choices = controller.modelManager.model.GetAvailableTypes();
					if (choices != null) {
						//TODO
						// EditorGUILayout.BeginHorizontal();
						// EditorGUILayout.LabelField("Model", GUILayout.Width(EditorGUIUtility.labelWidth - 4));
						// EditorGUILayout.ObjectField(controller.modelManager.chosenModel,typeof(TypedReference));
						//
						// EditorGUILayout.EndHorizontal();
						
						
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.LabelField("Device Position", GUILayout.Width(EditorGUIUtility.labelWidth - 4));

						index = EditorGUILayout.Popup(index,
							choices.Select(x => x.devicePosition.ToString()).ToArray());
						controller.modelManager.model.chosenBuild = choices[index];
						EditorGUILayout.EndHorizontal();
					}
				}

				if (controller.modelManager.CanChangeComponent) {
					if (GUILayout.Button("Change Model")) {
						if (controller.modelManager?.model != null) {
							if (!EditorUtility.DisplayDialog("Replace Model?",
								"Are you sure you want to replace the current model? All references will be lost",
								"Yes",
								"no")) return;
						}

						controller.modelManager?.ChangeModel(controller.gameObject);
					}
				}
			}
		}
	}
}