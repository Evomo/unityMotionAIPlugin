using System;
using System.Collections.Generic;
using System.Linq;
using MotionAI.Core.Models.Generated;
using MotionAI.Core.POCO;
using UnityEngine;

namespace MotionAI.Core.Models {
	[Serializable]
	public class MoveHolder {
		public readonly string name;
		public readonly MovementEnum id;
		public OnMovementEvent onMove;

		public readonly HashSet<ElmoEnum> elmos;

		public MoveHolder(string mvName, MovementEnum val, ElmoEnum[] elmos = null) {
			name = mvName;
			id = val;
			onMove = new OnMovementEvent();
			this.elmos = new HashSet<ElmoEnum>(elmos ?? Array.Empty<ElmoEnum>());
		}

		public MoveHolder() {
		}
	}
}