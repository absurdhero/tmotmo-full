using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TouchSensor {
	UnityInput input;
	
	public TouchPhase[] allPhases = (TouchPhase[]) Enum.GetValues(typeof(TouchPhase));

	public TouchSensor(UnityInput input) {
		this.input = input;
	}

	private IEnumerable<Touch> allTouches {
		get { return touchesFor(new[] {TouchPhase.Began}); }
	}

	IEnumerable<Touch> touchesFor(ICollection<TouchPhase> phases) {
		for (int i = 0; i < input.touchCount; i++) {
			var touch = input.GetTouch(i);
			if (phases.Contains(touch.phase)) {
				yield return touch;
			}
		}
	}

	public bool insideSprite(Camera camera, Sprite sprite) {
		return insideSprite(camera, sprite, new[] {TouchPhase.Began});
	}

	public bool changeInsideSprite(Camera camera, Sprite sprite) {
		return insideSprite(camera, sprite, allPhases);
	}

	public bool insideSprite(Camera camera, Sprite sprite, ICollection<TouchPhase> phases) {
		foreach (var touch in touchesFor(phases)) {
			if (sprite.Contains(camera, touch.position))
				return true;
		}

		return false;
	}

	public bool any() {
		return allTouches.Any();
	}
}
