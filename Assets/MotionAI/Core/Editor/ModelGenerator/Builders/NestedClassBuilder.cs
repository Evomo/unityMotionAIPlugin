using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;
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


			// targetClass.Members.Add(s);
			// return this;


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

			WithReadOnlyField("movementName", mv.name);
			WithReadOnlyField("id", val);
			WithElmos(mv.elmos);
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