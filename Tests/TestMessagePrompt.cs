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
		const float epsilon = 0.000001f;
		const float beforeEverything = 10.0f;
		const float beforeMessageTime = beforeEverything + MessagePromptCoordinator.promptTime - epsilon;
		const float duringMessageTime = beforeEverything + MessagePromptCoordinator.promptTime + epsilon;
		const float afterMessageTime = beforeEverything + MessagePromptCoordinator.promptTime + 0.5f + epsilon;

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
				beforeEverything,
				new Dictionary<GameObject, ActionResponsePair[]> {}
			);
			messagePromptCoordinator.Update (beforeEverything);
		}

		[Test]
		public void triggerWhenTouchedAndTimeElapsed ()
		{
			var messagePromptCoordinator = new MessagePromptCoordinator (prompt, messageBox);

			var singleActionResponse = new Dictionary<GameObject, ActionResponsePair[]> {
				{mocks.DynamicMock<GameObject>(), new [] {new ActionResponsePair("action",   new[] {"response"})}}
			};

			using (mocks.Ordered()) {
				Expect.Call (sensor.insideSprite (null, null, new[] {TouchPhase.Began})).Return (true);
				Expect.Call (sensor.insideSprite (null, null, new[] {TouchPhase.Began})).Return (false);
			}
	
			mocks.ReplayAll ();

			bool hintTriggered = false;
				
			messagePromptCoordinator.hintWhenTouched (
				GameObject => { hintTriggered = true; }, 
				sensor, 
				beforeEverything,
				singleActionResponse
			);

			messagePromptCoordinator.Update (beforeEverything);

			Assert.That (hintTriggered, Is.False);

			messagePromptCoordinator.hintWhenTouched (
				GameObject => { hintTriggered = true; },
				sensor,
				afterMessageTime,
				singleActionResponse
			);

			messagePromptCoordinator.Update (afterMessageTime);

			Assert.That (hintTriggered, Is.True);

			mocks.VerifyAll ();
		}

		[Test]
		public void triggersFrontMostObjectWhenObjectsOverlap()
		{
			var messagePromptCoordinator = new MessagePromptCoordinator (prompt, messageBox);

			var back = new Vector3(0, 0, 0);
			var front = new Vector3(0, 0, -1);

			var first = new GameObject("first");
			var second = new GameObject("second");

			first.transform = new Transform();
			first.transform.position = back;
			second.transform = new Transform();
			second.transform.position = front;

			var singleActionResponse = new Dictionary<GameObject, ActionResponsePair[]> {
				{first, new [] {new ActionResponsePair("action",   new[] {"response"})}},
				{second, new [] {new ActionResponsePair("action2",   new[] {"response2"})}}
			};


			using (mocks.Ordered()) {
				Expect.Call (sensor.insideSprite (null, null, new[] {TouchPhase.Began})).Return (true);
				Expect.Call (sensor.insideSprite (null, null, new[] {TouchPhase.Began})).Return (false);

				Expect.Call (sensor.insideSprite (null, null, new[] {TouchPhase.Began})).Return (true);
				Expect.Call (sensor.insideSprite (null, null, new[] {TouchPhase.Began})).Return (true);
			}
			
			mocks.ReplayAll ();
			
			
			messagePromptCoordinator.hintWhenTouched (
				GameObject => { }, 
				sensor, 
				beforeEverything,
				singleActionResponse
			);

			messagePromptCoordinator.Update (beforeEverything);

			GameObject triggeredObject = null;

			messagePromptCoordinator.hintWhenTouched (
				gameObject => { triggeredObject = gameObject; }, 
			sensor,
			afterMessageTime,
			singleActionResponse
			);
			
			messagePromptCoordinator.Update (afterMessageTime + MessagePromptCoordinator.promptTime + 0.1f);
			
			Assert.That (triggeredObject, Is.EqualTo(second));
			
			mocks.VerifyAll ();
		}
		


		[Test]
		public void failToTriggerWhenTouchedOnSecondHintCall ()
		{
			var messagePromptCoordinator = new MessagePromptCoordinator (prompt, messageBox);
			
			var singleActionResponse = new Dictionary<GameObject, ActionResponsePair[]> {
				{mocks.DynamicMock<GameObject>(), new [] {new ActionResponsePair("action",   new[] {"response"})}}
			};
			
			using (mocks.Ordered()) {
				Expect.Call (sensor.insideSprite (null, null, new[] {TouchPhase.Began})).Return (true);
				Expect.Call(sensor.hasTaps()).Return(true);
				Expect.Call(sensor.hasTaps()).Return(true);
				Expect.Call (sensor.insideSprite (null, null, new[] {TouchPhase.Began})).Return (false);
				Expect.Call (sensor.insideSprite (null, null, new[] {TouchPhase.Began})).Return (false);
			}
			
			mocks.ReplayAll ();
			
			bool hintTriggered = false;
			bool secondHintTriggered = false;

			Action<GameObject> setHintTriggered =  GameObject => { hintTriggered = true; };
			Action<GameObject> setSecondHintTriggered =  GameObject => { secondHintTriggered = true; };

			messagePromptCoordinator.hintWhenTouched (
				setHintTriggered,
				sensor,
				beforeEverything,
				singleActionResponse
			);
			
			messagePromptCoordinator.Update (beforeEverything);


			messagePromptCoordinator.hintWhenTouched (
				setHintTriggered,
				sensor,
				beforeMessageTime,
				singleActionResponse
			);

			messagePromptCoordinator.hintWhenTouched (
				setSecondHintTriggered,
				sensor,
				beforeMessageTime,
				singleActionResponse
				);

			messagePromptCoordinator.Update(beforeMessageTime);

			messagePromptCoordinator.hintWhenTouched (
				setHintTriggered,
				sensor,
				afterMessageTime,
				singleActionResponse
				);

			messagePromptCoordinator.hintWhenTouched (
				setSecondHintTriggered,
				sensor,
				afterMessageTime,
				singleActionResponse
				);
			
			messagePromptCoordinator.Update(afterMessageTime);

			Assert.That (hintTriggered, Is.True);

			// second hint does not trigger because calling hintWhenTouched twice in one update fails
			Assert.That (secondHintTriggered, Is.False);

			mocks.VerifyAll ();
		}

		[Test]
		public void triggerFirstCallbackWhenTouchedOnFirstHintCall ()
		{
			var messagePromptCoordinator = new MessagePromptCoordinator (prompt, messageBox);
			
			var singleActionResponse = new Dictionary<GameObject, ActionResponsePair[]> {
				{mocks.DynamicMock<GameObject>(), new [] {new ActionResponsePair("action",   new[] {"response"})}}
			};
			
			using (mocks.Ordered()) {
				// touch on first tick
				Expect.Call (sensor.insideSprite (null, null, new[] {TouchPhase.Began})).Return (true);
				Expect.Call(sensor.hasTaps()).Return(true);
				// no touch on second tick
				Expect.Call (sensor.insideSprite (null, null, new[] {TouchPhase.Began})).Return (false);
			}
			
			mocks.ReplayAll ();
			
			bool hintTriggered = false;
			bool secondHintTriggered = false;
			
			Action<GameObject> setHintTriggered =  GameObject => { hintTriggered = true; };
			Action<GameObject> setSecondHintTriggered =  GameObject => { secondHintTriggered = true; };
			
			messagePromptCoordinator.hintWhenTouched (
				setHintTriggered,
				sensor,
				beforeEverything,
				singleActionResponse
				);
			
			messagePromptCoordinator.Update (beforeEverything);
			
			messagePromptCoordinator.hintWhenTouched (
				setSecondHintTriggered,
				sensor,
				beforeMessageTime,
				singleActionResponse
				);
			
			messagePromptCoordinator.Update(beforeMessageTime);

			messagePromptCoordinator.hintWhenTouched (
				setSecondHintTriggered,
				sensor,
				afterMessageTime,
				singleActionResponse
				);
			
			messagePromptCoordinator.Update(afterMessageTime);
			
			Assert.That (hintTriggered, Is.True);
			Assert.That (secondHintTriggered, Is.False);
			
			mocks.VerifyAll ();
		}

		[Test]
		public void failToTriggerAnyCallbackWhenTouchedOnSecondHintCall ()
		{
			var messagePromptCoordinator = new MessagePromptCoordinator (prompt, messageBox);
			
			var singleActionResponse = new Dictionary<GameObject, ActionResponsePair[]> {
				{mocks.DynamicMock<GameObject>(), new [] {new ActionResponsePair("action",   new[] {"response"})}}
			};
			
			using (mocks.Ordered()) {
				Expect.Call (sensor.insideSprite (null, null, new[] {TouchPhase.Began})).Return (false);
				// touch on second tick
				Expect.Call (sensor.insideSprite (null, null, new[] {TouchPhase.Began})).Return (true);
				Expect.Call(sensor.hasTaps()).Return(true);
			}
			
			mocks.ReplayAll ();
			
			bool hintTriggered = false;
			bool secondHintTriggered = false;
			
			Action<GameObject> setHintTriggered =  GameObject => { hintTriggered = true; };
			Action<GameObject> setSecondHintTriggered =  GameObject => { secondHintTriggered = true; };
			
			messagePromptCoordinator.hintWhenTouched (
				setHintTriggered,
				sensor,
				beforeEverything,
				singleActionResponse
				);
			
			messagePromptCoordinator.Update (beforeEverything);
			
			messagePromptCoordinator.hintWhenTouched (
				setSecondHintTriggered,
				sensor,
				beforeMessageTime,
				singleActionResponse
				);
			
			messagePromptCoordinator.Update(beforeMessageTime);
			
			messagePromptCoordinator.hintWhenTouched (
				setSecondHintTriggered,
				sensor,
				afterMessageTime,
				singleActionResponse
				);
			
			messagePromptCoordinator.Update(afterMessageTime);
			
			Assert.That (hintTriggered, Is.False);
			Assert.That (secondHintTriggered, Is.False);
			
			mocks.VerifyAll ();
		}
	}
}

