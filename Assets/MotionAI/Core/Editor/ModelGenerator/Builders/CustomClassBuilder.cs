using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;

namespace MotionAI.Core.Editor.ModelGenerator.Builders {
	public partial class CustomClassBuilder : BaseClassBuilder {
		private readonly string outputFile;
		private string _folderPath;

		private readonly CustomClassBuilder _external;

		private readonly List<CustomClassBuilder> _internalClasses;

		public CustomClassBuilder(string folderPath, string className) : base(className) {
			outputFile = $"{folderPath}/{className}.cs";
			this._folderPath = folderPath;
			this.className = className;

			_internalClasses = new List<CustomClassBuilder>();
		}


		public CustomClassBuilder ToParentBuilder() {
			return _external ?? this;
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

		public CustomClassBuilder WithInternalClass(string cname) {
			CustomClassBuilder icb = new CustomClassBuilder(cname, this);
			_internalClasses.Add(icb);

			
			return icb;
		}


		private void AddInternalClasses() {
			foreach (CustomClassBuilder internalClass in _internalClasses) {
				internalClass.AddInternalClasses();
				targetClass.Members.Add(internalClass.TargetClass);
			}
		}
		public override void Build() {
			if (_external != null) {
				_external.Build();
				return;
			}
			CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
			CodeGeneratorOptions options = new CodeGeneratorOptions();
			options.BracingStyle = "block";
			options.BlankLinesBetweenMembers = false;
			AddInternalClasses();
			using (StreamWriter sourceWriter = new StreamWriter(outputFile)) {
				provider.GenerateCodeFromCompileUnit(
					targetUnit, sourceWriter, options);
			}
		}
	}
}