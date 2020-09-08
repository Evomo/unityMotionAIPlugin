using System;
using MotionAI.Core.POCO;
using UnityEngine.Events;

namespace MotionAI.Core.Controller {
	public class OnControllerPaired : UnityEvent<string> { }

	public class OnMotion  : UnityEvent<Movement>{}
}