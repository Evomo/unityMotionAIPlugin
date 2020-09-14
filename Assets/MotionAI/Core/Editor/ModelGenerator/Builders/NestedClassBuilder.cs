using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using MotionAI.Core.Models;
using MotionAI.Core.Util;
using UnityEngine;
using static MotionAI.Core.Models.Constants;

namespace MotionAI.Core.Editor.ModelGenerator.Builders {
	public partial class CustomClassBuilder {
		public CustomClassBuilder(string cname, CustomClassBuilder external) : base(cname) {
			_external = external;
			_internalClasses = new List<CustomClassBuilder>();


			targetClass.TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed;
		}


		public CustomClassBuilder WithReadOnlyField<T>(string name, T initialValue) {
			CodeMemberField s = new CodeMemberField();
			CodeTypeReferenceExpression typeRef = new CodeTypeReferenceExpression(initialValue.GetType());
			bool isEnum = initialValue.GetType().IsEnum;
			bool isString = (typeof(T) == typeof(String));

			s.Type = new CodeTypeReference(initialValue.GetType());
			s.Name = name.CleanFromDB();
			s.Attributes = MemberAttributes.Const | MemberAttributes.Public;
			s.InitExpression = isEnum
				? (CodeExpression) new CodeFieldReferenceExpression(typeRef, initialValue.ToString().CleanFromDB())
				: new CodePrimitiveExpression(isString ? (object) initialValue : Int32.Parse(initialValue.ToString()));
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


		public CustomClassBuilder CreateMovement(Movement mv) {
			WithReadOnlyField("name", mv.name);
			WithReadOnlyField("id", mv.id);
			WithElmos(mv.elmos);
			return _external;
		}
	}
}