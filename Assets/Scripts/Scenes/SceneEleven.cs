using UnityEngine;
using System.Collections;

public class SceneEleven : Scene {
	Metronome videoSpeed;
	GameObject videoClip;
	Sprite videoSprite;

	public SceneEleven(SceneManager manager) : base(manager) {
		timeLength = 6f;
	}
	
	public override void LoadAssets() {
		videoClip = resourceFactory.Create("SceneTen/SideGuitarClip");
		videoClip.active = false;
	}
	
	public override void Setup (float startTime) {
		endScene();

		videoSpeed = new Metronome(startTime, 0.33333333f);

		videoClip.active = true;
		videoSprite = videoClip.GetComponent<Sprite>();
		videoSprite.setScreenPosition(0, 0);

	}

	public override void Update () {
		if (videoSpeed.isNextTick(Time.time)) {
			videoSprite.DrawNextFrame();
		}
	}

	public override void Destroy () {
		GameObject.Destroy(videoClip);
	}
}
