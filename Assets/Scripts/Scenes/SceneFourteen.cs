using UnityEngine;
using System.Collections;

public class SceneFourteen : Scene {
	GameObject background, bottomDither;
	FallingGuyProp fallingGuyProp;
	Metronome animation;
	UnityInput input;
	bool leftHandTouched = false, rightHandTouched = false;

	public SceneFourteen(SceneManager manager, FallingGuyProp fallingGuyProp) : base(manager) {
		timeLength = 8.0f;
		this.fallingGuyProp = fallingGuyProp;
		input = new UnityInput();
	}

	public override void LoadAssets() {
		background = resourceFactory.Create("SceneTwelve/TealBackground");
		bottomDither = resourceFactory.Create("SceneTwelve/BottomDither");
		background.active = false;
		bottomDither.active = false;
	}

	public override void Setup (float startTime) {
		background.active = true;
		bottomDither.active = true;

		animation = new Metronome(startTime, 0.1f);
		fallingGuyProp.Setup();
		fallingGuyProp.ensureArmIsOnLeftOfScreen();
	}

	public override void Update () {
		for (int i = 0; i < input.touchCount; i++) {
			var touch = input.GetTouch(i);
			leftHandTouched |= fallingGuyProp.otherArm.Contains(touch.position);
			rightHandTouched |= fallingGuyProp.guyWithArmOut.Contains(touch.position);
		}
		
		if (Application.isEditor && input.GetMouseButtonUp(0)) {
			if (leftHandTouched) rightHandTouched = true;
			leftHandTouched = true;
		}

		if (leftHandTouched) fallingGuyProp.stopLefSide();
		if (rightHandTouched) fallingGuyProp.stopRightSide();
		
		if (leftHandTouched && rightHandTouched) endScene();
		
		fallingGuyProp.updateFallingGuy(animation);
	}

	public override void Destroy () {
		GameObject.Destroy(background);
		GameObject.Destroy(bottomDither);
		fallingGuyProp.Destroy();
	}
}
