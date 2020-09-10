using System.Linq;
using MotionAI.Core.POCO;
using UnityEngine;

namespace MotionAI.Core.Controller {
	public class MotionAiController : MonoBehaviour {
		public string DeviceId { get; private set; }
		public bool IsPaired { get; private set; }

		[Tooltip(
			"Does this controller react to every movement or does it only subscribes to movements with the corresponding device ID?")]
		public bool isGlobal;

		public void SetDevice(string id, OnMovementEvent onMovement) {
			DeviceId = id;
			IsPaired = true;
			onMovement.AddListener(MovementCallBack);
		}

		private void MovementCallBack(Movement msg) {
			string diD = msg.elmos.First().deviceIdent;
			if (diD == DeviceId || !isGlobal) {
				HandleMovement(msg);
			}
		}

		protected virtual void HandleMovement(Movement msg) {
			Debug.Log("MOVEMENT");
		}

		public void Unpair() {
			DeviceId = null;
			IsPaired = false;
		}
	}
}