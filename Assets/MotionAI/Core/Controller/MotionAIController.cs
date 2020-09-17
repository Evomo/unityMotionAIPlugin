using System;
using System.Linq;
using MotionAI.Core.Models;
using MotionAI.Core.POCO;
using TypeReferences;
using UnityEngine;

namespace MotionAI.Core.Controller {
	public class MotionAIController : MonoBehaviour {
		[Serializable]
		public class ModelManager {
			[Inherits(typeof(AbstractModelComponent), ExcludeNone = true),
			 TypeOptions(Grouping = Grouping.ByNamespaceFlat)]
			public TypeReference chosenModel;
			public AbstractModelComponent modelComponent;

			private Type _lastRef;
			public bool IsSameModel => _lastRef == chosenModel;


			public void ChangeModel(GameObject component) {

					if (modelComponent) {
						DestroyImmediate(modelComponent);


					var comp = component.AddComponent(chosenModel) as AbstractModelComponent;
					modelComponent = comp;
					_lastRef = chosenModel.Type;
				}
			}
		}


		[Serializable]
		public class ControllerSettings {
			public bool isPaired;
			public string deviceId;

			[Tooltip(
				"Does this controller react to every movement or does it only subscribes to movements with the corresponding device ID?")]
			public bool isGlobal;

			public void Pair(string id) {
				deviceId = id;
				isPaired = true;
			}


			public void Unpair() {
				deviceId = null;
				isPaired = false;
			}
		}


		public ControllerSettings controllerSettings;
		public ModelManager modelManager;
		public string DeviceId => controllerSettings.deviceId;
		public bool IsPaired => controllerSettings.isPaired;

		public bool IsGlobal {
			get => controllerSettings.isGlobal;
			set => controllerSettings.isGlobal = value;
		}


		public void SetDevice(string id, OnMovementEvent onMovement) {
			controllerSettings.Pair(id);
			onMovement.AddListener(MovementCallBack);
		}


		private void MovementCallBack(MovementDto msg) {
			string diD = msg.elmos.First().deviceIdent;
			if (diD == DeviceId || IsGlobal) {
				HandleMovement(msg);
			}
		}

		protected virtual void HandleMovement(MovementDto msg) {
			Debug.Log("MOVEMENT");
		}

		public void Unpair() {
			controllerSettings.Unpair();
		}
	}
}