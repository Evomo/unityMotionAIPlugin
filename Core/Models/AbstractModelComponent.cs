using System.Collections.Generic;
using MotionAI.Core.Controller;
using MotionAI.Core.Models.Generated;
using MotionAI.Core.POCO;
using UnityEngine;

namespace MotionAI.Core.Models {
	[DisallowMultipleComponent]
	[RequireComponent(typeof(MotionAIController))]
	public abstract class AbstractModelComponent : MonoBehaviour {
		[HideInInspector] public ModelBuildMeta chosenBuild;

		public virtual string modelName => "Abstract model";

		[Tooltip("Gaming models react automatically to each Elemental Movement, Fitness only classifies complete movements")]
		public ModelType modelType;

		public abstract List<MoveHolder> GetMoveHolders();
		public abstract List<ModelBuildMeta> GetAvailableTypes();
	}
}