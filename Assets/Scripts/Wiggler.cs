using UnityEngine;
using System.Collections.Generic;

class Wiggler : Repeater {
	protected GameObject[] centerPivots;
	Dictionary<GameObject, Vector3> initialScales;
	float sceneStart;
	float sceneLength;
	protected float totalRotation;
	
	bool doWiggle;
	
	protected const int zoomTicks = 5;
	protected const int wiggleTicks = 15;

	private static GameObject[] pivotsFromSprite(Sprite[] sprites) {
		var pivots = new GameObject[sprites.Length];
		for(int i = 0; i < sprites.Length; i++) {
			pivots[i] = sprites[i].createPivotOnCenter();
		}
		return pivots;
	}

	public Wiggler(float startTime, float sceneLength, Sprite sprite) :
	this(startTime, sceneLength, new[] {sprite}) {}

	public Wiggler(float startTime, float sceneLength, Sprite[] sprites) :
	this(startTime, sceneLength, pivotsFromSprite(sprites)) {}

	public Wiggler(float startTime, float sceneLength, GameObject[] centerPivots) : base(0.05f, 0, startTime) {
		sceneStart = startTime;
		this.sceneLength = sceneLength;
		this.centerPivots = centerPivots;

		// preserve the scale of each pivot
		initialScales = new Dictionary<GameObject, Vector3>();
		foreach(var pivot in this.centerPivots) {
			initialScales.Add(pivot, pivot.transform.localScale);
		}

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
			foreach(var pivot in centerPivots) {
				pivot.transform.Rotate(Vector3.back, -totalRotation, Space.Self);
			}
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
		foreach(var pivot in centerPivots) {
			pivot.transform.localScale = initialScales[pivot] * (1f + tick / (float) zoomTicks / 24f);
		}
	}
	
	virtual protected void wiggle() {
		float angle = -Mathf.PingPong(currentTick - zoomTicks / 64f * Mathf.PI, Mathf.PI / 16f);
		totalRotation += angle;
		foreach(var pivot in centerPivots) {
			pivot.transform.Rotate(Vector3.back, angle, Space.Self);
		}
	}
}

class ReverseWiggler : Wiggler {
	public ReverseWiggler(float startTime, float sceneLength, Sprite sprite) :
	base(startTime, sceneLength, sprite) {}

	public ReverseWiggler(float startTime, float sceneLength, Sprite[] sprites) :
	base(startTime, sceneLength, sprites) {}

	public ReverseWiggler(float startTime, float sceneLength, GameObject[] centerPivots) :
	base(startTime, sceneLength, centerPivots) {}
   
	override protected void wiggle() {
		float angle = Mathf.PingPong(currentTick - zoomTicks / 64f * Mathf.PI, Mathf.PI / 16f);
		totalRotation += angle;
		foreach(var pivot in centerPivots) {
			pivot.transform.Rotate(Vector3.back, angle, Space.Self);
		}
	}
}
