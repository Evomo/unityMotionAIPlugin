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
using Unity.Collections;
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


            public AbstractModelComponent ChangeModel(GameObject component) {
                if (modelComponent) DestroyImmediate(modelComponent);


                var comp = component.AddComponent(chosenModel);
                modelComponent = (AbstractModelComponent) comp;
                return modelComponent;
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

            public UtilHelper.EvomoDeviceOrientation deviceOrientation;

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
        [ReadOnly] public List<MoveHolder> trackedMoves;
        public ControllerSettings controllerSettings;
        public ModelManager modelManager;

        [HideInInspector] public OnMovementEvent onEvoMovement;

        public string DeviceId => controllerSettings.deviceId;
        public bool IsPaired => controllerSettings.isPaired;
        public UtilHelper.EvomoDeviceOrientation DeviceOrientation => controllerSettings.deviceOrientation;

        public bool IsGlobal {
            get => controllerSettings.isGlobal;
            set => controllerSettings.isGlobal = value;
        }

        #endregion

        #region Init

        private void FillDictionary() {
            _moveHolders = new Dictionary<MovementEnum, MoveHolder>();
            foreach (MoveHolder move in trackedMoves) {
                _moveHolders.Add(move.id, move);
            }
        }


        public virtual void Start() {
            MAIHelper.Log($"Starting MotionAIController {transform.gameObject.name}");
            if (modelManager.model == null) {
                AbstractModelComponent activeModel = gameObject.GetComponent<AbstractModelComponent>();
                if (activeModel != null) {
                    modelManager.SetFoundModel(activeModel);
                    if (trackedMoves == null) {
                        trackedMoves = new List<MoveHolder>();
                    }

                    
                    MAIHelper.Log($"Set model {activeModel.modelName}");
                }
                else {
                    throw new NullReferenceException("MotionAIController requires a model");
                }
            }
            onEvoMovement.AddListener(msg => HandleMovement(msg));
            FillDictionary();
        }

        public void SetDevice(string id, OnMovementEvent onMovement) {
            controllerSettings.Pair(id);
            if (onMovement != null) {
                onMovement.AddListener(MovementCallBack);
            }
            else {
                MAIHelper.Log($"Pair Controller - failed - onMovement is null {id}");
            }
            
        }


        public void Unpair() => controllerSettings.Unpair();

        public void ChangeModel(GameObject g) {
            AbstractModelComponent amc = modelManager?.ChangeModel(g);
            trackedMoves = amc.GetMoveHolders();

        }

        #endregion


        private void MovementCallBack(EvoMovement msg) {
            if (msg.deviceID == DeviceId || IsGlobal) {
                // ID Check
                MoveHolder holder = new MoveHolder();

                if (!string.IsNullOrEmpty(msg.typeLabel)) {
                    if (_moveHolders?.TryGetValue(msg.typeID, out holder) ?? false) {
                        onEvoMovement.Invoke(msg); // internal ovveridable movement
                        holder?.onMove.Invoke(msg); // per move holder 
                    }
                }
            }
        }

        protected virtual void HandleMovement(EvoMovement msg) {
            MAIHelper.Log("HandleMovement must be overwritten!");
        }
    }
}