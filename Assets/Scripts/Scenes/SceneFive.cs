using UnityEngine;
using System;

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
		//endScene(); // no interaction required to continue

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
		GameObject mouthLeft;
		GameObject mouthRight;
		int delay = 1;
		int frame = 0;
		
		const int totalFrames = 8;		
		
		public BigMouthAnimator(GameObjectFactory<string> resourceFactory) : base(0.25f) {
			mouthLeft = resourceFactory.Create("SceneFive/MouthLeft");
			var leftPosition = mouthLeft.transform.position;
			leftPosition.x = -2.95f;
			leftPosition.y = -5.6f;
			leftPosition.z = -0.5f;
			mouthLeft.transform.position = leftPosition;

			mouthRight = resourceFactory.Create("SceneFive/MouthRight");
			var rightPosition = mouthRight.transform.position;
			rightPosition.x = 1f;
			rightPosition.y = -5.6f;
			rightPosition.z = -0.5f;
			mouthRight.transform.position = rightPosition;
		}
		
		public override void OnTick() {
			if (delay > 0) {
				delay -= 1;
				return;
			}

			moveMouth(0, 4);
			moveMouth(5, 10);
			moveMouth(10, 11);
			
			incrementFrame();
		}
		
		public void Destroy() {
			GameObject.Destroy(mouthLeft);
			GameObject.Destroy(mouthRight);
		}
		
		private void incrementFrame() {
			frame = (frame + 1) % totalFrames;
		}

		private void moveMouth(int start, int end) {
			if(frame > start && frame < end) {
				mouthLeft.GetComponent<Sprite>().NextTexture();
				mouthRight.GetComponent<Sprite>().NextTexture();
			}
		}
	}
}
