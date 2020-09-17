using MotionAI.Core.Controller;
using UnityEditor;
using UnityEngine;

namespace MotionAI.Core.Editor {
	[CustomEditor(typeof(MotionAIController), true)]
	public class ModelManagerEditor : UnityEditor.Editor {
		public override void OnInspectorGUI() {
			base.OnInspectorGUI();

			MotionAIController controller = target as MotionAIController;


			//I love me some ifs <3 
			if (controller != null) {
				if (!controller.modelManager.IsSameModel) {
					if (GUILayout.Button("Change Model")) {
						if (controller.modelManager?.model != null ) {
							if (!EditorUtility.DisplayDialog("Replace Model?",
								"Are you sure you want to replace the current model? All references will be lost", "Yes",
								"no")) return;
						}

						controller.modelManager?.ChangeModel(controller.gameObject);
					}	
				}
			}
		}
	}
}