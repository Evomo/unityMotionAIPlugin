using MotionAI.Core.Controller.DebugMovement;

namespace MotionAI.Core.Editor.CustomEditor {
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(MotionAIControlDebugger))]
	public class MotionAIControlDebuggerEditor : Editor {
		override public void OnInspectorGUI() {
			MotionAIControlDebugger comp = (MotionAIControlDebugger) target;


			if (comp.debugAsset == null) {
				if (GUILayout.Button("Create Asset")) {
					EvoInputDebugAsset deb = EvoInputDebugAsset.CreateDebugAsset();
					comp.debugAsset = deb;
					comp.GenerateValues();
				}
			}
			else {
				if (GUILayout.Button("Populate with Model Events")) {
					comp.GenerateValues();
				}
			}

			DrawDefaultInspector();
		}
	}
}