using System;
using UnityEngine;
using System.Collections.Generic;

public class SpriteCollection {
	Camera camera;
	AbstractTouchSensor sensor;
	Sprite[] sprites;
	float[] lastTouchedTimes;

	public SpriteCollection(Sprite[] sprites, Camera camera, AbstractTouchSensor sensor) {
		this.sprites = sprites;
		this.camera = camera;
		this.sensor = sensor;
		
		lastTouchedTimes = new float[sprites.Length];
	}
	
	public bool touchedAtSameTime(float now) {
		float min = float.MaxValue;
		float max = float.MinValue;
		
		for (int i = 0; i < sprites.Length; i++) {
			if (!sprites[i].belowFinger(sensor)) {
				return false;
			}

			lastTouchedTimes[i] = lastTouchedFor(sprites[i], now, lastTouchedTimes[i]);
			
			if (lastTouchedTimes[i] < min) {
				min = lastTouchedTimes[i];
			}
			
			if (lastTouchedTimes[i] > max) {
				max = lastTouchedTimes[i];
			}
		}
		
		const float maxDoubleTouchDelay = 0.2f;

		return (max - min) < maxDoubleTouchDelay;
	}

	float lastTouchedFor(Sprite sprite, float now, float lastTouched) {
		const float delayBeforeRegisteringTouch = 0.05f;
		if (sensor.insideSprite(camera, sprite, new[] {TouchPhase.Began})
			&& lastTouched + delayBeforeRegisteringTouch < now) {
			return now;
		}
		return lastTouched;
	}
}

