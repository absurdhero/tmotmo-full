using System.Collections.Generic;

using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using NMock;
using UnityEngine;

namespace Tests
{
	[TestFixture]
	public class TestTouchSensor
	{
		const float epsilon = 0.000001f;

		private MockFactory factory;
		Mock<Camera> camera;
		Mock<AbstractInput> inputReturningTouch;

		[SetUp]
		public void SetUp ()
		{
			factory = new MockFactory();
			camera = factory.CreateMock<Camera>();
			inputReturningTouch = inputReturningMockTouch();
		}
		
		[Test]
		public void touchNotInsideSpriteWhenAnotherSpriteInFront()
		{
			var sprite = spriteAt(Vector3.forward);
			expectAndReturnAlpha(sprite, 1.0f);

			var frontSprite = spriteAt(Vector3.back);
			expectAndReturnAlpha(frontSprite, 1.0f);

			var sensor = new TouchSensor(
				inputReturningTouch.MockObject,
				findInScene(new[] { sprite.MockObject, frontSprite.MockObject}));
			
			Assert.That(sensor.insideSprite(camera.MockObject, sprite.MockObject), Iz.False);
		}
		
		[Test]
		public void touchInsideSpriteWhenAnotherSpriteNotInFront() {
			var sprite = spriteAt(Vector3.zero);
			expectAndReturnAlpha(sprite, 1.0f);
			var backSprite = spriteAt(Vector3.forward);

			var sensor = new TouchSensor(
				inputReturningTouch.MockObject,
				findInScene(new[] { sprite.MockObject, backSprite.MockObject}));
			
			Assert.That(sensor.insideSprite(camera.MockObject, sprite.MockObject), Iz.True);
		}
		
		[Test]
		public void touchInsideSpriteWhenTransparentSpriteInFront() {
			var sprite = spriteAt(Vector3.forward);
			expectAndReturnAlpha(sprite, 1.0f);

			var frontSprite = spriteAt(Vector3.back);
			expectAndReturnAlpha(frontSprite, 0.0f);

			var sensor = new TouchSensor(
				inputReturningTouch.MockObject,
				findInScene(new[] { sprite.MockObject, frontSprite.MockObject}));
			
			Assert.That(sensor.insideSprite(camera.MockObject, sprite.MockObject), Iz.True);
		}
		
		[Test]
		public void noTouchWhenAllSpritesAreTransparent() {
			var sprite = spriteAt(Vector3.forward);
			expectAndReturnAlpha(sprite, 0.0f);

			var frontSprite = spriteAt(Vector3.back);
			expectAndReturnAlpha(frontSprite, 0.0f);

			var sensor = new TouchSensor(
				inputReturningTouch.MockObject,
				findInScene(new[] { sprite.MockObject, frontSprite.MockObject}));
			
			Assert.That(sensor.insideSprite(camera.MockObject, sprite.MockObject), Iz.False);
		}
		
		[Test]
		public void touchWhenOnlyOneSpriteBeneathFinger() {
			var sprite = spriteAt(Vector3.forward);
			expectAndReturnAlpha(sprite, 1.0f);

			var sensor = new TouchSensor(
				inputReturningTouch.MockObject,
				findInScene(new[] { sprite.MockObject}));
			
			Assert.That(sensor.insideSprite(camera.MockObject, sprite.MockObject), Iz.True);
		}
		
		Mock<AbstractInput> inputReturningMockTouch() {
			var touch = touchExpectedAtOrigin();
			var mockInput = factory.CreateMock<AbstractInput>();
			mockInput.Expects.AtLeastOne.GetProperty(_ => _.touchCount).WillReturn(1);
			mockInput.Expects.AtLeastOne.MethodWith(_ => _.GetTouch(0)).WillReturn(touch.MockObject);
			return mockInput;
		}
		
		Mock<Touch> touchExpectedAtOrigin() {
			var touch = factory.CreateMock<Touch>();
			touch.Expects.AtLeastOne.GetProperty(_ => _.phase).WillReturn(TouchPhase.Began);
			touch.Expects.AtLeastOne.GetProperty(_ => _.position).WillReturn(Vector2.zero);
			return touch;
		}

		Mock<Sprite> spriteAt(Vector3 position) {
			var sprite = factory.CreateMock<Sprite>();
			sprite.Expects.AtLeastOne.Method(_ => _.Contains(camera.MockObject, Vector2.zero)).WithAnyArguments().WillReturn(true);
			sprite.Expects.AtLeastOne.GetProperty(_ => _.worldPosition).WillReturn(position);
			return sprite;
		}

		void expectAndReturnAlpha(Mock<Sprite> sprite, float alpha) {
			sprite.Expects.AtLeastOne.Method(_ => _.getAlphaAtScreenPosition(null)).WithAnyArguments().WillReturn(alpha);
		}

		GameObjectFinder findInScene(Sprite[] sprites) {
			var finder = factory.CreateMock<GameObjectFinder>();
			finder.Expects.One.MethodWith(_ => _.allSprites()).WillReturn(new List<Sprite>(sprites));
			return finder.MockObject;
		}
	}
}

