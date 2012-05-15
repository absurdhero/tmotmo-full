using UnityEngine;
using System.Collections;

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
		
		Vector2 stompDistance = new Vector2(60f, 80f);
		Vector3 stompDisplacement = new Vector3(10, 10, 0);
		bool stomped = false;
		const int firstCrackFrame = 3;

		
		public StompAnimator(GameObjectFactory<string> resourceFactory, VideoClipAnimator video) : base(0.125f, 16) {
			this.resourceFactory = resourceFactory;
			this.video = video;

			// double the scale on all of these because the art is half-size
			background = resourceFactory.Create("SceneTen/GreenBackground");
			shoe = resourceFactory.Create("SceneTen/Shoe");
			shoe.transform.localScale = Vector3.one * 2;
			shoe.GetComponent<Sprite>().setScreenPosition(0, 50);
			amp = resourceFactory.Create("SceneTen/Amp");
			amp.GetComponent<Sprite>().setScreenPosition(130, 50);
			amp.transform.localScale = Vector3.one * 2;
			wires = resourceFactory.Create("SceneTen/Wires");
			wires.GetComponent<Sprite>().setScreenPosition(30, 30);
			wires.transform.localScale = Vector3.one * 2;			
			wireShadow = resourceFactory.Create("SceneTen/WireShadow");
			wireShadow.GetComponent<Sprite>().setScreenPosition(30, 30);
			wireShadow.transform.localScale = Vector3.one * 2;
		}
		
		public void beginCracks() {
			glassCracks = resourceFactory.Create("SceneTen/GlassCracks");
			glassCracks.transform.localScale = Vector3.one * 2;
			glassSprite.setScreenPosition(0, 0);
			glassSprite.setDepth(3f);
		}
		
		public override void OnTick() {
			if (currentTick < 11) {
				float movementMagnitude = Mathf.Sin(currentTick * Mathf.PI / 4f);
				moveShoe(movementMagnitude);
				pushAmp(movementMagnitude);
				
				if (currentTick == ticks) {
					video.Show();
				}
			}
			
			if (currentTick == firstCrackFrame) {
				beginCracks();
			}
			
			if (currentTick > firstCrackFrame) {
				glassSprite.DrawNextFrame();
			}
		}

		private void moveShoe(float movementMagnitude) {
			int x = (int)(stompDistance.x * movementMagnitude);
			int y = (int) (Camera.main.pixelHeight - stompDistance.y * movementMagnitude - 156);
			shoeSprite.setScreenPosition(x, y);
		}

		private void pushAmp(float movementMagnitude) {
			if (movementMagnitude == 1.0f) {
				stomped = true;
				ampSprite.setScreenPosition(ampSprite.getScreenPosition() + stompDisplacement);
				wiresSprite.setScreenPosition(wiresSprite.getScreenPosition() + stompDisplacement);
				wireShadowSprite.setScreenPosition(wireShadowSprite.getScreenPosition() + stompDisplacement);
			} 
			if (movementMagnitude != 1.0f && stomped) {
				ampSprite.setScreenPosition(ampSprite.getScreenPosition() - stompDisplacement);
				wiresSprite.setScreenPosition(wiresSprite.getScreenPosition() - stompDisplacement);
				wireShadowSprite.setScreenPosition(wireShadowSprite.getScreenPosition() - stompDisplacement);
				stomped = false;
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
