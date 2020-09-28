using System;
using System.Collections.Generic;

namespace MotionAI.Core.Editor.ModelGenerator {
	public class ModelJsonDump {
		public List<ModelJson> models;
		public List<ModelSeriesJson> model_series;
		public List<MovementJson> movement_types;
	}


	[Serializable]
	public class MovementJson {
		public int id;
		public string name;
		public List<string> elmos;
		
	}
	[Serializable]
	public class ModelJson {
		public string device_position;
		public List<string> movement_types;
		public int test_run;
		public string filename;
	}
	
	
	[Serializable]
	public class ModelSeriesJson {
		public string device_position;
		public string model_type;
		public string name;
		public BuildJson builds;
	}

	[Serializable]
	public class BuildJson {
		public int prod;
		public int beta;
	}
}