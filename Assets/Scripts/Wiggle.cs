using UnityEngine;
using System.Linq;
using System.Collections.Generic;

class Wiggle : Repeater {
	List<GameObject> centerPivots;
	float sceneStart;
	float sceneLength;
	
	bool doWiggle;
	
	const int zoomTicks = 5;
	const int wiggleTicks = 15;

	public Wiggle(float startTime, float sceneLength, Sprite sprite) :
	this(startTime, sceneLength, new[] {sprite}) {}

	public Wiggle(float startTime, float sceneLength, Sprite[] sprites) : base(0.05f, 0, startTime) {
		sceneStart = startTime;
		this.sceneLength = sceneLength;

		centerPivots = sprites.Select<Sprite, GameObject>(
			sprite => sprite.createPivotOnCenter()).ToList();

		doWiggle = false;
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
			centerPivots.ForEach(pivot => pivot.transform.rotation = Quaternion.identity);
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
		float angle = Mathf.PingPong(currentTick - zoomTicks / 64f * Mathf.PI, Mathf.PI / 16f);
		centerPivots.ForEach(pivot => pivot.transform.Rotate(Vector3.back, angle, Space.Self));
	}
}
