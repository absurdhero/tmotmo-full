using UnityEngine;
using System.Collections;

public class SceneFourteen : Scene {
	DitheredBlueBackground background;
	FallingGuyProp fallingGuyProp;
	Metronome animation;
	UnityInput input;
	bool leftHandTouched = false, rightHandTouched = false;

	public SceneFourteen(SceneManager manager, FallingGuyProp fallingGuyProp) : base(manager) {
		timeLength = 8.0f;
		this.fallingGuyProp = fallingGuyProp;
		input = new UnityInput();
		background = new DitheredBlueBackground(resourceFactory);
	}

	public override void LoadAssets() {
		background.LoadAssets();
	}

	public override void Setup (float startTime) {
		background.Show();

		animation = new Metronome(startTime, 0.1f);
		fallingGuyProp.Setup();
		fallingGuyProp.ensureArmIsOnLeftOfScreen();
	}

	public override void Update () {
		for (int i = 0; i < input.touchCount; i++) {
			var touch = input.GetTouch(i);
			leftHandTouched |= fallingGuyProp.otherArm.Contains(Camera.main, touch.position);
			rightHandTouched |= fallingGuyProp.guyWithArmOut.Contains(Camera.main, touch.position);
			leftHandTouched |= fallingGuyProp.otherArm.Contains(fallingGuyProp.wrapCam, touch.position);
			rightHandTouched |= fallingGuyProp.guyWithArmOut.Contains(fallingGuyProp.wrapCam, touch.position);
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
		background.Destroy();
		fallingGuyProp.Destroy();
	}
}
