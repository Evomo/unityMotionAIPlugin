using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace MotionAI.Core.Models {
	public interface IMovementModel {
	}

	public enum ModelNames {
		gaming_movements,
		gaming_movements_hdll,
		gaming_movements_lll,
		gaming_movements_hdtt,
	}


	public enum ElmoEnum {
		hop_single_up,
		duck_down,
		side_step_left_up,
		side_step_right_up,
		hop_single_down,
		side_step_left_down,
		side_step_right_down,
		duck_up,
		none
	}
}