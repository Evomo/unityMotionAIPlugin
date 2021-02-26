#if UNITY_EDITOR && !UNITY_IOS 
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using MotionAI.Core;
using MotionAI.Core.Controller;
using MotionAI.Core.POCO;
using UnityEngine;
using UnityEngine.TestTools;

namespace MotionAI.Tests.PlayTests {
	public class ControllerTests {
		[UnityTest]
		public IEnumerator MotionManagerFindsControllers() {
			MotionAIManager manager = new GameObject().AddComponent<MotionAIManager>();

			int amountOfControllers = 3;
			for (int i = 0; i < amountOfControllers; i++) {
				MotionAIController c = new GameObject().AddComponent<MotionAIController>();
				// c.isGlobal = true;
			}

			manager.StartControlPairing();

			amountOfControllers.Should().Be(manager.controllerManager.unpairedAvailableControllers.Count);
			yield return null;
		}


		[UnityTest]
		public IEnumerator AllGlobalControllersArePaired() {
			MotionAIManager manager = new GameObject().AddComponent<MotionAIManager>();

			int amountOfControllers = 3;
			for (int i = 0; i < amountOfControllers; i++) {
				MotionAIController c = new GameObject().AddComponent<MotionAIController>();
				c.IsGlobal = true;
			}

			manager.StartControlPairing();

			amountOfControllers.Should().Be(manager.controllerManager.PairedControllers.Count);
			yield return null;
		}


		[UnityTest]
		public IEnumerator ControllersWithDifferentDidGetPaired() {
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

			MotionAIManager manager = new GameObject().AddComponent<MotionAIManager>();
			List<string> dids = new List<string>();
			List<MotionAIController> controllers = new List<MotionAIController>();


			int amountOfControllers = 3;
			for (int i = 0; i < amountOfControllers; i++) {
				MotionAIController c = new GameObject().AddComponent<MotionAIController>();
				controllers.Add(c);
				dids.Add(new string(Enumerable.Repeat(chars, 10)
					.Select(s => s[Random.Range(0, s.Length)]).ToArray()));
			}

			manager.StartControlPairing();


			dids.ForEach(did => manager.controllerManager.ManageMotion(new EvoMovement {
				deviceID = did
			}));

			for (int i = 0; i < amountOfControllers; i++) {
				string did = dids[i];
				MotionAIController c = manager.controllerManager.controllers[did].First();
				c.DeviceId.Should().Be(did);
				c.IsPaired.Should().BeTrue();
			}

			yield return null;
		}


		[UnityTest]
		public IEnumerator ControllersGetUnpaired() {
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

			MotionAIManager manager = new GameObject().AddComponent<MotionAIManager>();
			List<string> dids = new List<string>();
			List<MotionAIController> controllers = new List<MotionAIController>();


			int amountOfControllers = 3;
			for (int i = 0; i < amountOfControllers; i++) {
				MotionAIController c = new GameObject().AddComponent<MotionAIController>();
				controllers.Add(c);
				dids.Add(new string(Enumerable.Repeat(chars, 10)
					.Select(s => s[Random.Range(0, s.Length)]).ToArray()));
			}

			manager.StartControlPairing();


			dids.ForEach(did => manager.controllerManager.ManageMotion(new EvoMovement {
				elmos = new List<ElementalMovement> {
					new ElementalMovement()
				}
			}));

			manager.controllerManager.UnpairControllers();
			manager.controllerManager.PairedControllers.Count.Should().Be(0);
			yield return null;
		}
	}
}
#endif