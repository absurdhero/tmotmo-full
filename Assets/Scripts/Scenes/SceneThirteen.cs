using UnityEngine;
using System.Collections;

public class SceneThirteen : Scene {
	DitheredBlueBackground background;
	public FallingGuyProp fallingGuyProp { get; private set; }
	Metronome animation;

	public SceneThirteen(SceneManager manager, FallingGuyProp fallingGuyProp) : base(manager) {
		timeLength = 8.0f;
		this.fallingGuyProp = fallingGuyProp;
		permitUnloadResources = false;
		background = new DitheredBlueBackground(resourceFactory);
	}

	public override void LoadAssets() {
		background.LoadAssets();
	}

	public override void Setup (float startTime) {
		endScene();
		
		background.Show();

		animation = new Metronome(startTime, 0.1f);
		fallingGuyProp.Setup();
		fallingGuyProp.ensureArmIsOnLeftOfScreen();
	}

	public override void Update () {
		fallingGuyProp.updateFallingGuyAndMoveApart(animation, Time.time);
	}

	public override void Destroy () {
		background.Destroy();
	}
}
