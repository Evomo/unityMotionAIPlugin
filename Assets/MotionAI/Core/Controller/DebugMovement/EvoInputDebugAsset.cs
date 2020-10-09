using System.Collections.Generic;
using System.Linq;
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


		private float _lastSuccesfulInput;
		private float TimeSinceLastInput => Time.timeSinceLevelLoad - _lastSuccesfulInput;


		private void Awake() {
			_lastSuccesfulInput = 0;
			debugMovement = debugMovement == null
				? new List<InputDebugMoveContainer>()
				: debugMovement
					.OrderBy(x => x.delay).ToList();
			debugElmo = debugElmo == null
				? new List<InputDebugElmoContainer>()
				: debugElmo.OrderBy(x => x.delay).ToList();
		}

		public void CheckInput() {
			Debug.Log(TimeSinceLastInput);
#if UNITY_EDITOR
			foreach (FakeEvoInput fei in debugMovement.Concat<FakeEvoInput>(debugElmo)) {
				if (TimeSinceLastInput < fei.delay) break;
				if (fei.CanUseInput(TimeSinceLastInput)) {
					bool wasInvoked = fei.TryInvoke(fakeDeviceId);
					if (wasInvoked) {
						_lastSuccesfulInput = Time.timeSinceLevelLoad;
						break;
					}
				}
			}
#endif
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