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
        public static string modelType {
            get {
                return "gaming";
            }
        }
        public static int betaID {
            get {
                return 2115;
            }
        }
        public static int productionID {
            get {
                return 2129;
            }
        }
        public static string modelName {
            get {
                return "subway-surfer";
            }
        }
        public override System.Collections.Generic.List<MotionAI.Core.Models.MoveHolder> GetMoveHolders() {
            return moves.GetType().GetFields().Select(x => (MoveHolder)(x.GetValue(moves))).ToList();;
        }
        [Serializable()]
        public class Movements {
            public MoveHolder duck = new MotionAI.Core.Models.MoveHolder("duck", (MovementEnum)90, new ElmoEnum[]{(ElmoEnum)43, (ElmoEnum)44});
            public MoveHolder hop_single = new MotionAI.Core.Models.MoveHolder("hop_single", (MovementEnum)115, new ElmoEnum[]{(ElmoEnum)91, (ElmoEnum)84});
            public MoveHolder turn_90_right = new MotionAI.Core.Models.MoveHolder("turn_90_right", (MovementEnum)101, new ElmoEnum[]{(ElmoEnum)198});
            public MoveHolder turn_90_left = new MotionAI.Core.Models.MoveHolder("turn_90_left", (MovementEnum)102, new ElmoEnum[]{(ElmoEnum)197});
            public MoveHolder turn_90 = new MotionAI.Core.Models.MoveHolder("turn_90", (MovementEnum)103, new ElmoEnum[]{(ElmoEnum)196});
            public MoveHolder side_step = new MotionAI.Core.Models.MoveHolder("side_step", (MovementEnum)106, new ElmoEnum[]{(ElmoEnum)169, (ElmoEnum)174});
            public MoveHolder side_step_left = new MotionAI.Core.Models.MoveHolder("side_step_left", (MovementEnum)110, new ElmoEnum[]{(ElmoEnum)170, (ElmoEnum)171});
            public MoveHolder side_step_right = new MotionAI.Core.Models.MoveHolder("side_step_right", (MovementEnum)111, new ElmoEnum[]{(ElmoEnum)173, (ElmoEnum)172});
        }
    }
}