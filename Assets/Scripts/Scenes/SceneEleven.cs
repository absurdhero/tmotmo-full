using UnityEngine;
using System.Collections;

public class SceneEleven : Scene {
	Metronome videoSpeed;
	GameObject videoClip;
	Sprite videoSprite;

	public SceneEleven(SceneManager manager) : base(manager) {
	}
	
	public override void Setup () {
		timeLength = 5f;
		endScene();

		videoSpeed = new Metronome(Time.time, 0.33333333f);

		videoClip = resourceFactory.Create("SceneTen/SideGuitarClip");
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
