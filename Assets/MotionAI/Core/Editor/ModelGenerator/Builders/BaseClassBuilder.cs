using System;
using System.CodeDom;
using System.Globalization;
using MotionAI.Core.Util;

namespace MotionAI.Core.Editor.ModelGenerator.Builders {
	public abstract class BaseClassBuilder {
		protected CodeTypeDeclaration targetClass;
		protected CodeCompileUnit targetUnit;
		private CodeNamespace codeNamespace;
		protected string className;

		public CodeTypeDeclaration TargetClass => targetClass;

		protected BaseClassBuilder(string name) {
			TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

			string cleanName = textInfo.ToTitleCase(name).CleanFromDB();
			this.className = cleanName;
			targetUnit = new CodeCompileUnit();
			targetClass = new CodeTypeDeclaration(cleanName);


			codeNamespace = new CodeNamespace($"MotionAI.Core.Models.{cleanName}");
			targetUnit.Namespaces.Add(codeNamespace);
				codeNamespace.Types.Add(targetClass);
		}


		public abstract void Build();
	}
}