using UnityEngine;
using System;

class SceneTwo : Scene {
	public GameObject background;
	
	public override void Setup() {
		timeLength = 8.0f;
		
		background = (GameObject)GameObject.Instantiate(Resources.Load("TitleScreen/BackgroundQuad"));

	}

	public override void Destroy() {
		GameObject.Destroy(background);
	}

	public override void Update () {
	}
}