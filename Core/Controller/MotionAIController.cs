using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MotionAI.Core.Controller.DebugMovement;
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


			[SerializeField, ShowOnly] private AbstractModelComponent modelComponent;


			public bool CanChangeComponent => model == null || modelComponent.GetType() != chosenModel.Type;


			public void ChangeModel(GameObject component) {
				if (modelComponent) DestroyImmediate(modelComponent);


				var comp = component.AddComponent(chosenModel);
				modelComponent = (AbstractModelComponent) comp;
			}


			public void SetFoundModel(AbstractModelComponent activeModel) {
				this.chosenModel = model.GetType();
				this.modelComponent = activeModel;
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

		public UtilHelper.EvomoDeviceOrientation deviceOrientation;
		public ControllerSettings controllerSettings;
		public ModelManager modelManager;
		
		[HideInInspector]
		public OnMovementEvent OnEvoMovement;
		
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
			MAIHelper.Log($"Starting MotionAIController {transform.gameObject.name}");
			if (modelManager.model == null) {
				AbstractModelComponent activeModel = gameObject.GetComponent<AbstractModelComponent>();
				if (activeModel != null) {
					modelManager.SetFoundModel(activeModel);
					MAIHelper.Log($"Set model {activeModel.modelName}");
				}
				else {
					throw new NullReferenceException("MotionAIController requires a model");
				}
			}

			FillDictionary();
		}

		public void SetDevice(string id, OnMovementEvent onMovement) {
			MAIHelper.Log($"Pair Controller - deviceID {id} OnMovementIsNotNUll: {onMovement != null}");
			controllerSettings.Pair(id);
			if (onMovement != null)
			{
				onMovement.AddListener(MovementCallBack);
				
			}
			else
			{
				MAIHelper.Log($"Pair Controller - failed - onMovement is null {id}");
			}
		}


		public void Unpair() {
			controllerSettings.Unpair();
		}

		#endregion


		private void MovementCallBack(EvoMovement msg) {
			MAIHelper.Log($"MovementCallBack");
			if (msg.deviceID == DeviceId || IsGlobal) {
				InvokeEvents(msg);
				
				HandleMovement(msg);
			}
		}

		private void InvokeEvents(EvoMovement e) {
			OnEvoMovement.Invoke(e);
			MoveHolder holder = new MoveHolder();
			if (!string.IsNullOrEmpty(e.typeLabel)) {
				if (_moveHolders?.TryGetValue(e.typeID, out holder) ?? false) {
					holder?.onMove.Invoke(e);
				}
			}
		}


		protected virtual void HandleMovement(EvoMovement msg)
		{
			MAIHelper.Log("HandleMovement must be overwritten!");
		}
	}
}