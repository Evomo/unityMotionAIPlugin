using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MotionAI.Core.Models;
using MotionAI.Core.Models.Generated;
using MotionAI.Core.POCO;
using MotionAI.Core.Util;
using TypeReferences;
using UnityEngine;

namespace MotionAI.Core.Controller {
	public class MotionAIController : MonoBehaviour {
		#region Internal Classes

		[Serializable]
		public class ModelManager {
			public AbstractModelComponent model => modelComponent;

			[Inherits(typeof(AbstractModelComponent), ExcludeNone = true),
			 TypeOptions(Grouping = Grouping.ByNamespaceFlat)]
			public TypeReference chosenModel;


			[SerializeField] private AbstractModelComponent modelComponent;


			public bool CanChangeComponent => model == null || modelComponent.GetType() != chosenModel.Type;


			public void ChangeModel(GameObject component) {
				if (modelComponent) DestroyImmediate(modelComponent);


				var comp = component.AddComponent(chosenModel);
				modelComponent = (AbstractModelComponent) comp;
			}
		}


		[Serializable]
		public class ControllerSettings {
			[ShowOnly] public bool isPaired;
			[ShowOnly] public string deviceId;

			[Tooltip(
				"Does this controller react to every movement or does it only subscribe to movements with the corresponding device ID?")]
			public bool isGlobal = true;

			public void Pair(string id) {
				deviceId = id;
				isPaired = true;
			}


			public void Unpair() {
				deviceId = null;
				isPaired = false;
			}
		}

		#endregion

		#region Fields

		private Dictionary<MovementEnum, MoveHolder> _moveHolders;
		public ControllerSettings controllerSettings;
		public ModelManager modelManager;
		public string DeviceId => controllerSettings.deviceId;
		public bool IsPaired => controllerSettings.isPaired;

		public bool IsGlobal {
			get => controllerSettings.isGlobal;
			set => controllerSettings.isGlobal = value;
		}

		#endregion

		#region Init

		private void FillDictionary() {
			_moveHolders = new Dictionary<MovementEnum, MoveHolder>();
			foreach (MoveHolder move in modelManager.model.GetMoveHolders()) {
				_moveHolders[move.id] = move;
			}
		}


		public virtual void Start() {
			FillDictionary();
		}

		public void SetDevice(string id, OnMovementEvent onMovement) {
			controllerSettings.Pair(id);
			onMovement.AddListener(MovementCallBack);
		}


		public void Unpair() {
			controllerSettings.Unpair();
		}

		#endregion


		private void MovementCallBack(MovementDto msg) {
			string diD = msg.elmos.First().deviceIdent;
			if (diD == DeviceId || IsGlobal) {
				InvokeEvents(msg);
				HandleMovement(msg);
			}
		}

		private void InvokeEvents(MovementDto e) {
			MoveHolder holder = new MoveHolder();
			if (_moveHolders?.TryGetValue(e.typeID, out holder) ?? false) {
				holder?.onMove.Invoke(e);
			}
		}

		protected virtual void HandleMovement(MovementDto msg) { }
	}
}