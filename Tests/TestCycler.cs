using System;
using NUnit.Framework;
using Rhino.Mocks;
using UnityEngine;

namespace Irrelevant
{

  [TestFixture]
  public class TestCycler
  {
    private MockRepository mocks;
    private Sprite sprite;

    private float beginning = 0.0f;
    private float frameTime = 1.0f;

    [SetUp]
    public void SetUp()
    {
      mocks = new MockRepository();
      sprite = mocks.DynamicMock<Sprite>();
    }

    [Test]
    public void SwitchFrameEachTime() {
      var cycler = new Cycler(frameTime, 3, beginning);
      using (mocks.Record()) {
        Expect.Call(delegate{sprite.NextTexture();}).Repeat.Twice();
      }
      using (mocks.Playback()) {
        cycler.AddSprite(sprite);
        cycler.Update(beginning + frameTime);
        cycler.Update(beginning + frameTime * 2);
      }
    }

    [Test]
    public void DontSwitchFramesWhenFrameIntervalHasntPassed() {
      var cycler = new Cycler(frameTime, 3, beginning);
      using (mocks.Record()) {
        Expect.Call(delegate{sprite.NextTexture();}).Repeat.Once();
      }
      using (mocks.Playback()) {
        cycler.AddSprite(sprite);
        cycler.Update(beginning + frameTime / 2.0f);
        cycler.Update(beginning + frameTime);
        cycler.Update(beginning + frameTime + (frameTime / 2.0f));
      }
    }

    [Test]
    public void CyclerStopsWhenTotalCyclesReached() {
      int totalCycles = 3;
      var cycler = new Cycler(frameTime, totalCycles, beginning);
      using (mocks.Record()) {
        Expect.Call(delegate{sprite.NextTexture();}).Repeat.Times(totalCycles);
      }
      using (mocks.Playback()) {
        cycler.AddSprite(sprite);
        cycler.Update(beginning + frameTime);
        cycler.Update(beginning + frameTime * 2);
        cycler.Update(beginning + frameTime * 3);
        cycler.Update(beginning + frameTime * 4);
        cycler.Update(beginning + frameTime * 5);
      }
    }
    [Test]
    public void CyclerAnimatesIndefinitelyWhenTotalCyclesIsZero() {
      int totalCycles = 0;
      var cycler = new Cycler(frameTime, totalCycles, beginning);
      using (mocks.Record()) {
        Expect.Call(delegate{sprite.NextTexture();}).Repeat.Times(3);
      }
      using (mocks.Playback()) {
        cycler.AddSprite(sprite);
        cycler.Update(beginning + frameTime);
        cycler.Update(beginning + frameTime * 2);
        cycler.Update(beginning + frameTime * 3);
      }
    }
  }
}

