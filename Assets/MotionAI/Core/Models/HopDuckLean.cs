using UnityEngine.Events;

namespace MotionAI.Core.Models {
	public class HopDuckLean : MotionModel {
		
		
		public UnityEvent hop, duck, leanLeft, leanRight;

		public HopDuckLean() : base("hopDuckLean") {
			
		}
	}
}