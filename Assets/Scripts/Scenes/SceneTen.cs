using UnityEngine;
using System.Collections;

public class SceneTen : Scene {
	ClipAnimator animator;
	
	public SceneTen(SceneManager manager) : base(manager) {
	}
	
	public override void Setup () {
		timeLength = 8.0f;
		endScene();
		
		animator = new ClipAnimator(resourceFactory);
	}

	public override void Update () {
		
		animator.Update(Time.time);
	}

	public override void Destroy () {
		animator.Destroy();
	}

	class ClipAnimator : Repeater {
		GameObjectFactory<string> resourceFactory;
		GameObject pedalMockup;
		GameObject videoClip;

		Sprite videoSprite;
		float delayUntilRotoscope = 1.666f;
		float epsilon = 0.0001f;

		
		public ClipAnimator(GameObjectFactory<string> resourceFactory) : base(0.3333333f, 5+19) {
			this.resourceFactory = resourceFactory;
			pedalMockup = resourceFactory.Create("SceneTen/PedalMockup");
			if (delayUntilRotoscope == 0) createVideoClip();
		}
		
		public override void OnTick() {
			if (delayUntilRotoscope > epsilon) {
				delayUntilRotoscope -= interval;
				pedalStomp();
				
				return;
			}

			videoSprite.DrawNextFrame();
			
			
		}
		
		public void Destroy() {
			GameObject.Destroy(videoClip);
			GameObject.Destroy(pedalMockup);
		}
		
		private void pedalStomp() {
			// it is time to start rotoscoping
			if (delayUntilRotoscope <= epsilon) {
				createVideoClip();
			}
		}
		
		private void createVideoClip() {
			videoClip = resourceFactory.Create("SceneTen/SideGuitarClip");
			videoSprite = videoClip.GetComponent<Sprite>();
			videoSprite.setScreenPosition(0, 0);
		}
	}

}
