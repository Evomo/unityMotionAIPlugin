using System;
using MotionAI.Core.Controller;
using MotionAI.Core.Models.Generated;
using UnityEngine;

namespace MotionAI.Core.POCO {
	#region Debug Containters

	[Serializable]
	public class InputDebugElmoContainer : FakeEvoInput {
		public ElmoEnum val;


		public InputDebugElmoContainer(ElmoEnum v) {
			name = v.ToString();
			val = v;
		}

		public override BridgeMessage PrepareMessage(string fakeDeviceId) {
			BridgeMessage msg = StartDebugMessage();
			msg.elmo = new ElementalMovement {
				// typeID = val,
				typeLabel = val.ToString()
			};
			msg.deviceID = fakeDeviceId;

			return msg;
		}
	}

	[Serializable]
	public class InputDebugMoveContainer : FakeEvoInput {
		public MovementEnum val;

		public InputDebugMoveContainer(MovementEnum m) {
			name = m.ToString();
			val = m;
		}

		public override BridgeMessage PrepareMessage(string deviceId) {
			BridgeMessage msg = StartDebugMessage();
			msg.movement = new EvoMovement {
				typeID = val,
				typeLabel = val.ToString()
			};
			msg.deviceID = deviceId;
			return msg;
		}
	}

	public abstract class FakeEvoInput {
		[HideInInspector] public string name;
		public KeyCode keycode;
		[Range(0, 3)] public float delay = .5f;

		protected BridgeMessage StartDebugMessage() {
			return new BridgeMessage {
				message = new Message(),
				elmo = new ElementalMovement(),
				movement = new EvoMovement()
			};
		}

		public abstract BridgeMessage PrepareMessage(string fakeDeviceId);

		public bool CheckKeycode => Input.GetKeyDown(keycode);
	}

	#endregion
}