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
	public class InternalClassBuilder : BaseClassBuilder {
		private readonly CustomClassBuilder _external;

		public InternalClassBuilder(string cname, CustomClassBuilder external) : base(cname) {
			_external = external;

			targetClass.TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed;
		}

		private InternalClassBuilder WithReferenceTo(string cleanString) {
			CodeMemberField s = new CodeMemberField();
//			s.Type = new CodeTypeReference(initialValue.GetType());


			return this;
		}

		public InternalClassBuilder WithReadOnlyField<T>(string name, T initialValue) {
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

		public InternalClassBuilder WithElmos(List<string> elmos) {
			foreach (string elmo in elmos) {
				try {
					string cleanString = elmo.CleanFromDB();
					ElmoEnum val = (ElmoEnum) Enum.Parse(typeof(ElmoEnum), cleanString);

					WithReadOnlyField(elmo, val);
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

	
		public InternalClassBuilder WithMovements(List<string> movements) {
			Type movementClass = typeof(Movements);

			foreach (string move in movements) {
				try {
					TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

					string cleanString = textInfo.ToTitleCase(move.CleanFromDB());
					var member = movementClass.GetMember(cleanString);

					WithReferenceTo(cleanString);
				}
				catch (ArgumentException e) {
					Debug.LogError(e);
				}
			}

			return this;
		}


		public static implicit operator CustomClassBuilder(InternalClassBuilder b) => b._external;

		public override void Build() {
			_external.Build();
		}
	}
}