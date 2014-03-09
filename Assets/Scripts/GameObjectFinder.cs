using System;
using UnityEngine;
using System.Collections.Generic;

public class GameObjectFinder {
	public virtual List<Sprite> allSprites() {
		var sprites = new List<Sprite>();
		foreach (var inScene in all()) {
			var possibleSpriteInScene = inScene.GetComponent<Sprite>();
			if (possibleSpriteInScene != null) {
				sprites.Add(possibleSpriteInScene);
			}
		}
		return sprites;
	}

	public virtual GameObject[] all() {
		return UnityEngine.Object.FindObjectsOfType(typeof(GameObject)) as GameObject[];
	}
}

