using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TouchSensor : MarshalByRefObject {
	UnityInput input;
	
	public static readonly TouchPhase[] allPhases = (TouchPhase[]) Enum.GetValues(typeof(TouchPhase));

	public TouchSensor(UnityInput input) {
		this.input = input;
	}

	private IList<Touch> allTouches {
		get { return touchesFor(new[] {TouchPhase.Began}); }
	}

	IList<Touch> touchesFor(ICollection<TouchPhase> phases) {
		var touches = new List<Touch>();
		for (int i = 0; i < input.touchCount; i++) {
			var touch = input.GetTouch(i);
			if (phases.Contains(touch.phase)) {
				touches.Add(touch);
			}
		}
		return touches;
	}

	public bool insideSprite(Camera camera, Sprite sprite) {
		return insideSprite(camera, sprite, new[] {TouchPhase.Began});
	}

	public bool changeInsideSprite(Camera camera, Sprite sprite) {
		return insideSprite(camera, sprite, allPhases);
	}

	public bool insideSprite(Camera camera, Sprite sprite, ICollection<TouchPhase> phases) {
		if (input.touchCount == 0) {
			return false;
		}

		if (!touchesFor(phases).Any(touch => sprite.Contains(camera, touch.position))) {
			return false;
		}

		var spritesInScene = new List<Sprite>();
		GameObject[] objectsInScene = UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
		foreach (var inScene in objectsInScene) {
			var possibleSpriteInScene = inScene.GetComponent<Sprite>();
			if (possibleSpriteInScene != null) {
				spritesInScene.Add(possibleSpriteInScene);
			}
		}

		foreach (var touch in touchesFor(phases)) {
			if (!sprite.Contains(camera, touch.position)) {
				continue;
			}

			var topSprite = sprite;
			foreach (var spriteInScene in spritesInScene) {
				if (spriteInScene.Contains(camera, touch.position)
				    && spriteInScene.worldPosition.z < sprite.worldPosition.z) {
					topSprite = spriteInScene;
				}
			}
			if (topSprite == sprite) {
				return true;
			}
		}

		return false;
	}

	public bool hasTaps() {
		return allTouches.Count > 0;
	}
}
