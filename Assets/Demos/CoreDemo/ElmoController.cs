using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using MotionAI.Core.Controller;
using MotionAI.Core.Models;
using MotionAI.Core.POCO;
using UnityEngine.Events;

namespace Demos.CoreDemo {
	public class ElmoController : MotionAIController {
		private ElementalMovement lastElmo;

		public UnityEvent jump, duck, left, right;

		public override void HandleMovement(Movement msg) {
			msg.elmos.ForEach(HandleElmo);
		}

		private void HandleElmo(ElementalMovement elementalMovement) {
			if (!elementalMovement.rejected) {
				switch (elementalMovement.typeID) {
				case ElmoEnum.hop_single_up:
					jump.Invoke();
					break;
				case ElmoEnum.duck_down:
					duck.Invoke();
					break;
				case ElmoEnum.side_step_left_up:
					left.Invoke();
					break;
				case ElmoEnum.side_step_right_up:
					right.Invoke();
					break;
				}

				// rescue rejected elmo
				if (lastElmo != null) {
					if (lastElmo.rejected && elementalMovement.rejected == false) {
						switch (lastElmo.typeID) {
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
			}

			lastElmo = elementalMovement;
		}

		private void RecoverElmo(ElementalMovement elementalMovement, UnityEvent callback) {
			ElmoEnum lastOpposite = DownOpossite(lastElmo.typeID);
			if (elementalMovement.typeID == lastOpposite) {
				callback.Invoke();
			}
		}

		private ElmoEnum DownOpossite(ElmoEnum e) {
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
				return ElmoEnum.none;
			}
		}
	}
}