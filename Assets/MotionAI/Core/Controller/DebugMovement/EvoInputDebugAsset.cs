using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MotionAI.Core.Models.Generated;
using MotionAI.Core.POCO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif
namespace MotionAI.Core.Controller.DebugMovement {
	#region Scriptable Object

	public class EvoInputDebugAsset : ScriptableObject {
		public string fakeDeviceId = "global";


		public List<InputDebugMoveContainer> debugMovement;
		public List<InputDebugElmoContainer> debugElmo;


		public float lastSuccesfulInput;
		public Dictionary<KeyCode, FakeEvoInput> fakeEvoInputs;


		private float _canPerformUntil;

		public float CanPerformUntil {
			get => this._canPerformUntil;
			set => _canPerformUntil = Time.timeSinceLevelLoad + value;
		}

		public bool CanPerform => Time.timeSinceLevelLoad >= CanPerformUntil;

		private void SetDict(FakeEvoInput x) {
			if (fakeEvoInputs.ContainsKey(x.keycode)) {
				throw new DuplicateNameException($"Keycode {x.keycode.ToString()} was used in multiple movements");
			}

			if (x.keycode != KeyCode.None) {
				fakeEvoInputs[x.keycode] = x;
			}
		}

		public void Init() {
			fakeEvoInputs = new Dictionary<KeyCode, FakeEvoInput>();
			lastSuccesfulInput = Time.timeSinceLevelLoad;
			CanPerformUntil = 0;
			debugElmo.ForEach(x => SetDict(x));
			debugMovement.ForEach(x => SetDict(x));
		}


		public Tuple<BridgeMessage, float> CheckInput() {
#if UNITY_EDITOR
			if (CanPerform) {
				foreach (FakeEvoInput fei in fakeEvoInputs.Values) {
					if (fei.CheckKeycode) {
						BridgeMessage msg = fei.PrepareMessage(fakeDeviceId);
						lastSuccesfulInput = Time.timeSinceLevelLoad;
						CanPerformUntil = fei.delay;
						return new Tuple<BridgeMessage, float>(msg, fei.delay);
					}
				}
			}

#endif
			return new Tuple<BridgeMessage, float>(null, 0);
		}


#if UNITY_EDITOR
		[MenuItem("Evomo/MotionAI/InputDebugAsset")]
		public static EvoInputDebugAsset CreateDebugAsset() {
			EvoInputDebugAsset gs = CreateInstance<EvoInputDebugAsset>();
			AssetDatabase.CreateAsset(gs, $"Assets/{Application.productName}-debugger.asset");
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			return gs;
		}
#endif

		#endregion
	}
}