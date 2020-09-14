using System;
using UnityEngine;

namespace MotionAI {
	
	public class UtilHelper {
		

		public enum EventSource {
			app,
			manual
		}

		public enum FailureType {
			toLess,
			toMuch
		}


		
		public static string ToCustomOrientation(DeviceOrientation dvO) {
			switch (dvO) {
				case DeviceOrientation.LandscapeLeft:
					return "buttonLeft";
				case DeviceOrientation.LandscapeRight:
					return "buttonRight";

				default:
					return "buttonDown";
			}
		}
	}
}