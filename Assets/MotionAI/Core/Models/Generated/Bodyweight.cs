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
    
    public class Bodyweight : AbstractModelComponent {
        [HideInInspector()]
        public Movements moves = new Movements();
        [HideInInspector()]
        public Metadata meta = new Metadata();
        public override string modelName {
            get {
                return "bodyweight";
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
            public MoveHolder bench_dip = new MotionAI.Core.POCO.MoveHolder("bench_dip", (MovementEnum)6, new ElmoEnum[]{(ElmoEnum)3, (ElmoEnum)4});
            public MoveHolder jumping_jack = new MotionAI.Core.POCO.MoveHolder("jumping_jack", (MovementEnum)5, new ElmoEnum[]{(ElmoEnum)95, (ElmoEnum)94});
            public MoveHolder mountain_climbers = new MotionAI.Core.POCO.MoveHolder("mountain_climbers", (MovementEnum)10, new ElmoEnum[]{(ElmoEnum)120, (ElmoEnum)121});
            public MoveHolder lunge = new MotionAI.Core.POCO.MoveHolder("lunge", (MovementEnum)12, new ElmoEnum[]{(ElmoEnum)114, (ElmoEnum)119});
            public MoveHolder burpee = new MotionAI.Core.POCO.MoveHolder("burpee", (MovementEnum)24, new ElmoEnum[]{(ElmoEnum)7, (ElmoEnum)8, (ElmoEnum)9, (ElmoEnum)10});
            public MoveHolder squat_jump = new MotionAI.Core.POCO.MoveHolder("squat_jump", (MovementEnum)19, new ElmoEnum[]{(ElmoEnum)187, (ElmoEnum)188});
            public MoveHolder high_knees = new MotionAI.Core.POCO.MoveHolder("high_knees", (MovementEnum)26, new ElmoEnum[]{(ElmoEnum)75, (ElmoEnum)74});
            public MoveHolder hip_raise = new MotionAI.Core.POCO.MoveHolder("hip_raise", (MovementEnum)28, new ElmoEnum[]{(ElmoEnum)76, (ElmoEnum)77});
            public MoveHolder calf_raise = new MotionAI.Core.POCO.MoveHolder("calf_raise", (MovementEnum)32, new ElmoEnum[]{(ElmoEnum)14, (ElmoEnum)13});
            public MoveHolder plank = new MotionAI.Core.POCO.MoveHolder("plank", (MovementEnum)33, new ElmoEnum[]{(ElmoEnum)126});
            public MoveHolder climbers = new MotionAI.Core.POCO.MoveHolder("climbers", (MovementEnum)34, new ElmoEnum[]{(ElmoEnum)16, (ElmoEnum)15});
            public MoveHolder plank_jack = new MotionAI.Core.POCO.MoveHolder("plank_jack", (MovementEnum)39, new ElmoEnum[]{(ElmoEnum)127, (ElmoEnum)128});
            public MoveHolder lunge_reverse = new MotionAI.Core.POCO.MoveHolder("lunge_reverse", (MovementEnum)41, new ElmoEnum[]{(ElmoEnum)115, (ElmoEnum)116});
            public MoveHolder good_morning = new MotionAI.Core.POCO.MoveHolder("good_morning", (MovementEnum)40, new ElmoEnum[]{(ElmoEnum)63, (ElmoEnum)62});
        }
        [Serializable()]
        public class Metadata {
            public ModelBuildMeta Chest = new MotionAI.Core.POCO.ModelBuildMeta("Chest", 0, 1915, "bodyweight");
        }
    }
}
