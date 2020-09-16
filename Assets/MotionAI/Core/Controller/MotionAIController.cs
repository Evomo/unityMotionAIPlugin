using System.Linq;
using MotionAI.Core.POCO;
using UnityEngine;

namespace MotionAI.Core.Controller {
	public class MotionAIController : MonoBehaviour {
		public string DeviceId => _deviceID;

		public bool IsPaired => _isPaired;

		[SerializeField] private bool _isPaired;
		[SerializeField] private string _deviceID;

		[Tooltip(
			"Does this controller react to every movement or does it only subscribes to movements with the corresponding device ID?")]
		public bool isGlobal;

		public void SetDevice(string id, OnMovementEvent onMovement) {
			_deviceID = id;
			_isPaired = true;
			onMovement.AddListener(MovementCallBack);
		}

		private void MovementCallBack(MovementDto msg) {
			string diD = msg.elmos.First().deviceIdent;
			if (diD == DeviceId || isGlobal) {
				HandleMovement(msg);
			}
		}

		protected virtual void HandleMovement(MovementDto msg) {
			Debug.Log("MOVEMENT");
		}

		public void Unpair() {
			_deviceID = null;
			_isPaired = false;
		}
	}
}