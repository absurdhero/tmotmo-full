using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;

namespace Tests
{
  [TestFixture]
  public class MusicPlayerTest
  {
    [SetUp]
    public void SFJKHSDFJKH()
    {
    }
    
    [TearDown]
    public void SDFJHDSF() 
    {
    }
    
    
    [Test]
    public void whenNoTracksPlaying() {
      Assert.That(0, Is.Not.EqualTo(1));
      Assert.That("bob", Text.Contains("ob"));
    }
  }
}

