using System;
using MotionAI.Core.Models.Generated;
using UnityEngine;
using UnityEngine.Events;

namespace MotionAI.Core.Models {
	[Serializable]
	public class MoveHolder {
		[HideInInspector] public string name;
		[HideInInspector] public MovementEnum id;
		public UnityEvent onMove;

		public MoveHolder(string mvName, MovementEnum val) {
			name = mvName;
			id = val;
			onMove = new UnityEvent();
		}
	}
}