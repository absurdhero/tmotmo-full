using UnityEngine;

public class Grid {
	public float cellHeight { get; private set; }
	public float cellWidth { get; private set; }
	float horizontalOffset = 0f;
	float verticalOffset = 0f;
	
	public Grid(float cellWidth, float cellHeight) {
		this.cellWidth = cellWidth;
		this.cellHeight = cellHeight;
	}
	
	public Vector2 PixelAtCell(int i, int j) {
		return new Vector2(horizontalOffset + cellWidth * i,
				verticalOffset + cellHeight * j);
	}

	
	public float HorizontalOffset {
		get {
			return this.horizontalOffset;
		}
		set {
			horizontalOffset = value;
		}
	}

	public float VerticalOffset {
		get {
			return this.verticalOffset;
		}
		set {
			verticalOffset = value;
		}
	}
}
