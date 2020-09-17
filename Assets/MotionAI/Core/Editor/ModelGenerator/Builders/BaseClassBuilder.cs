using System;
using System.CodeDom;
using System.Globalization;
using MotionAI.Core.Util;

namespace MotionAI.Core.Editor.ModelGenerator.Builders {
	public partial class CustomClassBuilder {
		public CodeTypeDeclaration targetClass;
		protected CodeCompileUnit targetUnit;
		protected CodeNamespace codeNamespace;


		protected CustomClassBuilder(string name) {
			string cleanName = name.ToClassCase();
			targetUnit = new CodeCompileUnit();
			targetClass = new CodeTypeDeclaration(cleanName);


			codeNamespace = new CodeNamespace("MotionAI.Core.Models.Generated");
			codeNamespace.Types.Add(targetClass);

			targetUnit.Namespaces.Add(codeNamespace);
			// codeNamespace.Types.Add(targetClass);
		}
	}
}