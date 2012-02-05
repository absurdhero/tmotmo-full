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

	Color hi = new Color(0.8f, 0.8f, 0.8f, 1f);
	//Fader fader = new Fader(1.0f);

	bool touched1 = false;
	bool touched2 = false;
	
 	// animate both shapes at the same frequency
	private float shapeSpeed = 0.5f;
	
	public SceneOne(SceneManager manager) : base(manager) {
	}

	public override void Setup() {
		timeLength = 8.0f;
		
		background = (GameObject)GameObject.Instantiate(Resources.Load("TitleScreen/BackgroundQuad"));
		same = (GameObject)GameObject.Instantiate(Resources.Load("Scene1/Same"));
		notSame = (GameObject)GameObject.Instantiate(Resources.Load("Scene1/NotSame"));
		circle = (GameObject)GameObject.Instantiate(Resources.Load("Scene1/Circle"));
		triangle = (GameObject)GameObject.Instantiate(Resources.Load("Scene1/Triangle"));		

		same.GetComponent<Sprite>().setCenterToViewportCoord(Camera.main, 0.35f, 0.66f);
		notSame.GetComponent<Sprite>().setCenterToViewportCoord(Camera.main, 0.7f, 0.66f);
		var circleSprite = circle.GetComponent<Sprite>();
		var triangleSprite = triangle.GetComponent<Sprite>();
		circleSprite.setCenterToViewportCoord(Camera.main, 0.3f, 0.33f);
		triangleSprite.setCenterToViewportCoord(Camera.main, 0.7f, 0.33f);
		
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
		AnimateShapes();
		notSameCycler.Update(Time.time);
		
		touched1 = false;
		touched2 = false;
		for (int i = 0; i < Input.touchCount; i++) {
			var touch = Input.GetTouch(i);
			touched1 |= circle.GetComponent<Sprite>().Contains(touch.position);
			touched2 |= triangle.GetComponent<Sprite>().Contains(touch.position);
		}

		if (touched1 && touched2) {
			completed = true;
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
