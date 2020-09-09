using System.Linq;
using MotionAI.Core.POCO;
using UniRx;
using UnityEngine;

namespace MotionAI.Core.Controller {
	public class MotionAiController : MonoBehaviour {
		[SerializeReference]private string _deviceId;


		public bool isPaired {
			get;
			private set;
		}
		[Tooltip(
			"Does this controller react to every movement or does it only subscribes to movements with the corresponding device ID?")]
		public bool isGlobal;

		public void SetDevice(string id, OnMovementEvent onMovement) {
			_deviceId = id;
			onMovement.AddListener(MovementCallBack);
		}

		private void MovementCallBack(Movement msg) {
			string diD = msg.elmos.First().deviceIdent;
			if (diD == _deviceId || !isGlobal) {
				HandleMovement(msg);
			}
		}

		protected virtual void HandleMovement(Movement msg) {
			Debug.Log("MOVEMENT");
		}
	}
}