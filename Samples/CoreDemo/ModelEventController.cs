using MotionAI.Core.Controller;
using MotionAI.Core.Models;
using MotionAI.Core.Models.Generated;
using MotionAI.Core.POCO;
using UnityEngine;

namespace MotionAI.Samples.CoreDemo {
	public class ModelEventController : MotionAIController {
		private Subway_Surfer _movementModel;

		//Don't forget to override! Or the events won't work
		public override void Start() {
			base.Start();
			Debug.Log("Starting inspector controller");

			//Initialize the controller by casting the model to the controller you wish to use
			_movementModel = GetComponent<AbstractModelComponent>() as Subway_Surfer;

			//Once initialized you can access its members and events!

			if (_movementModel != null) {
				_movementModel.moves?.duck.onMove.AddListener(DuckCallBack);
			}

			//In this particular example I want to subscribe to ALL events except duck!
			foreach (MoveHolder moveHolder in modelManager.model.GetMoveHolders()) {
				if (moveHolder.id != MovementEnum.duck) {
					moveHolder.onMove.AddListener(AllEventsCallback);
				}
			}
		}

		private void AllEventsCallback(MovementDto e) {
			Debug.Log("I'M RECEIVING EVERY SINGLE EVENT");
		}

		private void DuckCallBack(MovementDto mov) {
			Debug.Log($"Got event {mov.typeLabel} in the inspector controller!");
		}

		protected override void HandleMovement(MovementDto msg) {
			//You can also override this method present in the MotionAIController base class and filter check each movement individually

			Debug.Log("Got a movement in the override!");
		}
	}
}