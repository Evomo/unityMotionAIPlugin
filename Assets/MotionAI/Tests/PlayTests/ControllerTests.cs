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
				MotionAiController c = new GameObject().AddComponent<MotionAiController>();
				// c.isGlobal = true;
			}

			manager.ControlPairing();

			amountOfControllers.Should().Be(manager.controllerManager.unpairedAvailableControllers.Count);
			yield return null;
		}


		[UnityTest]
		public IEnumerator AllGlobalControllersArePaired() {
			MotionAIManager manager = new GameObject().AddComponent<MotionAIManager>();

			int amountOfControllers = 3;
			for (int i = 0; i < amountOfControllers; i++) {
				MotionAiController c = new GameObject().AddComponent<MotionAiController>();
				c.isGlobal = true;
			}

			manager.ControlPairing();

			amountOfControllers.Should().Be(manager.controllerManager.AmountOfPairedControllers);
			yield return null;
		}


		[UnityTest]
		public IEnumerator ControllersWithDifferentDidGetPaired() {
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

			MotionAIManager manager = new GameObject().AddComponent<MotionAIManager>();
			List<string> dids = new List<string>();
			List<MotionAiController> controllers = new List<MotionAiController>();


			int amountOfControllers = 3;
			for (int i = 0; i < amountOfControllers; i++) {
				MotionAiController c = new GameObject().AddComponent<MotionAiController>();
				controllers.Add(c);
				dids.Add(new string(Enumerable.Repeat(chars, 10)
					.Select(s => s[Random.Range(0, s.Length)]).ToArray()));
			}

			manager.ControlPairing();


			dids.ForEach(did => manager.controllerManager.ManageMotion(new Movement {
				elmos = new List<ElementalMovement> {
					new ElementalMovement {
						deviceIdent = did
					}
				}
			}));

			for (int i = 0; i < amountOfControllers; i++) {
				string did = dids[i];
				MotionAiController c =  manager.controllerManager.controllers[did].First();
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
			List<MotionAiController> controllers = new List<MotionAiController>();


			int amountOfControllers = 3;
			for (int i = 0; i < amountOfControllers; i++) {
				MotionAiController c = new GameObject().AddComponent<MotionAiController>();
				controllers.Add(c);
				dids.Add(new string(Enumerable.Repeat(chars, 10)
					.Select(s => s[Random.Range(0, s.Length)]).ToArray()));
			}

			manager.ControlPairing();


			dids.ForEach(did => manager.controllerManager.ManageMotion(new Movement {
				elmos = new List<ElementalMovement> {
					new ElementalMovement {
						deviceIdent = did
					}
				}
			}));

			manager.controllerManager.UnpairControllers();
			manager.controllerManager.AmountOfPairedControllers.Should().Be(0);
			yield return null;
		}
	}
}