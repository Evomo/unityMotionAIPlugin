using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using MotionAI.Core.Models;
using MotionAI.Core.Util;

namespace MotionAI.Core.Editor.ModelGenerator.Builders {
	public partial class CustomClassBuilder {
		private readonly string outputFile;
		private string _folderPath;

		private readonly CustomClassBuilder _external;

		private readonly List<CustomClassBuilder> _internalClasses;

		public CustomClassBuilder(string folderPath, string className) : this(className) {
			outputFile = $"{folderPath}/{className.ToClassCase()}.cs";
			this._folderPath = folderPath;

			_internalClasses = new List<CustomClassBuilder>();
		}


		public CustomClassBuilder WithEnum<T>(string enumName, Dictionary<string, T> dictEnum) {
			CodeTypeDeclaration type = new CodeTypeDeclaration(enumName);
			type.IsEnum = true;
			bool isString = typeof(T) == typeof(string);
			bool isEnumVal = typeof(T).IsEnum;

			foreach (var keyValuePair in dictEnum) {
				CodeMemberField f = new CodeMemberField(enumName, keyValuePair.Key.ToString().CleanFromDB());

				if (!isEnumVal) {
					if (!isString) {
						f.InitExpression = new CodePrimitiveExpression(keyValuePair.Value);
					}
				}
				else {
					CodeTypeReferenceExpression typeRef = new CodeTypeReferenceExpression(keyValuePair.Value.GetType());
					f.InitExpression = new CodeFieldReferenceExpression(typeRef, keyValuePair.Value.ToString());
				}

				type.Members.Add(f);
			}


			targetClass.Members.Add(type);
			return this;
		}

		public CustomClassBuilder WithMethod(string name, string codeSnippet, Type returnType) {
			CodeMemberMethod method = new CodeMemberMethod();
			method.Attributes =
				MemberAttributes.Public | MemberAttributes.Override;
			method.Name = name;
			method.ReturnType =
				new CodeTypeReference(returnType);

			CodeMethodReturnStatement returnStatement =
				new CodeMethodReturnStatement();

			returnStatement.Expression = new CodeSnippetExpression(codeSnippet);

			method.Statements.Add(returnStatement);
			targetClass.Members.Add(method);
			return this;
		}

		public CustomClassBuilder WithImport(string import) {
			CodeNamespaceImport i = new CodeNamespaceImport(import);
			codeNamespace.Imports.Add(i);
			return this;
		}

		public CustomClassBuilder WithEnum(string enumName, List<string> values) {
			Dictionary<string, string> enumDict = values.ToDictionary(x => x, x => x);
			WithEnum(enumName, enumDict);
			return this;
		}

		public CustomClassBuilder
			WithInternalClass(string cname, MemberAttributes attributes = MemberAttributes.Public) {
			CustomClassBuilder icb = new CustomClassBuilder(cname, this);
			icb.targetClass = new CodeTypeDeclaration(cname.ToClassCase());

			icb.targetClass.Attributes = attributes;

			_internalClasses.Add(icb);


			return icb;
		}


		private void AddInternalClasses() {
			foreach (CustomClassBuilder internalClass in _internalClasses) {
				internalClass.AddInternalClasses();
				if (_external == null) {
					targetClass.Members.Add(internalClass.targetClass);
				}
				else {
					targetClass.Members.Add(internalClass.targetClass);
				}
			}
		}

		public void Build() {
			if (_external != null) {
				_external.Build();
				return;
			}

			AddInternalClasses();


			CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
			CodeGeneratorOptions options = new CodeGeneratorOptions();
			options.BracingStyle = "block";
			options.BlankLinesBetweenMembers = false;
			using (StreamWriter sourceWriter = new StreamWriter(outputFile)) {
				provider.GenerateCodeFromCompileUnit(
					targetUnit, sourceWriter, options);
			}
		}
	}
}