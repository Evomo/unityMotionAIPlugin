using System;
using MotionAI.Core.Controller;
using MotionAI.Core.Controller.DebugMovement;
using MotionAI.Core.POCO;
using UnityEngine;
using UnityEngine.UI;

namespace MotionAI.Samples.InputDebugger {
	public class InputDebuggerManager : MonoBehaviour {
		public Text stopwatchText;
		public Text movementText;
		public MotionAIControlDebugger debugger;
		public MotionAIController debugController;

		private EvoMovement _lastMove;

		private void Start() {
			debugController = FindObjectOfType<MotionAIController>();
			debugger = FindObjectOfType<MotionAIControlDebugger>();

			debugController.OnEvoMovement.AddListener(movement => _lastMove = movement);
		}

		private void Update() {
			string t;

			if (_lastMove != null) {
				movementText.text = $"{JsonUtility.ToJson(_lastMove)}";
				string canPerform = debugger.debugAsset.CanPerform
					? "now!"
					: $"in {debugger.debugAsset.CanPerformUntil - Time.timeSinceLevelLoad} seconds";
				t =
					$"Last movement performed at {debugger.debugAsset.lastSuccesfulInput} can perform next move {canPerform}";
			}
			else {
				t = "Press any of the selected keys!";
			}

			stopwatchText.text = t;
		}
	}
}