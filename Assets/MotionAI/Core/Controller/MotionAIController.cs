using System.Linq;
using MotionAI.Core.POCO;
using UniRx;
using UnityEngine;

namespace MotionAI.Core.Controller {
	public class MotionAIController : MonoBehaviour {
		[SerializeField] private string _deviceId;

		public void setDevice(string id, OnMovementEvent onMovement) {
			_deviceId = id;
			onMovement.AsObservable()
				.Where(movement => movement.elmos.First().deviceIdent == _deviceId)
				.Subscribe(HandleMovement);
		}

		public virtual void HandleMovement(Movement msg) {
			
			Debug.Log("MOVEMENT");
		}
	}
}