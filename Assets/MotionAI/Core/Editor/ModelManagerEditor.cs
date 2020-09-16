using System;
using System.Linq;
using System.Reflection;
using MotionAI.Core.Models;
using UnityEditor;
using UnityEngine;

namespace MotionAI.Core.Editor {

	[CustomEditor(typeof(ModelManager), true)]
	public class ModelManagerEditor : UnityEditor.Editor {
		
		public override void OnInspectorGUI() {
			base.OnInspectorGUI();
			DrawButtonsIfRequested();
		}
		

		private void DrawButtonsIfRequested() {
			// Loop through all methods with no parameters
			var methods = target.GetType()
				.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
				.Where(m => m.GetParameters().Length == 0);

			foreach (var method in methods) {
				// Get the ButtonAttribute on the method (if any)
				var ba = (ButtonAttribute) Attribute.GetCustomAttribute(method, typeof(ButtonAttribute));

				if (ba == null)
					continue;

				var buttonName = ObjectNames.NicifyVariableName(method.Name);

				if (GUILayout.Button(buttonName))
					method.Invoke(target, null);
			}
		}
	}
}