using System.Collections.Generic;
using MotionAI.Core.Controller;
using MotionAI.Core.Models.Generated;
using MotionAI.Core.POCO;
using UnityEngine;

namespace MotionAI.Core.Models {
	[RequireComponent(typeof(MotionAIController))]
	public abstract class AbstractModelComponent : MonoBehaviour {
		public abstract List<MoveHolder> GetMoveHolders();
	}
}