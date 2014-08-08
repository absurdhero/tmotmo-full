using NUnit.Framework;
using UnityEngine;
using NMock;
using Is = NMock.Is;

namespace Irrelevant
{
	[TestFixture]
	public class TestSpriteCollection {
		private MockFactory factory;
		private Mock<Sprite> sprite1, sprite2;
		private Mock<Camera> camera;
		private Mock<AbstractTouchSensor> sensor;
		private Sprite[] sprites;

		[SetUp]
		public void SetUp() {
			factory = new MockFactory();
			camera = factory.CreateMock<Camera>();
			sensor = factory.CreateMock<AbstractTouchSensor>();

			sprite1 = factory.CreateMock<Sprite>();
			sprite2 = factory.CreateMock<Sprite>();
			sprites = new[] { sprite1.MockObject, sprite2.MockObject };
		}

		[Test]
		public void touchedWhenAllSpritesTouched() {
			var collection = new SpriteCollection(sprites, camera.MockObject, sensor.MockObject);

			sensor.Expects.AtLeastOne.Method(_ => _.insideSprite(null, null, null)).WithAnyArguments().WillReturn(true);

			Assert.That(collection.touchedAtSameTime(0f), Iz.True);
		}

		[Test]
		public void touchedWhenOneSpriteTouched() {
			var collection = new SpriteCollection(sprites, camera.MockObject, sensor.MockObject);

			sensor.Expects.AtLeastOne.Method(_ => _.insideSprite(null, null, null))
				.With(Is.Anything, Is.EqualTo(sprite1.MockObject), Is.Anything).WillReturn(true);
			sensor.Expects.One.Method(_ => _.insideSprite(null, null, null))
				.With(Is.Anything, Is.EqualTo(sprite2.MockObject), Is.Anything).WillReturn(false);

			Assert.That(collection.touchedAtSameTime(0f), Iz.False);
		}

		[Test]
		public void touchedWhenOneThenBoth() {
			var collection = new SpriteCollection(sprites, null, sensor.MockObject);

			using (factory.Ordered()) {
				var heldDown = new[] {TouchPhase.Began, TouchPhase.Moved, TouchPhase.Stationary};
				var initialTouch = new[] {TouchPhase.Began};

				// first finger down
				sensor.Expects.One.Method(_ => _.insideSprite(null, null, null))
					.With(Is.Anything, Is.EqualTo(sprite1.MockObject), Is.EqualTo(heldDown))
					.WillReturn(true);
				sensor.Expects.One.Method(_ => _.insideSprite(null, null, null))
					.With(Is.Anything, Is.EqualTo(sprite1.MockObject), Is.EqualTo(initialTouch))
					.WillReturn(true);
				sensor.Expects.One.Method(_ => _.insideSprite(null, null, null))
					.With(Is.Anything, Is.EqualTo(sprite2.MockObject), Is.EqualTo(heldDown))
					.WillReturn(false);

				// second finger down
				sensor.Expects.One.Method(_ => _.insideSprite(null, null, null))
					.With(Is.Anything, Is.EqualTo(sprite1.MockObject), Is.EqualTo(heldDown))
					.WillReturn(true);
				sensor.Expects.One.Method(_ => _.insideSprite(null, null, null))
					.With(Is.Anything, Is.EqualTo(sprite1.MockObject), Is.EqualTo(initialTouch))
					.WillReturn(false);
				sensor.Expects.One.Method(_ => _.insideSprite(null, null, null))
					.With(Is.Anything, Is.EqualTo(sprite2.MockObject), Is.EqualTo(heldDown))
					.WillReturn(true);
				sensor.Expects.One.Method(_ => _.insideSprite(null, null, null))
					.With(Is.Anything, Is.EqualTo(sprite2.MockObject), Is.EqualTo(initialTouch))
					.WillReturn(true);
			}
			
			collection.touchedAtSameTime(0f);
			Assert.That(collection.touchedAtSameTime(0.1f), Iz.True);
		}

		[Test]
		public void notTouchedWhenOneThenBothTooLate() {
			var collection = new SpriteCollection(sprites, null, sensor.MockObject);

			using (factory.Ordered()) {
				var heldDown = new[] {TouchPhase.Began, TouchPhase.Moved, TouchPhase.Stationary};
				var initialTouch = new[] {TouchPhase.Began};

				// first finger down
				sensor.Expects.One.Method(_ => _.insideSprite(null, null, null))
					.With(Is.Anything, Is.EqualTo(sprite1.MockObject), Is.EqualTo(heldDown))
					.WillReturn(true);
				sensor.Expects.One.Method(_ => _.insideSprite(null, null, null))
					.With(Is.Anything, Is.EqualTo(sprite1.MockObject), Is.EqualTo(initialTouch))
					.WillReturn(true);
				sensor.Expects.One.Method(_ => _.insideSprite(null, null, null))
					.With(Is.Anything, Is.EqualTo(sprite2.MockObject), Is.EqualTo(heldDown))
					.WillReturn(false);
				
				// second finger down
				sensor.Expects.One.Method(_ => _.insideSprite(null, null, null))
					.With(Is.Anything, Is.EqualTo(sprite1.MockObject), Is.EqualTo(heldDown))
					.WillReturn(true);
				sensor.Expects.One.Method(_ => _.insideSprite(null, null, null))
					.With(Is.Anything, Is.EqualTo(sprite1.MockObject), Is.EqualTo(initialTouch))
					.WillReturn(false);
				sensor.Expects.One.Method(_ => _.insideSprite(null, null, null))
					.With(Is.Anything, Is.EqualTo(sprite2.MockObject), Is.EqualTo(heldDown))
					.WillReturn(true);
				sensor.Expects.One.Method(_ => _.insideSprite(null, null, null))
					.With(Is.Anything, Is.EqualTo(sprite2.MockObject), Is.EqualTo(initialTouch))
					.WillReturn(true);
			}
			
			collection.touchedAtSameTime(0f);
			Assert.That(collection.touchedAtSameTime(0.3f), Iz.False);
		}
	}
}

