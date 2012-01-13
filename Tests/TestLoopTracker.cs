using System;
using Rhino.Mocks;
using NUnit.Framework;
using UnityEngine;

namespace Irrelevant
{

  [TestFixture]
  public class TestLoopTracker
  {
    private LoopTracker tracker;
    private Sounds sounds;
    private MockRepository mocks;

    private float beginning = 0f;

    [SetUp]
    public void SetUp()
    {
      mocks = new MockRepository();
      sounds = mocks.DynamicMock<Sounds>();
      tracker = new LoopTracker(sounds);
    }

    [Test]
    public void AudioMovesToBeginningWhenNoLoopPreviouslyPlayed() {
        float loopLength = 2f;
        using (mocks.Record()) {
          Expect.Call(delegate{sounds.setAudioTime(beginning);});
        }
        using (mocks.Playback()) {
          tracker.NextLoop(loopLength);
        }
    }

    [Test]
    public void AudioMovesToEndOfPreviousLoopWhenAdvancingToNextLoop () {
        float endTime = 2f;
        float anything = 8f;
        using (mocks.Record()) {
          Expect.Call(delegate{sounds.setAudioTime(beginning);});
          Expect.Call(delegate{sounds.setAudioTime(endTime);});
        }
        using (mocks.Playback()) {
          tracker.NextLoop(endTime);
          tracker.NextLoop(anything);
        }
    }
  }
}
