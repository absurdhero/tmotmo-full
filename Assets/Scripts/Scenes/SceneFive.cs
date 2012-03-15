using UnityEngine;
using System;
using System.Collections.Generic;

class SceneFive : Scene {
	GameObject background;
	GameObject faceLeft;
	GameObject faceRight;
	
	BigMouthAnimator bigMouthAnimator;
	LipsAppear lips;
	
	public SceneFive(SceneManager manager) : base(manager) {
	}

	public override void Setup () {
		timeLength = 4.0f;
		endScene(); // no interaction required to continue

		background = resourceFactory.Create(this, "PurpleQuad");
		faceLeft = resourceFactory.Create(this, "FaceLeft");
		var leftPosition = faceLeft.transform.position;
		leftPosition.x = -5f;
		leftPosition.y = -6f;
		faceLeft.transform.position = leftPosition;
		
		faceRight = resourceFactory.Create(this, "FaceRight");
		var rightPosition = faceRight.transform.position;
		rightPosition.x = 1f;
		rightPosition.y = -6f;
		faceRight.transform.position = rightPosition;
		
		bigMouthAnimator = new BigMouthAnimator(resourceFactory);
		lips = new LipsAppear(resourceFactory);
	}

	public override void Update () {
		bigMouthAnimator.Update(Time.time);
		lips.Update(Time.time);
	}
	
	public override void Destroy () {
		GameObject.Destroy(background);
		GameObject.Destroy(faceLeft);
		GameObject.Destroy(faceRight);
		bigMouthAnimator.Destroy();
		lips.Destroy();
	}

	class LipsAppear : Repeater {
		GameObject lips;
		
		public LipsAppear(GameObjectFactory<string> resourceFactory) : base(0.5f) {
			lips = resourceFactory.Create("SceneFive/Lips");
			var lipsPosition = lips.transform.position;
			lipsPosition.x = -6f;
			lipsPosition.y = -5f;
			lipsPosition.z = -1f;
			lips.transform.position = lipsPosition;
			lips.active = false;
		}

		public override void OnTick ()
		{
			if(currentTick > 6) {
				lips.active = true;
			}
			
			if(currentTick == 8) {
				Reset(Time.time);
				lips.active = false;
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
		
		public BigMouthAnimator(GameObjectFactory<string> resourceFactory) : base(0.25f) {
			var mouthLeftGameObject = resourceFactory.Create("SceneFive/MouthLeft");
			var leftPosition = mouthLeftGameObject.transform.position;
			leftPosition.x = -2.95f;
			leftPosition.y = -5.6f;
			leftPosition.z = -0.5f;
			mouthLeftGameObject.transform.position = leftPosition;
			mouthLeft = mouthLeftGameObject.GetComponent<Sprite>();

			var mouthRightGameObject = resourceFactory.Create("SceneFive/MouthRight");
			var rightPosition = mouthRightGameObject.transform.position;
			rightPosition.x = 1f;
			rightPosition.y = -5.6f;
			rightPosition.z = -0.5f;
			mouthRightGameObject.transform.position = rightPosition;
			mouthRight = mouthRightGameObject.GetComponent<Sprite>();
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
