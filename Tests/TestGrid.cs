using System;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using UnityEngine;

namespace Tests
{
	[TestFixture]
	public class TestGrid
	{
		[Test]
		public void PixelsAtCellZeroAreZeroVector()		
		{
			Grid grid = new Grid(8, 8);
			Assert.That(grid.PixelAtCell(0, 0), Is.EqualTo(Vector2.zero));
		}
		
		[Test]
		public void PixelsAtCellZeroAreEqualToOffsetWhenOffsetIsNotZero()		
		{
			Grid grid = new Grid(8, 8);
			grid.HorizontalOffset = 50f;
			grid.VerticalOffset = 32f;
			Assert.That(grid.PixelAtCell(0, 0), Is.EqualTo(new Vector2(grid.HorizontalOffset, grid.VerticalOffset)));
		}

		[Test]
		public void PixelOffsetsAreZeroWhenNotSet()		
		{
			int gridWidth = 8;
			int gridHeight = 5;
			Grid grid = new Grid(gridWidth, gridHeight);
			
			int i = 3;
			int j = 4;
			float width = gridWidth * i;
			float height = gridHeight * j;
			Assert.That(grid.PixelAtCell(i, j), Is.EqualTo(new Vector2(width, height)));
		}

		[Test]
		public void PixelWidthAndHeightEqualToOffsetPlusCellSize()		
		{
			int gridWidth = 8;
			int gridHeight = 5;
			Grid grid = new Grid(gridWidth, gridHeight);
			grid.HorizontalOffset = 50f;
			grid.VerticalOffset = 32f;
			
			int i = 3;
			int j = 4;
			float width = grid.HorizontalOffset + gridWidth * i;
			float height = grid.VerticalOffset + gridHeight * j;
			Assert.That(grid.PixelAtCell(i, j), Is.EqualTo(new Vector2(width, height)));
		}
		
	}
}

