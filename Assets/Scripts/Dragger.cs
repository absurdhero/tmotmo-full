using UnityEngine;
using System;

public class Dragger {
	AbstractInput input;
	Sprite sprite;
	Vector3 startPosition;
	
	public Dragger(AbstractInput input, Sprite sprite) {
		this.input = input;
		this.sprite = sprite;
		this.startPosition = sprite.getScreenPosition();
	}
	
	/// if the sprite was dragged, return move amount in screen coordinates.
	/// return the zero vector if dragging happened outside of the sprite
	public Vector3 movementIfDragged() {
		if (input.touchCount > 0 && input.hasMoved(0)) {
			var touch = input.GetTouch(0);
			if (!sprite.Contains(Camera.main, touch.position)) return Vector3.zero;
			return new Vector3(touch.deltaPosition.x, touch.deltaPosition.y, 0f);
		}
		return Vector3.zero;
	}
	
	public Vector3 totalDragDistance() {
		return sprite.getScreenPosition() - startPosition;
	}
}
