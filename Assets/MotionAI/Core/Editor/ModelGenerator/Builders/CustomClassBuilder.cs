using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;

namespace MotionAI.Core.Editor.ModelGenerator.Builders {
	public class CustomClassBuilder : BaseClassBuilder {
		private readonly string outputFile;
		private string _folderPath;


		private readonly List<InternalClassBuilder> _internalClasses;

		public CustomClassBuilder(string folderPath, string className) : base(className) {
			outputFile = $"{folderPath}/{className}.cs";
			this._folderPath = folderPath;
			this.className = className;

			_internalClasses = new List<InternalClassBuilder>();
		}


		public CustomClassBuilder WithEnum(string enumName, List<string> values) {
			CodeTypeDeclaration type = new CodeTypeDeclaration(enumName);
			type.IsEnum = true;

			foreach (var valueName in values) {
				// Creates the enum member
				CodeMemberField f = new CodeMemberField(enumName, valueName);
				type.Members.Add(f);
			}

			targetClass.Members.Add(type);
			return this;
		}

		public InternalClassBuilder WithInternalClass(string cname) {
			InternalClassBuilder icb = new InternalClassBuilder(cname, this);
			_internalClasses.Add(icb);

			return icb;
		}
		
		
		
		public override void Build() {
			CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
			CodeGeneratorOptions options = new CodeGeneratorOptions();
			options.BracingStyle = "block";
			options.BlankLinesBetweenMembers = false;

			foreach (InternalClassBuilder internalClass in _internalClasses) {
				targetClass.Members.Add(internalClass.TargetClass);
			}

			using (StreamWriter sourceWriter = new StreamWriter(outputFile)) {
				provider.GenerateCodeFromCompileUnit(
					targetUnit, sourceWriter, options);
			}
		}
	}
}