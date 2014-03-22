using UnityEngine;

public static class SpriteTouchExtentions {
	public static bool belowFinger(this Sprite obj, AbstractTouchSensor sensor) {
		return sensor.insideSprite(Camera.main, obj, new[] {TouchPhase.Began, TouchPhase.Moved, TouchPhase.Stationary});
	}
}
