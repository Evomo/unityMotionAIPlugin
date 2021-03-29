using UnityEngine;

namespace MotionAI.Core.POCO {
	public class UtilHelper {
		public enum EventSource {
			app,
			manual
		}


		public enum FailureType {
			toLess,
			toMuch
		}

		public enum MovementType {
			Jump,
			Duck,
			Left,
			Right,
			Tab
		}


		public enum EvomoDeviceOrientation {
			buttonRight,
			buttonLeft,
			buttonDown,
			buttonUp
		}
		
		public enum EvomoSensorType {
			Smartphone,
			Movesense
		}
	}
}