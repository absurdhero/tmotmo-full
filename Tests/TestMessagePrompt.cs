using System;
using System.Collections.Generic;
using System.Reflection;

using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using UnityEngine;

namespace Tests
{
	[TestFixture]
	public class PromptTest
	{
		private MockRepository mocks;
		private TouchSensor sensor;
		private Prompt prompt;
		private MessageBox messageBox;
		const float beginningOfTime = 10.0f;
		const float twoSecondsLater = beginningOfTime + 3f;
		
		[SetUp]
		public void SetUp ()
		{
			mocks = new MockRepository ();
			sensor = mocks.StrictMock<TouchSensor> ();
			prompt = mocks.DynamicMock<Prompt> ();
			messageBox = mocks.DynamicMock<MessageBox> ();
		}
		
		[Test]
		public void doesNotTriggerWhenNotTouched ()
		{
			var messagePromptCoordinator = new MessagePromptCoordinator (prompt, messageBox);

			messagePromptCoordinator.hintWhenTouched (
				GameObject => { Assert.Fail (); },
				sensor,
				beginningOfTime,
				new Dictionary<GameObject, ActionResponsePair[]> {}
			);
			messagePromptCoordinator.Update (beginningOfTime);
		}

		[Test]
		public void triggerWhenTouchedAndTimeElapsed ()
		{
			var messagePromptCoordinator = new MessagePromptCoordinator (prompt, messageBox);

			var singleActionResponse = new Dictionary<GameObject, ActionResponsePair[]> {
				{mocks.DynamicMock<GameObject>(), new [] {new ActionResponsePair("action",   new[] {"response"})}}
			};

			using (mocks.Ordered()) {
				//Expect.Call(sensor.hasTaps()).Return(true);
				Expect.Call (sensor.insideSprite (null, null, new[] {TouchPhase.Began})).Return (true);
				Expect.Call (sensor.insideSprite (null, null, new[] {TouchPhase.Began})).Return (false);
			}
	
			mocks.ReplayAll ();

			bool hintTriggered = false;
				
			messagePromptCoordinator.hintWhenTouched (
				GameObject => { hintTriggered = true; }, 
				sensor, 
				beginningOfTime,
				singleActionResponse
			);

			messagePromptCoordinator.Update (beginningOfTime);

			Assert.That (hintTriggered, Is.False);

			messagePromptCoordinator.hintWhenTouched (
				GameObject => { hintTriggered = true; },
				sensor,
				twoSecondsLater,
				singleActionResponse
			);

			messagePromptCoordinator.Update (twoSecondsLater);

			Assert.That (hintTriggered, Is.True);

			mocks.VerifyAll ();
		}
	}
}

