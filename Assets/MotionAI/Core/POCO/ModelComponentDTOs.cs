using System;
using System.Collections.Generic;
using MotionAI.Core.Models.Generated;

namespace MotionAI.Core.POCO {
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

	[Serializable]
	public class ModelBuildMeta {
		public readonly int betaID;
		public readonly int prodID;
		public readonly DevicePosition devicePosition;
		public readonly string modelName;
		public ModelBuildMeta(string devicePosition, int prodID, int betaID, string name) {
			this.devicePosition = (DevicePosition)Enum.Parse(typeof(DevicePosition),devicePosition);
			this.prodID = prodID;
			this.betaID = betaID;
			modelName = name;
		}
	}
}