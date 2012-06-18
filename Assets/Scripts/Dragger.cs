using UnityEngine;
using System;

public class Dragger {
	UnityInput input;
	Sprite sprite;
	Vector3 startPosition;
	
	public Dragger(Sprite sprite) {
		this.sprite = sprite;
		this.startPosition = sprite.getScreenPosition();
		input = new UnityInput();		
	}
	
	/// if the sprite was dragged, return move amount in screen coordinates.
	/// the zero vector is dragging happened outside of the sprite
	public Vector3 movementIfDragged() {
		if (input.touchCount > 0 && input.hasMoved(0)) {
			var touch = input.GetTouch(0);
			if (!sprite.Contains(touch.position)) return Vector3.zero;
			return new Vector3(touch.deltaPosition.x, touch.deltaPosition.y, 0f);
		}
		return Vector3.zero;
	}
	
	public Vector3 totalDragDistance() {
		return sprite.getScreenPosition() - startPosition;
	}
}
