using NUnit.Framework;
using NMock;

namespace Irrelevant
{
	[TestFixture]
	public class TestLoopTracker {
		private LoopTracker tracker;
		private Mock<MockSounds> sounds;
		private MockFactory factory;
		private float beginning = 0f;

		public class MockSounds : Sounds {
			public MockSounds() : base(null) {}
		}
		
		[SetUp]
		public void SetUp() {
			factory = new MockFactory();
			sounds = factory.CreateMock<MockSounds>();
			tracker = new LoopTracker(sounds.MockObject);
		}

		[Test]
		public void AudioMovesToBeginningWhenNoLoopPreviouslyPlayed() {
			float loopLength = 2f;
			
			sounds.Expects.One.Method(_ => _.pickStemsFor(0)).WithAnyArguments();
			sounds.Expects.One.MethodWith(_ => _.setAudioTime(beginning));
			tracker.NextLoop(loopLength);
		}

		[Test]
		public void AudioMovesToEndOfPreviousLoopWhenAdvancingToNextLoop() {
			float endTime = 2f;
			float anything = 8f;
			
			sounds.Expects.AtLeastOne.Method(_ => _.pickStemsFor(0)).WithAnyArguments();
			sounds.Expects.One.MethodWith(_ => _.setAudioTime(beginning));
			sounds.Expects.One.MethodWith(_ => _.setAudioTime(endTime));

			tracker.NextLoop(endTime);
			tracker.NextLoop(anything);
		}
	}
}
