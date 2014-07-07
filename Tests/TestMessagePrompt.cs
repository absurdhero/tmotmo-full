using System;
using System.Collections.Generic;

using NUnit.Framework;
using NMock;
using UnityEngine;
using Is = NMock.Is;

namespace Tests
{
    [TestFixture]
    public class PromptTest {
        private MockFactory factory;
        private Mock<AbstractTouchSensor> sensor;
        private Mock<TextControl> prompt;
        private Mock<AbstractMessageBox> messageBox;
        private Mock<GameObject> gameObject;
        private Mock<Sprite> sprite;
        const float epsilon = 0.000001f;
        const float beforeEverything = 10.0f;
        const float beforeMessageTime = beforeEverything + MessagePromptCoordinator.promptTime - epsilon;
        const float duringMessageTime = beforeEverything + MessagePromptCoordinator.promptTime + epsilon;
        const float afterMessageTime = beforeEverything + MessagePromptCoordinator.promptTime + 0.5f + epsilon;

        [SetUp]
        public void SetUp() {

            factory = new MockFactory();
            sensor = factory.CreateMock<AbstractTouchSensor>();
            prompt = factory.CreateMock<TextControl>();
            prompt.Expects.One.Method(_ => _.show());
            prompt.Expects.One.Method(_ => _.hide());
            prompt.Expects.One.Method(_ => _.setText("")).With(Is.TypeOf(typeof(String)));

            sprite = factory.CreateMock<Sprite>();

            messageBox = factory.CreateMock<AbstractMessageBox>();
            messageBox.Expects.One.Method(_ => _.setMessage("")).With(Is.TypeOf(typeof(String)));
            messageBox.Expects.One.Method(_ => _.hide());
            messageBox.Expects.One.Method(_ => _.show());
            gameObject = factory.CreateMock<GameObject>();
            gameObject.Expects.Any.Method(_ => _.GetComponent<Sprite>()).WillReturn(sprite.MockObject);
        }

        [Test]
        public void doesNotTriggerWhenNotTouched() {
            var messagePromptCoordinator = new MessagePromptCoordinator(prompt.MockObject, messageBox.MockObject);

            messagePromptCoordinator.hintWhenTouched(
                GameObject => Assert.Fail(),
                sensor.MockObject,
                beforeEverything,
                new Dictionary<GameObject, ActionResponsePair[]> { }
            );
            messagePromptCoordinator.Update(beforeEverything);
        }

        [Test]
        public void triggerWhenTouchedAndTimeElapsed() {
            var messagePromptCoordinator = new MessagePromptCoordinator(prompt.MockObject, messageBox.MockObject);

            var singleActionResponse = new Dictionary<GameObject, ActionResponsePair[]> {
                { gameObject.MockObject, new [] { new ActionResponsePair("action", new[] { "response" }) } }
            };

            using (factory.Ordered()) {
                sensor.Expects.One.MethodWith(_ => _.insideSprite(null, sprite.MockObject, new[] { TouchPhase.Began })).WillReturn(true);
                sensor.Expects.One.MethodWith(_ => _.insideSprite(null, sprite.MockObject, new[] { TouchPhase.Began })).WillReturn(false);
            }
	
            bool hintTriggered = false;
				
            messagePromptCoordinator.hintWhenTouched(
                GameObject => {
                    hintTriggered = true;
                }, 
                sensor.MockObject, 
                beforeEverything,
                singleActionResponse
            );

            messagePromptCoordinator.Update(beforeEverything);

            Assert.That(hintTriggered, Iz.False);

            messagePromptCoordinator.hintWhenTouched(
                GameObject => {
                    hintTriggered = true;
                },
                sensor.MockObject,
                afterMessageTime,
                singleActionResponse
            );

            messagePromptCoordinator.Update(afterMessageTime);

            Assert.That(hintTriggered, Iz.True);
        }

        [Test]
        public void triggersFrontMostObjectWhenObjectsOverlap() {
            var messagePromptCoordinator = new MessagePromptCoordinator(prompt.MockObject, messageBox.MockObject);

            var first = factory.CreateMock<GameObject>();
            first.Expects.Any.Method(_ => _.GetComponent<Sprite>()).WillReturn(sprite.MockObject);
            var second = factory.CreateMock<GameObject>();
            second.Expects.Any.Method(_ => _.GetComponent<Sprite>()).WillReturn(sprite.MockObject);

            var back = new Vector3(0, 0, 0);
            var backTransform = new Transform() {
                position = back
            };
            var front = new Vector3(0, 0, -1);
            var frontTransform = new Transform() {
                position = front
            };

            // if you think this looks weird, it is. we can't mock bareback public fields, and Unity is full of them.
            first.MockObject.transform = backTransform;
            second.MockObject.transform = frontTransform;

            var frontObjectResponse = "front object response";
            var frontObjectAction = "action2";
            var actionResponses = new Dictionary<GameObject, ActionResponsePair[]> {
                { first.MockObject, new [] { new ActionResponsePair("action", new[] { "response" }) } },
                { second.MockObject, new [] { new ActionResponsePair(frontObjectAction, new[] { frontObjectResponse }) } }
            };

            using (factory.Ordered()) {
                sensor.Expects.One.MethodWith(_ => _.insideSprite(null, sprite.MockObject, new[] { TouchPhase.Began })).WillReturn(true);
                sensor.Expects.One.MethodWith(_ => _.insideSprite(null, sprite.MockObject, new[] { TouchPhase.Began })).WillReturn(false);
                sensor.Expects.One.MethodWith(_ => _.insideSprite(null, sprite.MockObject, new[] { TouchPhase.Began })).WillReturn(true);
            }
			
            messagePromptCoordinator.hintWhenTouched(
                GameObject => {
                }, 
                sensor.MockObject, 
                beforeEverything,
                actionResponses
            );

            messagePromptCoordinator.Update(beforeEverything);

            GameObject triggeredObject = null;

            messageBox.Expects.One.Method(_ => _.setMessage("")).With(Is.EqualTo(frontObjectResponse));
            messageBox.Expects.One.Method(_ => _.hide());
            messageBox.Expects.One.Method(_ => _.show());

            prompt.Expects.One.Method(_ => _.setText("")).With(Is.EqualTo(frontObjectAction));
            prompt.Expects.One.Method(_ => _.hide());
            prompt.Expects.One.Method(_ => _.show());

            messagePromptCoordinator.hintWhenTouched(
                gameObject => {
                    triggeredObject = gameObject;
                }, 
                sensor.MockObject,
                afterMessageTime,
                actionResponses
            );
                
            messageBox.Expects.One.Method(_ => _.hide());
            messagePromptCoordinator.Update(afterMessageTime + MessagePromptCoordinator.promptTime + 0.1f);

            Assert.That(triggeredObject, Iz.EqualTo(second.MockObject));
        }


        [Test]
        public void failToTriggerWhenTouchedOnSecondHintCall() {
            var messagePromptCoordinator = new MessagePromptCoordinator(prompt.MockObject, messageBox.MockObject);
			
            var singleActionResponse = new Dictionary<GameObject, ActionResponsePair[]> {
                { gameObject.MockObject, new [] { new ActionResponsePair("action", new[] { "response" }) } }
            };
	
            using (factory.Ordered()) {
                sensor.Expects.One.MethodWith(_ => _.insideSprite(null, sprite.MockObject, new[] { TouchPhase.Began })).WillReturn(true);
                sensor.Expects.One.MethodWith(_ => _.hasTaps()).WillReturn(true);
                sensor.Expects.One.MethodWith(_ => _.hasTaps()).WillReturn(true);
                sensor.Expects.One.MethodWith(_ => _.insideSprite(null, sprite.MockObject, new[] { TouchPhase.Began })).WillReturn(false);
                sensor.Expects.One.MethodWith(_ => _.insideSprite(null, sprite.MockObject, new[] { TouchPhase.Began })).WillReturn(false);
            }
			
			
            bool hintTriggered = false;
            bool secondHintTriggered = false;

            Action<GameObject> setHintTriggered = GameObject => {
                hintTriggered = true;
            };
            Action<GameObject> setSecondHintTriggered = GameObject => {
                secondHintTriggered = true;
            };

            messagePromptCoordinator.hintWhenTouched(
                setHintTriggered,
                sensor.MockObject,
                beforeEverything,
                singleActionResponse
            );
			
            messagePromptCoordinator.Update(beforeEverything);

            messagePromptCoordinator.hintWhenTouched(
                setHintTriggered,
                sensor.MockObject,
                beforeMessageTime,
                singleActionResponse
            );

            messagePromptCoordinator.hintWhenTouched(
                setSecondHintTriggered,
                sensor.MockObject,
                beforeMessageTime,
                singleActionResponse
            );

            messagePromptCoordinator.Update(beforeMessageTime);

            messagePromptCoordinator.hintWhenTouched(
                setHintTriggered,
                sensor.MockObject,
                afterMessageTime,
                singleActionResponse
            );

            messagePromptCoordinator.hintWhenTouched(
                setSecondHintTriggered,
                sensor.MockObject,
                afterMessageTime,
                singleActionResponse
            );
			
            messagePromptCoordinator.Update(afterMessageTime);

            Assert.That(hintTriggered, Iz.True);

            // second hint does not trigger because calling hintWhenTouched twice in one update fails
            Assert.That(secondHintTriggered, Iz.False);

        }

        [Test]
        public void triggerFirstCallbackWhenTouchedOnFirstHintCall() {
            var messagePromptCoordinator = new MessagePromptCoordinator(prompt.MockObject, messageBox.MockObject);
			
            var singleActionResponse = new Dictionary<GameObject, ActionResponsePair[]> {
                { gameObject.MockObject, new [] { new ActionResponsePair("action", new[] { "response" }) } }
            };
			
            using (factory.Ordered()) {
                // touch on first tick
                sensor.Expects.One.MethodWith(_ => _.insideSprite(null, sprite.MockObject, new[] { TouchPhase.Began })).WillReturn(true);
                sensor.Expects.One.MethodWith(_ => _.hasTaps()).WillReturn(true);
                // no touch on second tick
                sensor.Expects.One.MethodWith(_ => _.insideSprite(null, sprite.MockObject, new[] { TouchPhase.Began })).WillReturn(false);
            }
			
			
            bool hintTriggered = false;
            bool secondHintTriggered = false;
			
            Action<GameObject> setHintTriggered = GameObject => {
                hintTriggered = true;
            };
            Action<GameObject> setSecondHintTriggered = GameObject => {
                secondHintTriggered = true;
            };
			
            messagePromptCoordinator.hintWhenTouched(
                setHintTriggered,
                sensor.MockObject,
                beforeEverything,
                singleActionResponse
            );
			
            messagePromptCoordinator.Update(beforeEverything);
			
            messagePromptCoordinator.hintWhenTouched(
                setSecondHintTriggered,
                sensor.MockObject,
                beforeMessageTime,
                singleActionResponse
            );
			
            messagePromptCoordinator.Update(beforeMessageTime);

            messagePromptCoordinator.hintWhenTouched(
                setSecondHintTriggered,
                sensor.MockObject,
                afterMessageTime,
                singleActionResponse
            );
			
            messagePromptCoordinator.Update(afterMessageTime);
			
            Assert.That(hintTriggered, Iz.True);
            Assert.That(secondHintTriggered, Iz.False);
			
        }

        [Test]
        public void failToTriggerAnyCallbackWhenTouchedOnSecondHintCall() {
            var messagePromptCoordinator = new MessagePromptCoordinator(prompt.MockObject, messageBox.MockObject);
			
            var singleActionResponse = new Dictionary<GameObject, ActionResponsePair[]> {
                { gameObject.MockObject, new [] { new ActionResponsePair("action", new[] { "response" }) } }
            };
			
            using (factory.Ordered()) {
                sensor.Expects.One.MethodWith(_ => _.insideSprite(null, sprite.MockObject, new[] { TouchPhase.Began })).WillReturn(false);
                // touch on second tick
                sensor.Expects.One.MethodWith(_ => _.insideSprite(null, sprite.MockObject, new[] { TouchPhase.Began })).WillReturn(true);
                sensor.Expects.One.MethodWith(_ => _.hasTaps()).WillReturn(true);
            }
			
			
            bool hintTriggered = false;
            bool secondHintTriggered = false;
			
            Action<GameObject> setHintTriggered = GameObject => {
                hintTriggered = true;
            };
            Action<GameObject> setSecondHintTriggered = GameObject => {
                secondHintTriggered = true;
            };
			
            messagePromptCoordinator.hintWhenTouched(
                setHintTriggered,
                sensor.MockObject,
                beforeEverything,
                singleActionResponse
            );
			
            messagePromptCoordinator.Update(beforeEverything);
			
            messagePromptCoordinator.hintWhenTouched(
                setSecondHintTriggered,
                sensor.MockObject,
                beforeMessageTime,
                singleActionResponse
            );
			
            messagePromptCoordinator.Update(beforeMessageTime);
			
            messagePromptCoordinator.hintWhenTouched(
                setSecondHintTriggered,
                sensor.MockObject,
                afterMessageTime,
                singleActionResponse
            );
			
            messagePromptCoordinator.Update(afterMessageTime);
			
            Assert.That(hintTriggered, Iz.False);
            Assert.That(secondHintTriggered, Iz.False);
        }
    }
}

