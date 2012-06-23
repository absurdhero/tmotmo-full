using UnityEngine;
using System;

class SceneOne : Scene {
	public GameObject background;
	public GameObject same;
	public GameObject notSame;
	public GameObject circle;
	public GameObject triangle;
	
	Cycler notSameCycler;
	Cycler circleCycler;
	Cycler triangleCycler;

	int triangleWaitTime = 4;

	//Color hi = new Color(0.8f, 0.8f, 0.8f, 1f);
	//Fader fader = new Fader(1.0f);

	bool touched1 = false;
	bool touched2 = false;
	
 	// animate both shapes at the same frequency
	private float shapeSpeed = 0.5f;
	
	private UnityInput input;
	
	public SceneOne(SceneManager manager) : base(manager) {
		input = new UnityInput();
	}

	public override void Setup() {
		timeLength = 8.0f;
		
		background = resourceFactory.Create("TitleScene/BackgroundQuad");
		same = resourceFactory.Create(this, "Same");
		notSame = resourceFactory.Create(this, "NotSame");
		circle = resourceFactory.Create(this, "Circle");
		triangle = resourceFactory.Create(this, "Triangle");

		same.GetComponent<Sprite>().setCenterToViewportCoord(0.35f, 0.66f);
		notSame.GetComponent<Sprite>().setCenterToViewportCoord(0.7f, 0.66f);
		var circleSprite = circle.GetComponent<Sprite>();
		var triangleSprite = triangle.GetComponent<Sprite>();
		circleSprite.setCenterToViewportCoord(0.3f, 0.33f);
		triangleSprite.setCenterToViewportCoord(0.7f, 0.33f);
		
		// hide the triangle to start
		triangle.active = false;
		
		circleCycler = new Cycler(shapeSpeed);
		circleCycler.AddSprite(circle);
		
		notSameCycler = new DelayedCycler(0.2f, 4, 1.2f);
		notSameCycler.AddSprite(notSame);
	}

	public override void Destroy() {
		GameObject.Destroy(circle);
		GameObject.Destroy(triangle);
		GameObject.Destroy(same);
		GameObject.Destroy(notSame);
		GameObject.Destroy(background);
	}

	public override void Update () {
		notSameCycler.Update(Time.time);
		
		touched1 = false;
		touched2 = false;
		for (int i = 0; i < input.touchCount; i++) {
			var touch = input.GetTouch(i);
			touched1 |= circle.GetComponent<Sprite>().Contains(touch.position);
			touched2 |= triangle.GetComponent<Sprite>().Contains(touch.position);
		}
		
		if (Application.isEditor && input.GetMouseButtonUp(0)) {
			touched1 = true;
			touched2 = true;
		}
		
		if (touched1 && touched2) {
			Handheld.Vibrate();
			endScene();
		} else {
			AnimateShapes();
		}
	}
	
	void AnimateShapes() {
		circleCycler.Update(Time.time);
		if (circleCycler.Count() == triangleWaitTime) {
			triangleCycler = new DelayedCycler(shapeSpeed, 6, 1f);
			triangle.active = true;
			triangleCycler.AddSprite(triangle);
		}
		if (triangleCycler != null) {
			triangleCycler.Update(Time.time);
		}
	}
}
