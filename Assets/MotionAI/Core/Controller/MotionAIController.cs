using MotionAI.Core.POCO;
using UnityEngine;

namespace MotionAI.Core.Controller {
	public class MotionAIController : MonoBehaviour {
		[SerializeField] private string _deviceId;

		public void setDevice(string id) {
			_deviceId = id;
			
		}

		internal void HandleMovement(Movement msg) {
			throw new System.NotImplementedException();
		}
	}
}