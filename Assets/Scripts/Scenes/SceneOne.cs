using UnityEngine;
using System;

class SceneOne : Scene {
	public GameObject background;
	public Sprite same;
	public Sprite notSame;
	public Sprite circle;
	public Sprite triangle;
	
	Wiggler wiggler;

	Cycler notSameCycler;
	Cycler circleCycler;
	Cycler triangleCycler;

	int triangleWaitTime = 4;

 	// animate both shapes at the same frequency
	private float shapeSpeed = 0.5f;
	
	private UnityInput input;
	
	public SceneOne(SceneManager manager) : base(manager) {
		timeLength = 8.0f;
		input = new UnityInput();
	}

	public override void LoadAssets() {
		background = resourceFactory.Create("TitleScene/BackgroundQuad");
		background.active = false;

		same = resourceFactory.Create(this, "Same").GetComponent<Sprite>();
		notSame = resourceFactory.Create(this, "NotSame").GetComponent<Sprite>();
		circle = resourceFactory.Create(this, "Circle").GetComponent<Sprite>();
		triangle = resourceFactory.Create(this, "Triangle").GetComponent<Sprite>();

		same.visible(false);
		notSame.visible(false);
		circle.visible(false);
		triangle.visible(false);
	}

	public override void Setup(float startTime) {
		background.active = true;
		same.visible(true);
		notSame.visible(true);
		circle.visible(true);
		triangle.visible(true);

		same.setCenterToViewportCoord(0.35f, 0.66f);
		notSame.setCenterToViewportCoord(0.7f, 0.66f);
		circle.setCenterToViewportCoord(0.3f, 0.33f);
		triangle.setCenterToViewportCoord(0.7f, 0.33f);
		
		// hide the triangle to start
		triangle.visible(false);
		
		circleCycler = new Cycler(shapeSpeed, 0, startTime);
		circleCycler.AddSprite(circle);
		
		notSameCycler = new DelayedCycler(0.2f, 4, 1.2f, startTime);
		notSameCycler.AddSprite(notSame);

		wiggler = new Wiggler(startTime, timeLength, new[] {circle, triangle});
	}

	public override void Destroy() {
		Sprite.Destroy(circle);
		Sprite.Destroy(triangle);
		Sprite.Destroy(same);
		Sprite.Destroy(notSame);
		GameObject.Destroy(background);
		wiggler.Destroy();
	}

	public override void Update () {
		float now = Time.time;
		wiggler.Update(now);
		notSameCycler.Update(now);

		var touch = new TouchSensor(input);

		bool editorTouched = Application.isEditor && input.GetMouseButtonUp(0);
		
		if (editorTouched ||
			(touch.changeInsideSprite(Camera.main, circle) ||
			 touch.changeInsideSprite(Camera.main, triangle))) {
			// solved
			if (touch.insideSprite(Camera.main, circle) &&
			    touch.insideSprite(Camera.main, triangle) &&
			    triangleCycler != null) {
				Handheld.Vibrate();
				wiggler.wiggleNow(now);
				endScene();
			}

			// if touched circle, draw its bright first frame
			if (touch.changeInsideSprite(Camera.main, circle)) {
				circle.setFrame(0);
				circle.Animate();
			}

			// if touched triangle, ditto
			if (touch.changeInsideSprite(Camera.main, triangle)) {
				triangle.setFrame(0);
				triangle.Animate();
			}
			// don't continue animating once they tap both at the same time
		} else if (!solved) {
			AnimateShapes();
		}
	}
	
	void AnimateShapes() {
		circleCycler.Update(Time.time);

		if (triangleCycler == null && circleCycler.Count() >= triangleWaitTime) {
			triangleCycler = new DelayedCycler(shapeSpeed, 6, 1f);
			triangle.visible(true);
			triangleCycler.AddSprite(triangle);
		}
		if (triangleCycler != null) {
			triangleCycler.Update(Time.time);
		}
	}
}
