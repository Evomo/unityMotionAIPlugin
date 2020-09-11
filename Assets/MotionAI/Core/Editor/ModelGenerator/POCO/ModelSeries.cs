using System;

namespace MotionAI.Core.Editor.ModelGenerator.POCO {
	[Serializable]
	public class ModelSeries {
		public string position;
		public string name;
		public Build builds;
	}

	[Serializable]
	public class Build {
		public int prod;
		public int beta;
	}
}