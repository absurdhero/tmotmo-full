using System;
using NUnit.Framework;
using NMock;

namespace Irrelevant
{
	[TestFixture]
	public class TestCycler {
		private MockFactory factory;
		private Mock<Sprite> sprite;
		private float beginning = 0.0f;
		private float frameTime = 1.0f;
	        private Cycler cycler;

		[SetUp]
		public void SetUp() {
			factory = new MockFactory();
			sprite = factory.CreateMock<Sprite>();
		}

		[Test]
		public void SwitchFrameEachTime() {
			cycler = new Cycler(frameTime, 3, beginning);

			sprite.Expects.Exactly(2).Method(_ => _.DrawNextFrame());

			cycler.AddSprite(sprite.MockObject);
			cycler.Update(beginning + frameTime);
			cycler.Update(beginning + frameTime * 2);
		}

		[Test]
		public void DontSwitchFramesWhenFrameIntervalHasntPassed() {
			cycler = new Cycler(frameTime, 3, beginning);
			var inbetweenFrame = frameTime / 2.0f;

			sprite.Expects.One.Method(_ => _.DrawNextFrame());

			cycler.AddSprite(sprite.MockObject);
			cycler.Update(beginning + inbetweenFrame);
			cycler.Update(beginning + frameTime);
			cycler.Update(beginning + frameTime + inbetweenFrame);
		}

		[Test]
		public void CyclerStopsWhenTotalCyclesReached() {
			var totalCycles = 3;
			var attemptedCycles = 5;
			cycler = new Cycler(frameTime, totalCycles, beginning);

			sprite.Expects.Exactly(totalCycles).Method(_ => _.DrawNextFrame());

			cycler.AddSprite(sprite.MockObject);

			for (var cycle = 1; cycle < attemptedCycles; cycle++) {
				cycler.Update(beginning + frameTime * cycle);
			}
		}

		[Test]
		public void CyclerAnimatesIndefinitelyWhenTotalCyclesIsZero() {
			var totalCycles = 0;
			var attemptedCycles = 5;
			cycler = new Cycler(frameTime, totalCycles, beginning);

			sprite.Expects.Exactly(attemptedCycles).Method(_ => _.DrawNextFrame());

			cycler.AddSprite(sprite.MockObject);
			for (var cycle = 1; cycle < attemptedCycles; cycle++) {
				cycler.Update(beginning + frameTime * cycle);
			}
		}
	}
}

