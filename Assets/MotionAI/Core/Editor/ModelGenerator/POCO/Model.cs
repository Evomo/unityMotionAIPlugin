using System;
using System.Collections.Generic;

namespace MotionAI.Core.Editor.ModelGenerator.POCO {
	[Serializable]
	public class Model {
		public string device_position;
		public List<string> movement_types;
		public int test_run;
		public string filename;
	}
}