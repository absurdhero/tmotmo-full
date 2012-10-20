using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TouchSensor {
	UnityInput input;
	TouchPhase ANY_PHASE = TouchPhase.Began | TouchPhase.Canceled | TouchPhase.Ended | TouchPhase.Moved | TouchPhase.Stationary;
	public TouchSensor(UnityInput input) {
		this.input = input;
	}

	private IEnumerable<Touch> allTouches {
		get { return touchesFor(TouchPhase.Began); }
	}

	IEnumerable<Touch> touchesFor(TouchPhase phase) {
		for (int i = 0; i < input.touchCount; i++) {
			var touch = input.GetTouch(i);
			if (phase == ANY_PHASE || touch.phase == phase)
				yield return touch;
		}
	}

	public bool editorTouched {
		get { return Application.isEditor && input.GetMouseButtonUp(0); }
	}

	public bool insideSprite(Camera camera, Sprite sprite) {
		return insideSprite(camera, sprite, TouchPhase.Began);
	}

	public bool changeInsideSprite(Camera camera, Sprite sprite) {
		return insideSprite(camera, sprite, ANY_PHASE);
	}

	public bool insideSprite(Camera camera, Sprite sprite, TouchPhase phase) {
		foreach (var touch in touchesFor(phase)) {
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
