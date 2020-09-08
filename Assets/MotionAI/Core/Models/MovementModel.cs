using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace MotionAI.Core.Models {
	[CreateAssetMenu(menuName = "Evomo/Create Model", fileName = "Model")]

	public class MovementModel : ScriptableObject {
		public List<VoidEvent> movementEvents;

		public string modelName;
	}
}