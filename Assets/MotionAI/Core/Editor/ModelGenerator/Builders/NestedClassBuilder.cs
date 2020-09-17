using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MotionAI.Core.Models;
using MotionAI.Core.Models.Generated;
using MotionAI.Core.Util;
using UnityEngine;

namespace MotionAI.Core.Editor.ModelGenerator.Builders {
	public partial class CustomClassBuilder {
		public CustomClassBuilder(string cname, CustomClassBuilder external) : this(cname) {
			_external = external;
			_internalClasses = new List<CustomClassBuilder>();

			targetClass.TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed;
		}


		public CustomClassBuilder WithObject(string name, string typeName, CodeObjectCreateExpression cobe) {
			CodeMemberProperty s = new CodeMemberProperty();


			CodeMemberField objectValueField = new CodeMemberField();
			objectValueField.Attributes = MemberAttributes.Public;

			objectValueField.Name = name;
			if (cobe != null) {
				objectValueField.InitExpression = cobe;
			}

			objectValueField.Type = new CodeTypeReference(typeName);
			targetClass.Members.Add(objectValueField);

			return this;
		}

		public CustomClassBuilder WithObject(string name, string typeName) {
			WithObject(name, typeName, null);
			return this;
		}

		public CustomClassBuilder WithReadOnlyField<T>(string name, T initialValue,
			MemberAttributes attributes = MemberAttributes.Public | MemberAttributes.Static | MemberAttributes.Final) {
			CodeMemberProperty s = new CodeMemberProperty();

			CodeTypeReferenceExpression typeRef = new CodeTypeReferenceExpression(initialValue.GetType());
			bool isEnum = initialValue.GetType().IsEnum;
			bool isString = typeof(T) == typeof(string);

			s.Name = name.CleanFromDB();
			CodeExpression fieldReferenceExpression = isEnum
				? (CodeExpression) new CodeFieldReferenceExpression(typeRef, initialValue.ToString().CleanFromDB())
				: new CodePrimitiveExpression(isString ? (object) initialValue : Int32.Parse(initialValue.ToString()));


			s.Attributes = attributes;
			s.Name = name.CleanFromDB();
			s.HasGet = true;
			s.Type = new CodeTypeReference(initialValue.GetType());
			s.GetStatements.Add(new CodeMethodReturnStatement(fieldReferenceExpression));
			targetClass.Members.Add(s);
			return this;
		}

		public CustomClassBuilder WithElmos(List<string> elmos) {
			CustomClassBuilder elmoClass = this.WithInternalClass("Elmos");
			foreach (string elmo in elmos) {
				try {
					string cleanString = elmo.CleanFromDB();
					ElmoEnum val = (ElmoEnum) Enum.Parse(typeof(ElmoEnum), cleanString);

					elmoClass.WithReadOnlyField(elmo, val);
				}
				catch (ArgumentException e) {
					Debug.LogError(e);
				}
			}

			return this;
		}


		public CustomClassBuilder WithEvents(List<string> eventNames) {
			return this;
		}

		public CustomClassBuilder CreateMovement(MovementJson mv) {
			MovementEnum val = (MovementEnum) mv.id;

			string enumSnippet =
				$"new ElmoEnum[]{{{string.Join(", ", mv.elmos.Select(x => (ElmoEnum) Enum.Parse(typeof(ElmoEnum), x.CleanFromDB())).Select(x => $"(ElmoEnum){(int) x}"))}}}";

			CodeExpression[] p = new CodeExpression[] {
				new CodePrimitiveExpression(mv.name),
				new CodeSnippetExpression($"(MovementEnum){mv.id}"),
				new CodeSnippetExpression(enumSnippet),
			};
			var cobe = new CodeObjectCreateExpression(typeof(MoveHolder), p);
			WithObject(mv.name, "MoveHolder", cobe);
			return _external;
		}

		public CustomClassBuilder InheritsFrom(String type) {
			targetClass.BaseTypes.Add(type);
			return this;
		}

		public CustomClassBuilder WithCustomAttribute(string attributeName) {
			targetClass.CustomAttributes.Add(new CodeAttributeDeclaration(attributeName));
			return this;
		}
	}
}