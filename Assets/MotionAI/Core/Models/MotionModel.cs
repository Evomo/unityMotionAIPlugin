namespace MotionAI.Core.Models {
	public abstract class MotionModel {
		public string modelName = "default";

		protected MotionModel(string modelName) {
			this.modelName = modelName;
		}
	}
}