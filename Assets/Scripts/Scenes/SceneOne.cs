using UnityEngine;
using System;
using System.Collections.Generic;

class SceneOne : Scene {
	public Sprite background;
	public Sprite same;
	public Sprite notSame;
	public Sprite circle;
	public Sprite triangle;
	
	Wiggler wiggler;
	TouchSensor sensor;
	SpriteCollection shapes;

	Cycler notSameCycler;
	Cycler circleCycler;
	Cycler triangleCycler;

	const int triangleWaitTime = 4;

 	// animate both shapes at the same frequency
	const float shapeSpeed = 0.5f;

	public SceneOne(SceneManager manager) : base(manager) {
		timeLength = 8.0f;
	}

	public override void LoadAssets() {
		background = FullScreenQuad.create("TitleScene/bg");
		background.visible(false);

		same = Sprite.create(this, "same");
		notSame = Sprite.create(this, "notsame", "notsame_caps", "notsame_g1", "notsame_g2");
		circle = Sprite.create(this, "circle1", "circle2", "circle3", "circle4", "circle5");
		triangle = Sprite.create(this, "triangle1", "triangle2", "triangle3");

		same.visible(false);
		notSame.visible(false);
		circle.visible(false);
		triangle.visible(false);
	}

	public override void Setup(float startTime) {
		background.visible(true);
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

		sensor = new TouchSensor(input, gameObjectFinder);
		shapes = new SpriteCollection(new[] {circle, triangle}, camera, sensor);
	}

	public override void Destroy() {
		Sprite.Destroy(circle);
		Sprite.Destroy(triangle);
		Sprite.Destroy(same);
		Sprite.Destroy(notSame);
		FullScreenQuad.Destroy(background);
		wiggler.Destroy();
	}

	public override void Update () {
		float now = Time.time;
		wiggler.Update(now);
		notSameCycler.Update(now);

		if (solved) return;

		if (shapes.touchedAtSameTime(now) && triangleShowing()) {
			messagePromptCoordinator.clearTouch();
			messagePromptCoordinator.progress("stop shapes from changing");
			Handheld.Vibrate();
			wiggler.wiggleNow(now);
			endScene();
		} else {
			messagePromptCoordinator.hintWhenTouched(GameObject => {}, sensor, now,
				new Dictionary<GameObject, ActionResponsePair[]> {
					{circle.gameObject,   new [] {new ActionResponsePair("stop circle from changing",   new[] {"Nope."})}},
					{triangle.gameObject, new [] {new ActionResponsePair("stop triangle from changing", new[] {"Nope."})}},
				});
		}

		AnimateShapes(now);

		// if touched circle, draw its bright first frame
		if (sensor.changeInsideSprite(Camera.main, circle)) {
			circle.setFrame(0);
			circle.Animate();
		}

		// if touched triangle, ditto
		if (sensor.changeInsideSprite(Camera.main, triangle) && triangleShowing()) {
			triangle.setFrame(0);
			triangle.Animate();
		}
	}

	bool triangleShowing() {
		return triangleCycler != null;
	}

	void AnimateShapes(float time) {
		if (!triangleShowing() && circleCycler.Count() >= triangleWaitTime) {
			triangleCycler = new DelayedCycler(shapeSpeed, 6, 1f);
			triangle.visible(true);
			triangleCycler.AddSprite(triangle);
		}

		circleCycler.Update(time);

		if (triangleShowing()) {
			triangleCycler.Update(time);
		}
	}
}
