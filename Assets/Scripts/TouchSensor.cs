using UnityEngine;

public class TouchSensor {
	UnityInput input;

	public TouchSensor(UnityInput input) {
		this.input = input;
	}

	public bool insideSprite(Sprite sprite) {
		bool touched = false;
		for (int i = 0; i < input.touchCount; i++) {
			var touch = input.GetTouch(i);
			if (touch.phase == TouchPhase.Began) {
				touched |= sprite.Contains(touch.position);
			}
		}

		if (Application.isEditor && input.GetMouseButtonUp(0)) {
			var pos = input.mousePosition;
			touched |= sprite.Contains(pos);
		}

		return touched;
	}
}
