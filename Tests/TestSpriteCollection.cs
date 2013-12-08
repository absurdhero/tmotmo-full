using System;
using NUnit.Framework;
using Rhino.Mocks;
using UnityEngine;
using Rhino.Mocks.Constraints;
using System.Collections.Generic;
using System.Collections;

namespace Irrelevant
{
	[TestFixture]
	public class TestSpriteCollection {
		private MockRepository mocks;
		private Sprite sprite1, sprite2;
		private Camera camera;
		private TouchSensor sensor;
		private Sprite[] sprites;

		[SetUp]
		public void SetUp() {
			mocks = new MockRepository();
			camera = mocks.DynamicMock<Camera>();
			sensor = mocks.DynamicMock<TouchSensor>();

			sprite1 = mocks.DynamicMock<Sprite>();
			sprite2 = mocks.DynamicMock<Sprite>();
			sprites = new[] { sprite1, sprite2 };
		}

		[Test]
		public void touchedWhenAllSpritesTouched() {
			var collection = new SpriteCollection(sprites, camera, sensor);

			using (mocks.Record()) {
				Expect.Call (sensor.insideSprite(null, null, null)).IgnoreArguments().Return (true);
			}
			using (mocks.Playback()) {
				Assert.IsTrue(collection.touchedAtSameTime(0f));
			}
		}

		[Test]
		public void touchedWhenOneSpriteTouched() {
			var collection = new SpriteCollection(sprites, camera, sensor);

			using (mocks.Record()) {
				Expect.Call (sensor.insideSprite(null, null, null)).Constraints(Is.Anything(), Is.Same(sprite1), Is.Anything()).Return(true);
				Expect.Call (sensor.insideSprite(null, null, null)).Constraints(Is.Anything(), Is.Same(sprite2), Is.Anything()).Return(false);
			}
			using (mocks.Playback()) {
				Assert.IsFalse(collection.touchedAtSameTime(0f));
			}
		}

		[Test]
		public void touchedWhenOneThenBoth() {
			var collection = new SpriteCollection(sprites, null, sensor);

			using (mocks.Ordered()) {
				var heldDown = new[] {TouchPhase.Began, TouchPhase.Moved, TouchPhase.Stationary};
				var initialTouch = new[] {TouchPhase.Began};

				// first finger down
//				Expect.Call (sensor.insideSprite(null, null, null))
//					.Callback(delegate (Camera camera, Sprite sprite, ICollection<TouchPhase> phases) { Console.WriteLine("called with " + sprite + ", " + phases); return Is.Same(sprite1).Eval(sprite) && Is.Equal(heldDown).Eval(phases); }).Return(true);
				Expect.Call (sensor.insideSprite(null, null, null)).Constraints(Is.Anything(), Is.Same(sprite1), Is.Equal(heldDown)).Return(true);
				Expect.Call (sensor.insideSprite(null, null, null)).Constraints(Is.Anything(), Is.Same(sprite1), Is.Equal(initialTouch)).Return(true);
				Expect.Call (sensor.insideSprite(null, null, null)).Constraints(Is.Anything(), Is.Same(sprite2), Is.Equal(heldDown)).Return(false);

				// second finger down
				Expect.Call (sensor.insideSprite(null, null, null)).Constraints(Is.Anything(), Is.Same(sprite1), Is.Equal(heldDown)).Return(true);
				Expect.Call (sensor.insideSprite(null, null, null)).Constraints(Is.Anything(), Is.Same(sprite1), Is.Equal(initialTouch)).Return(false);
				Expect.Call (sensor.insideSprite(null, null, null)).Constraints(Is.Anything(), Is.Same(sprite2), Is.Equal(heldDown)).Return(true);
				Expect.Call (sensor.insideSprite(null, null, null)).Constraints(Is.Anything(), Is.Same(sprite2), Is.Equal(initialTouch)).Return(true);
			}
			
			mocks.ReplayAll ();

			collection.touchedAtSameTime(0f);
			Assert.IsTrue(collection.touchedAtSameTime(0.1f));
		}

		[Test]
		public void notTouchedWhenOneThenBothTooLate() {
			var collection = new SpriteCollection(sprites, null, sensor);

			using (mocks.Ordered()) {
				var heldDown = new[] {TouchPhase.Began, TouchPhase.Moved, TouchPhase.Stationary};
				var initialTouch = new[] {TouchPhase.Began};

				// first finger down
				Expect.Call (sensor.insideSprite(null, null, null)).Constraints(Is.Anything(), Is.Same(sprite1), Is.Equal(heldDown)).Return(true);
				Expect.Call (sensor.insideSprite(null, null, null)).Constraints(Is.Anything(), Is.Same(sprite1), Is.Equal(initialTouch)).Return(true);
				Expect.Call (sensor.insideSprite(null, null, null)).Constraints(Is.Anything(), Is.Same(sprite2), Is.Equal(heldDown)).Return(false);

				// second finger down
				Expect.Call (sensor.insideSprite(null, null, null)).Constraints(Is.Anything(), Is.Same(sprite1), Is.Equal(heldDown)).Return(true);
				Expect.Call (sensor.insideSprite(null, null, null)).Constraints(Is.Anything(), Is.Same(sprite1), Is.Equal(initialTouch)).Return(false);
				Expect.Call (sensor.insideSprite(null, null, null)).Constraints(Is.Anything(), Is.Same(sprite2), Is.Equal(heldDown)).Return(true);
				Expect.Call (sensor.insideSprite(null, null, null)).Constraints(Is.Anything(), Is.Same(sprite2), Is.Equal(initialTouch)).Return(true);
			}
			
			mocks.ReplayAll ();

			collection.touchedAtSameTime(0f);
			Assert.IsFalse(collection.touchedAtSameTime(0.3f));
		}
	}
}

