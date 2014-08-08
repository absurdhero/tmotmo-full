using System;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	public class WrapTest
	{
		[Test]
		public void singleLetterIsUnchanged() {
			assertNoWrap("1");
		}

		[Test]
		public void letterWithSpacesIsUnchanged() {
			assertNoWrap(" 1 ");
		}

		[Test]
		public void stringShorterThanMaxCharsIsUnchanged() {
			assertNoWrap("abc def ghi jkl mno");
		}

		[Test]
		public void wrapOnLastSpaceWhenEndsWithLongWord() {
			Assert.That(MessageBox.wrap (" 1 3 5 7 911131517192123252729"), Is.EqualTo (" 1 3 5 7\n911131517192123252729"));
		}

		[Test]
		public void wrapOnSecondToLastSpaceWhenEndsWithShortWord() {
			Assert.That(MessageBox.wrap (" 1 3 5 7 91113151719212325 27"), Is.EqualTo (" 1 3 5 7\n91113151719212325 27"));
		}

		private void assertNoWrap(String str) {
			Assert.That(MessageBox.wrap(str), Is.EqualTo(str));
		}
	}
}

