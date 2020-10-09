using System;
using System.Collections.Generic;
using System.Linq;
using MotionAI.Core.Models;
using MotionAI.Core.Models.Generated;
using MotionAI.Core.POCO;
using UnityEngine;

namespace MotionAI.Core.Controller.DebugMovement {
	[RequireComponent(typeof(MotionAIController))]
	public class MotionAIControlDebugger : MonoBehaviour {
		public EvoInputDebugAsset debugAsset;

		public void GenerateValues() {
			if (debugAsset != null) {
				MotionAIController maic = GetComponent<MotionAIController>();
				AbstractModelComponent model = maic.modelManager.model;
				if (model) {
					debugAsset.debugMovement.Clear();
					debugAsset.debugElmo.Clear();
					HashSet<ElmoEnum> elmos = new HashSet<ElmoEnum>();
					HashSet<MovementEnum> moves = new HashSet<MovementEnum>();
					foreach (MoveHolder mh in model.GetMoveHolders()) {
						moves.Add(mh.id);
						elmos.UnionWith(mh.elmos);
					}

					debugAsset.debugElmo = elmos.Select(e => new InputDebugElmoContainer(e)).ToList();
					debugAsset.debugMovement = moves.Select(e => new InputDebugMoveContainer(e)).ToList();
				}
			}
		}

		private void Update() {
			if (debugAsset != null) {
				debugAsset.CheckInput();
			}
		}
	}
}