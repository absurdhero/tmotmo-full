using UnityEngine;
using System.Collections;

public class SceneThirteen : Scene {
	GameObject background, bottomDither;
	FallingGuyProp fallingGuyProp;
	Metronome animation;

	public SceneThirteen(SceneManager manager, FallingGuyProp fallingGuyProp) : base(manager) {
		this.fallingGuyProp = fallingGuyProp;
	}

	public override void LoadAssets() {
		background = resourceFactory.Create("SceneTwelve/TealBackground");
		bottomDither = resourceFactory.Create("SceneTwelve/BottomDither");
		background.active = false;
		bottomDither.active = false;
	}

	public override void Setup () {
		timeLength = 8.0f;
		endScene();

		background.active = true;
		bottomDither.active = true;

		animation = new Metronome(Time.time, 0.1f);
		fallingGuyProp.Setup();
		fallingGuyProp.ensureArmIsOnLeftOfScreen();
	}

	public override void Update () {
		fallingGuyProp.updateFallingGuyAndMoveApart(animation, Time.time);
	}

	public override void Destroy () {
		GameObject.Destroy(background);
		GameObject.Destroy(bottomDither);
		fallingGuyProp.Destroy();
	}
}
