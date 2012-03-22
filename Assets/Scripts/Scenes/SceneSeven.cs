using UnityEngine;
using System;

class SceneSeven : Scene {
	BigHeadProp bigHeadProp;
	
	public SceneSeven(SceneManager manager) : base(manager) {
		bigHeadProp = new BigHeadProp(resourceFactory);
	}

	public override void Setup () {
		timeLength = 4.0f;
		endScene();
		
		bigHeadProp.Setup();
	}

	public override void Update () {
	}

	public override void Destroy () {
		bigHeadProp.Destroy();
	}
}
