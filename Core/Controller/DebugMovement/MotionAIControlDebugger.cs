using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MotionAI.Core.Models;
using MotionAI.Core.Models.Generated;
using MotionAI.Core.POCO;
using UnityEngine;

namespace MotionAI.Core.Controller.DebugMovement {
    [RequireComponent(typeof(MotionAIController))]
    public class MotionAIControlDebugger : MonoBehaviour {
        public EvoInputDebugAsset debugAsset;

        public void GenerateValues() {
            if (debugAsset != null) {
                MotionAIController maic = GetComponent<MotionAIController>();
                AbstractModelComponent model = maic.modelManager.model;
                if (model) {
                    debugAsset.debugMovement = new List<InputDebugMoveContainer>();
                    debugAsset.debugElmo = new List<InputDebugElmoContainer>();
                    HashSet<ElmoEnum> elmos = new HashSet<ElmoEnum>();
                    HashSet<MovementEnum> moves = new HashSet<MovementEnum>();
                    foreach (MoveHolder mh in model.GetMoveHolders()) {
                        moves.Add(mh.id);
                        elmos.UnionWith(mh.elmos);
                    }

                    debugAsset.debugElmo = elmos.Select(e => new InputDebugElmoContainer(e)).ToList();
                    debugAsset.debugMovement = moves.Select(e => new InputDebugMoveContainer(e)).ToList();
                }
            }
        }


        private void Awake() {
            debugAsset.Init();
        }

        private void Update() {
            if (debugAsset != null) {
                if (debugAsset.CanPerform) {
                    (BridgeMessage msg, float delay) = debugAsset.CheckInput();
                    if (msg != null) {
                        StartCoroutine(SendMovement(msg, delay));
                    }
                }
            }
        }

        private IEnumerator SendMovement(BridgeMessage msg, float delay) {
            yield return new WaitForSeconds(delay);
            MotionAIManager.Instance.Enqueue(msg);
        }
    }
}