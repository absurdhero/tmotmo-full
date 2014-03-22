using UnityEngine;
using System;
using System.Collections.Generic;

class SceneFive : AbstractScene {
	BigHeadProp bigHeadProp;
	
	BigMouthAnimator bigMouthAnimator;
	LipsAppear lips;
	
	public SceneFive(SceneManager manager) : base(manager) {
		timeLength = 4.0f;
		bigHeadProp = new BigHeadProp(resourceFactory);
	}
	
	public override void LoadAssets() {
		lips = new LipsAppear(resourceFactory);
	}

	public override void Setup (float startTime) {
		endScene(); // no interaction required to continue
		
		bigHeadProp.Setup();
		
		bigMouthAnimator = new BigMouthAnimator(startTime, resourceFactory);
	}

	public override void Update () {
		bigMouthAnimator.Update(Time.time);
		lips.Update(Time.time);
	}
	
	public override void Destroy () {
		bigHeadProp.Destroy();
		bigMouthAnimator.Destroy();
		lips.Destroy();
	}

	class LipsAppear : Repeater {
		GameObject lips;
		
		public LipsAppear(GameObjectFactory<string> resourceFactory) : base(0.5f) {
			lips = resourceFactory.Create("SceneFive/Lips");
			lips.GetComponent<Sprite>().setWorldPosition(-60f, -50f, -10f);
			lips.SetActive(false);
		}

		public override void OnTick ()
		{
			if(currentTick > 6) {
				lips.SetActive(true);
			}
			
			if(currentTick == 8) {
				Reset(Time.time);
				lips.SetActive(false);
			}
		}
		
		public void Destroy() {
			GameObject.Destroy(lips);
		}

		
	}
	
	class BigMouthAnimator : Repeater {
		Sprite mouthLeft;
		Sprite mouthRight;
		int sceneFrame = 0;
		
		const int totalFramesInScene = 16;		
		
		public BigMouthAnimator(float startTime, GameObjectFactory<string> resourceFactory) : base(0.25f, 0, startTime) {
			var mouthLeftGameObject = resourceFactory.Create("SceneFive/MouthLeft");
			mouthLeft = mouthLeftGameObject.GetComponent<Sprite>();
			mouthLeft.setWorldPosition(-29.5f, -56f, -5f);

			var mouthRightGameObject = resourceFactory.Create("SceneFive/MouthRight");
			mouthRight = mouthRightGameObject.GetComponent<Sprite>();
			mouthRight.setWorldPosition(10f, -56f, -5f);
		}
		
		public override void OnTick() {
			var sprites = getSpritesFor(sceneFrame);

			foreach(var sprite in sprites) {
				sprite.Animate();
			}

			incrementFrame();
		}
		
		private ICollection<Sprite> getSpritesFor(int sceneFrame) {
			if (sceneFrame == 0) return initialMouthFrame();
			if (sceneFrame >= 1 && sceneFrame <= 3) return sayThis();
			if (sceneFrame >= 4 && sceneFrame <= 4) return sayIs();
			if (sceneFrame >= 5 && sceneFrame <= 6) return sayOur();
			if (sceneFrame >= 9 && sceneFrame <= 10) return sayKiss();
			if (sceneFrame >= 11 && sceneFrame <= 14) return sayGoodbye();
			
			return new List<Sprite>();
		}
		
		public void Destroy() {
			GameObject.Destroy(mouthLeft.gameObject);
			GameObject.Destroy(mouthRight.gameObject);
		}
		
		private void incrementFrame() {
			sceneFrame = (sceneFrame + 1) % totalFramesInScene;
		}
		

		private ICollection<Sprite> initialMouthFrame()
		{
			setMouthFrame(0);
			return new List<Sprite>{ mouthLeft, mouthRight };
		}

		private ICollection<Sprite> sayThis() {
			if (sceneFrame == 1)	{
				setMouthFrame(1);
			}
			else nextMouthFrame();
			
			return new List<Sprite>{ mouthLeft, mouthRight };
		}

		private ICollection<Sprite> sayIs() {
			if (sceneFrame == 4)	{
				setMouthFrame(4);
			}
			else nextMouthFrame();
			return new List<Sprite>{ mouthLeft, mouthRight };
		}
		
		private ICollection<Sprite> sayOur() {
			if (sceneFrame == 5) {
				setMouthFrame(5);
			}
			else nextMouthFrame();
			return new List<Sprite>{ mouthLeft, mouthRight };
		}
		
		private ICollection<Sprite> sayKiss() {
			if (sceneFrame == 9) {
				setMouthFrame(7);
			}
			else nextMouthFrame();
			return new List<Sprite>{ mouthLeft, mouthRight };
		}

		private ICollection<Sprite> sayGoodbye() {
			if (sceneFrame == 11) {
				setMouthFrame(9);
			}
			else nextMouthFrame();
			return new List<Sprite>{ mouthLeft, mouthRight };
		}
		
		private void setMouthFrame(int index) {
			mouthLeft.setFrame(index);
			mouthRight.setFrame(index);
		}
		
		private void nextMouthFrame() {
			mouthLeft.nextFrame();
			mouthRight.nextFrame();
		}
	}
}
