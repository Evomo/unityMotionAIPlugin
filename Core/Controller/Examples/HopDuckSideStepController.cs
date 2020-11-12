using MotionAI.Core.Models.Generated;
using MotionAI.Core.POCO;

namespace MotionAI.Core.Controller.Examples {
	/// <summary>
	/// Example controller used to show how to create custom controllers, and gives you a quick way to create 4 way movement controllers
	/// </summary>
	public class HopDuckSideStepController : MotionAIController {
		private ElementalMovement _lastElmo;

		public OnElmoEvent jump, duck, left, right;

		protected override void HandleMovement(EvoMovement msg) {
			msg.elmos.ForEach(HandleElmo);
		}

		private void HandleElmo(ElementalMovement elementalMovement) {
			if (!elementalMovement.rejected) {
				switch (elementalMovement.typeID) {
					case ElmoEnum.hop_single_up:
						jump.Invoke(elementalMovement);
						break;
					case ElmoEnum.duck_up:
						duck.Invoke(elementalMovement);
						break;
					case ElmoEnum.side_step_left_up:
						left.Invoke(elementalMovement);
						break;
					case ElmoEnum.side_step_right_up:
						right.Invoke(elementalMovement);
						break;
				}


				// rescue rejected elmo
				if (_lastElmo.rejected) {
					switch (_lastElmo.typeID) {
						case ElmoEnum.duck_down:
							RecoverElmo(elementalMovement, duck);
							break;
						case ElmoEnum.side_step_left_down:
							RecoverElmo(elementalMovement, right);
							break;
						case ElmoEnum.side_step_right_down:
							RecoverElmo(elementalMovement, left);
							break;
						case ElmoEnum.hop_single_down:
							RecoverElmo(elementalMovement, jump);
							break;
					}
				}
			}

			_lastElmo = elementalMovement;
		}

		private void RecoverElmo(ElementalMovement elementalMovement, OnElmoEvent callback) {
			ElmoEnum lastOpposite = DownOpposite(_lastElmo.typeID);
			if (elementalMovement.typeID == lastOpposite) {
				callback.Invoke(elementalMovement);
			}
		}

		private ElmoEnum DownOpposite(ElmoEnum e) {
			switch (e) {
				case ElmoEnum.duck_down:
					return ElmoEnum.duck_up;
				case ElmoEnum.hop_single_down:
					return ElmoEnum.hop_single_up;
				case ElmoEnum.side_step_left_down:
					return ElmoEnum.side_step_left_up;
				case ElmoEnum.side_step_right_down:
					return ElmoEnum.side_step_right_up;
				default:
					return ElmoEnum.heartUp;
			}
		}
	}
}