using MotionAI.Core.Controller;
using MotionAI.Core.Models;
using MotionAI.Core.Models.Generated;
using MotionAI.Core.POCO;
using UnityEngine;
using UnityEngine.UI;

namespace MotionAI.Samples.CoreDemo {
	public class ModelEventController : MotionAIController {
		private Bodyweight _movementModel;
		
		public Text DebugText;

		//Don't forget to override! Or the events won't work
		public override void Start() {
			base.Start();
			Debug.Log("Starting inspector controller");

			//Initialize the controller by casting the model to the controller you wish to use
			_movementModel = GetComponent<AbstractModelComponent>() as Bodyweight;

			//Once initialized you can access its members and events!

			if (_movementModel != null) {
				_movementModel.moves?.jumping_jack.onMove.AddListener(JJCallBack);
			}

			//In this particular example I want to subscribe to ALL events except duck!
			foreach (MoveHolder moveHolder in modelManager.model.GetMoveHolders()) {
				if (moveHolder.id != MovementEnum.duck) {
					moveHolder.onMove.AddListener(AllEventsCallback);
				}
			}
		}

		private void AllEventsCallback(EvoMovement e) {
			Debug.Log($"Movement{e.typeLabel}");
			DebugText.text += $"/nMovement{e.typeLabel}";
		}

		private void JJCallBack(EvoMovement mov) {
			Debug.Log($"JumpingJack");
			DebugText.text += $"/nJumpingJack";
		}

		protected override void HandleMovement(EvoMovement msg) {
			//You can also override this method present in the MotionAIController base class and filter check each movement individually
			Debug.Log($"HandleMovement {msg.typeLabel}");
			DebugText.text += $"/nHandleMovement {msg.typeLabel}";
		}
	}
}