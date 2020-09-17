using System;
using MotionAI.Core.Models.Generated;
using MotionAI.Core.POCO;
using UnityEngine;
using UnityEngine.Events;

namespace MotionAI.Core.Models {
	[Serializable]
	public class MoveHolder {
		[HideInInspector] public string name;
		[HideInInspector] public MovementEnum id;
		public OnMovementEvent onMove;

		public MoveHolder(string mvName, MovementEnum val) {
			name = mvName;
			id = val;
			onMove = new OnMovementEvent();
		}

		public MoveHolder() {
		}
	}
}