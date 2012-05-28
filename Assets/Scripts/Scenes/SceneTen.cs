using UnityEngine;
using System.Collections.Generic;

public class SceneTen : Scene {
	VideoClipAnimator videoAnimator;
	StompAnimator stompAnimator;
	
	public SceneTen(SceneManager manager) : base(manager) {
	}
	
	public override void Setup () {
		timeLength = 8.0f;
		endScene();
		
		videoAnimator = new VideoClipAnimator(resourceFactory);
		stompAnimator = new StompAnimator(resourceFactory, videoAnimator);
	}

	public override void Update () {
		stompAnimator.Update(Time.time);		
		videoAnimator.Update(Time.time);
	}

	public override void Destroy () {
		stompAnimator.Destroy();
		videoAnimator.Destroy();
	}
	
	class StompAnimator : Repeater {
		GameObjectFactory<string> resourceFactory;
		VideoClipAnimator video;
		
		GameObject amp;
		GameObject shoe;
		GameObject wireShadow;
		GameObject glassCracks;
		GameObject wires;
		GameObject background;
		
		Sprite ampSprite { get { return amp.GetComponent<Sprite>(); }}
		Sprite shoeSprite { get { return shoe.GetComponent<Sprite>(); }}
		Sprite glassSprite { get { return glassCracks.GetComponent<Sprite>(); }}
		Sprite wiresSprite { get { return wires.GetComponent<Sprite>(); }}
		Sprite wireShadowSprite { get { return wireShadow.GetComponent<Sprite>(); }}
		
		Animatable pedalStomped, pedalUnStomped, stompFoot, retractFoot;
		Animatable[] animatables;
		
		const int firstCrackFrame = 3;

		
		public StompAnimator(GameObjectFactory<string> resourceFactory, VideoClipAnimator video) : base(0.125f, 16) {
			this.resourceFactory = resourceFactory;
			this.video = video;

			// double the scale on all of these because the art is half-size
			background = resourceFactory.Create("SceneTen/GreenBackground");
			shoe = resourceFactory.Create("SceneTen/Shoe");
			shoe.transform.localScale = Vector3.one * 2;
			shoe.GetComponent<Sprite>().setScreenPosition(0, 164);
			amp = resourceFactory.Create("SceneTen/Amp");
			amp.GetComponent<Sprite>().setScreenPosition(130, 50);
			amp.transform.localScale = Vector3.one * 2;
			wires = resourceFactory.Create("SceneTen/Wires");
			wires.GetComponent<Sprite>().setScreenPosition(30, 30);
			wires.transform.localScale = Vector3.one * 2;			
			wireShadow = resourceFactory.Create("SceneTen/WireShadow");
			wireShadow.GetComponent<Sprite>().setScreenPosition(30, 30);
			wireShadow.transform.localScale = Vector3.one * 2;
			
			pedalStomped = new PedalStomped(new List<Sprite> {ampSprite, wiresSprite, wireShadowSprite});
			pedalUnStomped = new PedalUnStomped(new List<Sprite> {ampSprite, wiresSprite, wireShadowSprite});
			stompFoot = new StompFoot(new List<Sprite> {shoeSprite});
			retractFoot = new RetractFoot(new List<Sprite> {shoeSprite});
			animatables = new[] {pedalStomped, pedalUnStomped, stompFoot, retractFoot};
				
		}
		
		public void beginCracks() {
			glassCracks = resourceFactory.Create("SceneTen/GlassCracks");
			glassCracks.transform.localScale = Vector3.one * 2;
			glassSprite.setScreenPosition(0, 0);
			glassSprite.setDepth(0f);
		}
		
		public override void OnTick() {
			foreach(var animatable in animatables) {
				animatable.animate(currentTick);
			}

			if (currentTick == ticks) {
				video.Show();
			}

			if (currentTick == firstCrackFrame) {
				beginCracks();
			}
			
			if (currentTick > firstCrackFrame) {
				glassSprite.DrawNextFrame();
			}
		}


		public void Destroy() {
			GameObject.Destroy(amp);
			GameObject.Destroy(shoe);
			GameObject.Destroy(wireShadow);
			GameObject.Destroy(glassCracks);
			GameObject.Destroy(wires);
			GameObject.Destroy(background);
		}
	}

	class PedalStomped : Mover {
		static readonly Vector3 stompDisplacement = new Vector3(10, -10);

		public PedalStomped(ICollection<Sprite> sprites) : base(sprites, stompDisplacement, 4, 4) {}
	}

	class PedalUnStomped : Mover {
		static readonly Vector3 unstompDisplacement = new Vector3(-10, 10);

		public PedalUnStomped(ICollection<Sprite> sprites) : base(sprites, unstompDisplacement, 6, 6) {}
	}
	
	class StompFoot : Mover {
		static readonly Vector3 descent = new Vector3(30f, -40f);

		public StompFoot(ICollection<Sprite> sprites) : base(sprites, descent, 0, 4) {}
	}

	class RetractFoot : Mover {
		static readonly Vector3 ascent = new Vector3(-30f, 40f);

		public RetractFoot(ICollection<Sprite> sprites) : base(sprites, ascent, 4, 8) {}
	}

	class VideoClipAnimator : Repeater {
		GameObjectFactory<string> resourceFactory;
		GameObject pedalMockup;
		GameObject videoClip;

		Sprite videoSprite;
		float delayUntilRotoscope = 1.666f;
		float epsilon = 0.0001f;

		
		public VideoClipAnimator(GameObjectFactory<string> resourceFactory) : base(0.3333333f, 5+19) {
			this.resourceFactory = resourceFactory;
			pedalMockup = resourceFactory.Create("SceneTen/PedalMockup");
			pedalMockup.active = false;
			if (delayUntilRotoscope == 0) createVideoClip();
		}
		
		public void Show() {
			pedalMockup.active = true;
		}
		
		public override void OnTick() {
			if (delayUntilRotoscope > epsilon) {
				delayUntilRotoscope -= interval;

				if (delayUntilRotoscope <= epsilon) {
					createVideoClip();
				}
				return;
			}

			videoSprite.DrawNextFrame();
		}
		
		public void Destroy() {
			GameObject.Destroy(videoClip);
			GameObject.Destroy(pedalMockup);
		}
		
		private void createVideoClip() {
			videoClip = resourceFactory.Create("SceneTen/SideGuitarClip");
			videoSprite = videoClip.GetComponent<Sprite>();
			videoSprite.setScreenPosition(0, 0);
		}
	}

}
