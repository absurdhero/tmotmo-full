using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TouchSensor : MarshalByRefObject {
	const float OPAQUE = 0.01f;
	
	AbstractInput input;

	GameObjectFinder gameObjectFinder;
	
	public static readonly TouchPhase[] allPhases = (TouchPhase[]) Enum.GetValues(typeof(TouchPhase));

	public TouchSensor(AbstractInput input, GameObjectFinder gameObjectFinder) {
		this.input = input;
		this.gameObjectFinder = gameObjectFinder;
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

		var spritesInScene = gameObjectFinder.allSprites();

		foreach (var touch in touchesFor(phases)) {
			if (!sprite.Contains(camera, touch.position)) {
				continue;
			}

			var topSprite = sprite;
			foreach (var spriteInScene in spritesInScene) {
				if (spriteInScene.Contains(camera, touch.position)
				    && spriteInScene.worldPosition.z < sprite.worldPosition.z
				    && (spriteInScene.getAlphaAtScreenPosition(touch.position) > OPAQUE)) {
					topSprite = spriteInScene;
				}
			}
			if (topSprite == sprite
			    && topSprite.getAlphaAtScreenPosition(touch.position) > OPAQUE) {
			    return true;
			}
		}

		return false;
	}

	public bool hasTaps() {
		return allTouches.Count > 0;
	}
}
