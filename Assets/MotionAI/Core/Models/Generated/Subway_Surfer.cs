//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MotionAI.Core.Models.Generated {
    using UnityEngine;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MotionAI.Core.POCO;
    
    public class Subway_Surfer : AbstractModelComponent {
        public Movements moves;
        public Metadata meta;
        public static MotionAI.Core.Models.Generated.ModelType modelType {
            get {
                return MotionAI.Core.Models.Generated.ModelType.gaming;
            }
        }
        public static string modelName {
            get {
                return "subway-surfer";
            }
        }
        public override System.Collections.Generic.List<MotionAI.Core.POCO.MoveHolder> GetMoveHolders() {
            return moves.GetType().GetFields().Select(x => (MoveHolder)(x.GetValue(moves))).ToList();;
        }
        public override System.Collections.Generic.List<MotionAI.Core.POCO.ModelBuildMeta> GetAvailableTypes() {
            return meta.GetType().GetFields().Select(x => (ModelBuildMeta)(x.GetValue(meta))).ToList();;
        }
        [Serializable()]
        public class Movements {
            public MoveHolder duck = new MotionAI.Core.POCO.MoveHolder("duck", (MovementEnum)90, new ElmoEnum[]{(ElmoEnum)43, (ElmoEnum)44});
            public MoveHolder hop_single = new MotionAI.Core.POCO.MoveHolder("hop_single", (MovementEnum)115, new ElmoEnum[]{(ElmoEnum)91, (ElmoEnum)84});
            public MoveHolder turn_90_right = new MotionAI.Core.POCO.MoveHolder("turn_90_right", (MovementEnum)101, new ElmoEnum[]{(ElmoEnum)198});
            public MoveHolder turn_90_left = new MotionAI.Core.POCO.MoveHolder("turn_90_left", (MovementEnum)102, new ElmoEnum[]{(ElmoEnum)197});
            public MoveHolder turn_90 = new MotionAI.Core.POCO.MoveHolder("turn_90", (MovementEnum)103, new ElmoEnum[]{(ElmoEnum)196});
            public MoveHolder side_step = new MotionAI.Core.POCO.MoveHolder("side_step", (MovementEnum)106, new ElmoEnum[]{(ElmoEnum)169, (ElmoEnum)174});
            public MoveHolder side_step_left = new MotionAI.Core.POCO.MoveHolder("side_step_left", (MovementEnum)110, new ElmoEnum[]{(ElmoEnum)170, (ElmoEnum)171});
            public MoveHolder side_step_right = new MotionAI.Core.POCO.MoveHolder("side_step_right", (MovementEnum)111, new ElmoEnum[]{(ElmoEnum)173, (ElmoEnum)172});
        }
        [Serializable()]
        public class Metadata {
            public ModelBuildMeta Hand = new MotionAI.Core.POCO.ModelBuildMeta("Hand", 2129, 2115);
        }
    }
}
