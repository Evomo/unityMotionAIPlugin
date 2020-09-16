using System;
using TypeReferences;
using UnityEngine;

#if UNITY_EDITOR
using MotionAI.Core.Editor;
using MotionAI.Core.Util;
using UnityEditor;
	
#endif


namespace MotionAI.Core.Models {
	[ExecuteInEditMode]
	public class ModelManager : MonoBehaviour {
		[SerializeField]
		[Inherits(typeof(AbstractModelComponent), ExcludeNone = true),
		 TypeOptions(Grouping = Grouping.ByNamespaceFlat)]
		private TypeReference chosenModel;

		public AbstractModelComponent modelComponent;

		 private Type lastRef;


#if UNITY_EDITOR
		[Button]
		private void ChangeModel() {
			if (lastRef != chosenModel) {
				if (modelComponent) {
					DestroyImmediate(modelComponent);
				}

				var comp = gameObject.AddComponent(chosenModel) as AbstractModelComponent;
				modelComponent = comp;
				lastRef = chosenModel.Type;
			}

		}
#endif

	}
}