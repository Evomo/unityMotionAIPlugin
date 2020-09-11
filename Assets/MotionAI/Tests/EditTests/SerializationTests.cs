using System;
using System.Linq;
using MotionAI.Core.POCO;
using NUnit.Framework;
using UnityEngine;
using FluentAssertions;

namespace MotionAI.Tests.EditTests {
	public class SerializationTests {
		[Test]
		public void SerializeElementalMovement() {
			string debugText = @"{""typeID"" : 1,
         ""end"" : ""2020-09-08T00:11:39.513"",
         ""rejected"" : false,
         ""typeLabel"" : ""test"",
         ""start"" : ""2020-09-08T00:11:38.513"",
         ""deviceIdent"" : ""3DF671DD-47D6-4F21-ADE2-6C2AB52A9875""
      }";


			ElementalMovement serialized = JsonUtility.FromJson<ElementalMovement>(debugText);

			ElementalMovement actual = new ElementalMovement {
				typeID = (ElmoEnum) 1,
				rejected = false,
				typeLabel = "test",
				deviceIdent = "3DF671DD-47D6-4F21-ADE2-6C2AB52A9875"
			};

			actual.Should().BeEquivalentTo(serialized);
		}

		[Test]
		public void SerializeMovementWithoutElmo() {
			string debugText = @"{
			""gVelAmplitudePositive"" : 0.29999999999999999,
			""start"" : ""2020-09-08T00:11:38.513"",
			""amplitude"" : 0.10000000000000001,
			""durationPositive"" : 0.20000000000000001,
			""typeID"" : 1,
			""typeLabel"" : ""testType"",
			""end"" : ""2020-09-08T00:11:39.513"",
			""durationNegative"" : 0.20000000000000001,
			""gVelAmplitudeNegative"" : 0.29999999999999999
}";


			Movement serialized = JsonUtility.FromJson<Movement>(debugText);

			Movement actual = new Movement {
				gVelAmplitudePositive = 0.29999999999999999f,
				amplitude = 0.10000000000000001f,
				durationPositive = 0.20000000000000001f,
				typeID = (MovementEnum) 1,
				typeLabel = "testType",
				durationNegative = 0.20000000000000001f,
				gVelAmplitudeNegative = 0.29999999999999999f
			};

			actual.Should().BeEquivalentTo(serialized);
		}

		[Test]
		public void SerializeMovementWithElmos() {
			string s = @"{
			""gVelAmplitudePositive"" : 0.29999999999999999,
			""start"" : ""2020-09-08T00:11:38.513"",
			""amplitude"" : 0.10000000000000001,
			""durationPositive"" : 0.20000000000000001,
			""elmos"" : [
			{
				""typeID"" : 1,
				""end"" : ""2020-09-08T00:11:39.513"",
				""rejected"" : false,
				""typeLabel"" : ""test"",
				""start"" : ""2020-09-08T00:11:38.513"",
				""deviceIdent"" : ""3DF671DD-47D6-4F21-ADE2-6C2AB52A9875""
			},
			{
				""typeID"" : 1,
				""typeLabel"" : ""test"",
				""rejected"" : true,
				""start"" : ""2020-09-08T00:11:38.514"",
				""end"" : ""2020-09-08T00:11:39.514"",
				""deviceIdent"" : ""3DF671DD-47D6-4F21-ADE2-6C2AB52A9875""
			}
			],
			""typeID"" : 1,
			""typeLabel"" : ""testType"",
			""end"" : ""2020-09-08T00:11:39.513"",
			""durationNegative"" : 0.20000000000000001,
			""gVelAmplitudeNegative"" : 0.29999999999999999
		}";

			Movement serialized = JsonUtility.FromJson<Movement>(s);

			Movement actual = new Movement {
				gVelAmplitudePositive = 0.29999999999999999f,
				amplitude = 0.10000000000000001f,
				durationPositive = 0.20000000000000001f,
				typeID = (MovementEnum) 1,
				typeLabel = "testType",
				durationNegative = 0.20000000000000001f,
				gVelAmplitudeNegative = 0.29999999999999999f
			};


			actual.elmos.Add(new ElementalMovement {
				typeID = (ElmoEnum) 1,
				rejected = false,
				typeLabel = "test",
				deviceIdent = "3DF671DD-47D6-4F21-ADE2-6C2AB52A9875"
			});

			actual.elmos.Add(new ElementalMovement {
				typeID = (ElmoEnum) 1,
				rejected = true,
				typeLabel = "test",
				deviceIdent = "3DF671DD-47D6-4F21-ADE2-6C2AB52A9875"
			});


			actual.Should().BeEquivalentTo(serialized);
		}

		[Test]
		public void DeserializationContainsElmos() {
			Movement actual = new Movement {
				gVelAmplitudePositive = 0.29999999999999999f,
				amplitude = 0.10000000000000001f,
				durationPositive = 0.20000000000000001f,
				typeID = (MovementEnum) 1,
				typeLabel = "testType",
				durationNegative = 0.20000000000000001f,
				gVelAmplitudeNegative = 0.29999999999999999f
			};


			actual.elmos.Add(new ElementalMovement {
				typeID = (ElmoEnum) 1,
				rejected = false,
				typeLabel = "test",
				deviceIdent = "3DF671DD-47D6-4F21-ADE2-6C2AB52A9875"
			});

			actual.elmos.Add(new ElementalMovement {
				typeID = (ElmoEnum) 1,
				rejected = true,
				typeLabel = "test",
				deviceIdent = "3DF671DD-47D6-4F21-ADE2-6C2AB52A9875"
			});

			string s = JsonUtility.ToJson(actual);

			Movement m = JsonUtility.FromJson<Movement>(s);
			
			Assert.IsTrue(m.elmos.Count == 2);
			actual.Should().BeEquivalentTo(m);


		}
	}
}