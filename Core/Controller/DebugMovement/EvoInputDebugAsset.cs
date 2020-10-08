using System;
using System.Collections.Generic;
using System.Linq;
using MotionAI.Core.Models.Generated;
using MotionAI.Core.POCO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace MotionAI.Core.Controller.DebugMovement {
	[Serializable]
	public class InputDebugElmoContainer : FakeEvoInput {
		public ElmoEnum val;

		public override BridgeMessage InvokeMovement(string fakeDeviceId) {
			BridgeMessage msg = new BridgeMessage();
			msg.elmo = new ElementalMovement {
				typeID = val,
				typeLabel = val.ToString()
			};
			msg.deviceID = fakeDeviceId;

			return msg;
		}
	}

	[Serializable]
	public class InputDebugMoveContainer : FakeEvoInput {
		public MovementEnum val;

		public override BridgeMessage InvokeMovement(string deviceId) {
			BridgeMessage msg = new BridgeMessage();
			msg.movement = new EvoMovement {
				typeID = val,
				typeLabel = val.ToString()
			};
			msg.deviceID = deviceId;
			return msg;
		}
	}

	public abstract class FakeEvoInput {
		public KeyCode keycode;
		public abstract BridgeMessage InvokeMovement(string fakeDeviceId);

		public bool TryInvoke(string fakeDeviceId) {
			bool canInvoke = Input.GetKeyDown(keycode);
			if (canInvoke) {
				MotionAIManager.Instance.Enqueue(InvokeMovement(fakeDeviceId));
			}

			return canInvoke;
		}
	}

	public class EvoInputDebugAsset : ScriptableObject {
		[Range(0, 2)] public float delay = .5f;
		public string fakeDeviceId = "global";
		public List<InputDebugMoveContainer> debugMovement;
		public List<InputDebugElmoContainer> debugElmo;


		private float _lastSuccesfulInput;
		private float TimeSinceLastInput => Time.time - _lastSuccesfulInput;
		private bool CanUseInput => TimeSinceLastInput >= delay;

		private void Awake() {
			_lastSuccesfulInput = -delay;
		}

		public void CheckInput() {
#if UNITY_EDITOR
			if (CanUseInput) {
				foreach (FakeEvoInput fei in debugMovement.Concat<FakeEvoInput>(debugElmo)) {
					bool wasInvoked = fei.TryInvoke(fakeDeviceId);
					_lastSuccesfulInput = wasInvoked ? Time.time : _lastSuccesfulInput;
					if (wasInvoked) break;
				}
			}
#endif
		}


#if UNITY_EDITOR
		[MenuItem("Evomo/MotionAI/InputDebugAsset")]
		public static void CreateDebugAsset() {
			EvoInputDebugAsset gs = CreateInstance<EvoInputDebugAsset>();
			AssetDatabase.CreateAsset(gs, $"Assets/{Application.productName}-debugger.asset");
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
#endif
	}
}