using UnityEngine;
using System.Linq;
using System.Collections.Generic;

class Wiggle : Repeater {
	List<GameObject> centerPivots;
	float sceneStart;
	float sceneLength;
	float totalRotation;
	
	bool doWiggle;
	
	const int zoomTicks = 5;
	const int wiggleTicks = 15;

	private static GameObject[] pivotsFromSprite(Sprite[] sprites) {
		return sprites.Select<Sprite, GameObject>(
			sprite => sprite.createPivotOnCenter()).ToArray();
	}

	public Wiggle(float startTime, float sceneLength, Sprite sprite) :
	this(startTime, sceneLength, new[] {sprite}) {}

	public Wiggle(float startTime, float sceneLength, Sprite[] sprites) :
	this(startTime, sceneLength, pivotsFromSprite(sprites)) {}

	public Wiggle(float startTime, float sceneLength, GameObject[] centerPivots) : base(0.05f, 0, startTime) {
		sceneStart = startTime;
		this.sceneLength = sceneLength;
		this.centerPivots = centerPivots.ToList();
		doWiggle = false;
		totalRotation = 0f;
	}
	
	public override void OnTick() {
		float time = Time.time;
		if (time - sceneStart >= sceneLength && (time - sceneStart) % sceneLength <= interval) {
			wiggleNow(time);
		}
		
		if (!doWiggle) return;
		
		if (currentTick < zoomTicks) {
			zoomIn();
		} else if (currentTick < zoomTicks + wiggleTicks) {
			wiggle();
		} else if (currentTick < zoomTicks + wiggleTicks + zoomTicks) {
			centerPivots.ForEach(pivot => pivot.transform.Rotate(Vector3.back, -totalRotation, Space.Self));
			totalRotation = 0f;
			zoomOut();
		} else {
			doWiggle = false;
		}
	}
	
	public void wiggleNow(float wiggleTime) {
		if (doWiggle) return; // already wiggling
		Reset(wiggleTime);
		doWiggle = true;
	}
	
	public void Destroy() {
		foreach(var pivot in centerPivots) {
			// GameObject.Destroy also destroys children so we must detach each Sprite from its pivot
			pivot.transform.DetachChildren();
			GameObject.Destroy(pivot);
		}
	}
	
	private void zoomIn() {
		zoomFor(currentTick);
	}

	private void zoomOut() {
		int zoomOutTicks = currentTick - zoomTicks - wiggleTicks;
		zoomFor(zoomTicks - zoomOutTicks);
	}
	
	private void zoomFor(int tick) {
		centerPivots.ForEach(pivot => pivot.transform.localScale = Vector3.one * (1f + tick / (float) zoomTicks / 24f));
	}
	
	private void wiggle() {
		float angle = -Mathf.PingPong(currentTick - zoomTicks / 64f * Mathf.PI, Mathf.PI / 16f);
		totalRotation += angle;
		centerPivots.ForEach(pivot => pivot.transform.Rotate(Vector3.back, angle, Space.Self));
	}
}