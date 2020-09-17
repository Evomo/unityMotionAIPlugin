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
			TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

			string cleanName = textInfo.ToTitleCase(name).CleanFromDB();
			targetUnit = new CodeCompileUnit();
			targetClass = new CodeTypeDeclaration(cleanName);


			
			codeNamespace = new CodeNamespace("MotionAI.Core.Models.Generated");
			targetUnit.Namespaces.Add(codeNamespace);
			// codeNamespace.Types.Add(targetClass);
		}
	}
}