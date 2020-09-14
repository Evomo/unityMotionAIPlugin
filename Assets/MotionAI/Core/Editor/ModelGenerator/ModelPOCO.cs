using System;
using System.Collections.Generic;

namespace MotionAI.Core.Editor.ModelGenerator {
	public class ModelJson {
		public List<Model> models;
		public List<ModelSeries> model_series;
		public List<Movement> movement_types;
	}


	[Serializable]
	public class Movement {
		public int id;
		public string name;
		public List<string> elmos;
		
	}
	[Serializable]
	public class Model {
		public string device_position;
		public List<string> movement_types;
		public int test_run;
		public string filename;
	}
	
	
	[Serializable]
	public class ModelSeries {
		public string device_position;
		public string model_type;
		public string name;
		public Build builds;
	}

	[Serializable]
	public class Build {
		public int prod;
		public int beta;
	}
}