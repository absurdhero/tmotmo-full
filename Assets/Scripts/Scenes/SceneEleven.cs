using UnityEngine;
using System.Collections;

public class SceneEleven : Scene {
	VideoClipAnimator videoAnimator;

	public SceneEleven(SceneManager manager) : base(manager) {
	}
	
	public override void Setup () {
		timeLength = 6.0f;
		endScene();

		videoAnimator = new VideoClipAnimator(resourceFactory);
	}

	public override void Update () {
		videoAnimator.Update(Time.time);
	}

	public override void Destroy () {
		videoAnimator.Destroy();
	}
	
	class VideoClipAnimator : Repeater {
		GameObject videoClip;

		Sprite videoSprite;

		public VideoClipAnimator(GameObjectFactory<string> resourceFactory) : base(0.3333333f, 5+19) {
			videoClip = resourceFactory.Create("SceneTen/SideGuitarClip");
			videoClip.active = true;
			videoSprite = videoClip.GetComponent<Sprite>();
			videoSprite.setScreenPosition(0, 0);
		}
		
		public override void OnTick() {
			videoSprite.DrawNextFrame();
		}
		
		public void Destroy() {
			GameObject.Destroy(videoClip);
		}
	}
}
