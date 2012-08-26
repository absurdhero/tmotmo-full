using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TouchSensor {
	UnityInput input;

	public TouchSensor(UnityInput input) {
		this.input = input;
	}

	private IEnumerable<Touch> allTouches {
		get {
			for (int i = 0; i < input.touchCount; i++) {
				var touch = input.GetTouch(i);
				if (touch.phase == TouchPhase.Began)
					yield return touch;
			}
		}
	}

	public bool editorTouched {
		get { return Application.isEditor && input.GetMouseButtonUp(0); }
	}

	public bool insideSprite(Camera camera, Sprite sprite) {
		foreach (var touch in allTouches) {
			if (sprite.Contains(camera, touch.position))
				return true;
		}

		if (editorTouched && sprite.Contains(camera, input.mousePosition))
			return true;

		return false;
	}

	public bool any() {
		return allTouches.Any() || editorTouched;
	}
}
