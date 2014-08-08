using NUnit.Framework;
using UnityEngine;

namespace Tests
{
	[TestFixture]
	public class TestGrid
	{
        private Grid grid;

		[Test]
		public void PixelsAtCellZeroAreZeroVector()		
		{
			grid = new Grid(8, 8);
			Assert.That(grid.PixelAtCell(0, 0), Is.EqualTo(Vector2.zero));
		}
		
		[Test]
		public void PixelsAtCellZeroAreEqualToOffsetWhenOffsetIsNotZero()		
		{
			grid = new Grid(8, 8) {
				HorizontalOffset = 50f,
				VerticalOffset = 32f
			};
			Assert.That(grid.PixelAtCell(0, 0), Is.EqualTo(new Vector2(grid.HorizontalOffset, grid.VerticalOffset)));
		}

		[Test]
		public void PixelOffsetsAreZeroWhenNotSet()		
		{
			int gridWidth = 8;
			int gridHeight = 5;
            int zeroHorizontalOffset = 0, zeroVerticalOffset = 0;
			grid = new Grid(gridWidth, gridHeight);
			
			int i = 3;
			int j = 4;
			float width = zeroHorizontalOffset + gridWidth * i;
			float height = zeroVerticalOffset + gridHeight * j;
			Assert.That(grid.PixelAtCell(i, j), Is.EqualTo(new Vector2(width, height)));
		}

		[Test]
		public void PixelWidthAndHeightEqualToOffsetPlusCellSize()		
		{
			int gridWidth = 8;
			int gridHeight = 5;
			grid = new Grid(gridWidth, gridHeight) {
				HorizontalOffset = 50f,
				VerticalOffset = 32f
			};
			
			int i = 3;
			int j = 4;
			float width = grid.HorizontalOffset + gridWidth * i;
			float height = grid.VerticalOffset + gridHeight * j;
			Assert.That(grid.PixelAtCell(i, j), Is.EqualTo(new Vector2(width, height)));
		}
		
	}
}

