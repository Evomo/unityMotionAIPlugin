using UnityEngine;

namespace MotionAI.Core.Controller {
	public class MotionAIController : MonoBehaviour {
		[SerializeField] private string _deviceId;

		public void setDeviceId(string id) {
			_deviceId = id;
		}
	}
}