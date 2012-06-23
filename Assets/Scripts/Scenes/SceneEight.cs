using UnityEngine;
using System;
using System.Collections.Generic;

class SceneEight : Scene {
	public Confetti confetti;

	BigHeadProp bigHeadProp;
	GameObject faceRightParent;
	MouthAnimator mouthAnimator;
	Sprite mouthLeft, mouthRight;
	
	Vector3 previousMousePosition;

	private UnityInput input;
	
	public SceneEight(SceneManager manager) : base(manager) {
		bigHeadProp = new BigHeadProp(resourceFactory);
		input = new UnityInput();
		confetti = new Confetti();
	}

	public override void Setup () {
		timeLength = 4.0f;
		
		bigHeadProp.Setup();
		faceRightParent = bigHeadProp.faceRight.createPivotOnTopLeftCorner();

		var mouthLeftGameObject = resourceFactory.Create(this, "MouthLeft-ItsInside");
		mouthLeft = mouthLeftGameObject.GetComponent<Sprite>();
		mouthLeft.setWorldPosition(-29.5f, -56f, -5f);

		var mouthRightGameObject = resourceFactory.Create(this, "MouthRight-ItsInside");
		mouthRightGameObject.transform.parent = faceRightParent.transform;
		mouthRight = mouthRightGameObject.GetComponent<Sprite>();
		mouthRight.setWorldPosition(10f, -56f, -5f);

		mouthAnimator = new MouthAnimator(mouthLeft, mouthRight);
	}

	public override void Update () {
		mouthAnimator.Update(Time.time);
		
		if (fullyTilted() && !confetti.pouring) {
			confetti.Pour(Time.time);
		}
		
		if (confetti.pouring) {
			confetti.Update(Time.time);
		}
		
		if (confetti.finishedPouring) {
			endScene();
		}

		if(completed) return;
		setLocationToTouch();
	}

	public override void Destroy () {
		GameObject.Destroy(faceRightParent);
		bigHeadProp.Destroy();
		mouthAnimator.Destroy();
	}

	void setLocationToTouch() {
		Vector3 movementDelta = Vector3.zero;
		
		if (Application.isEditor && input.GetMouseButton(0)) {
			movementDelta = input.mousePosition - previousMousePosition;
		}
		previousMousePosition = input.mousePosition;
		
		if (input.touchCount > 0 && input.GetTouch(0).phase == TouchPhase.Moved) {
			if (!bigHeadProp.faceRight.Contains(input.GetTouch(0).position)) return;
			movementDelta = new Vector3(input.GetTouch(0).deltaPosition.x, input.GetTouch(0).deltaPosition.y, 0f);
		}
		moveToLocation(movementDelta);
	}
	
	void moveToLocation(Vector3 movementDelta) {
		if (fullyTilted()) return;

		float squareMagnitude = movementDelta.x + movementDelta.y;
		if (squareMagnitude < 0) return;
		
		faceRightParent.transform.Rotate(new Vector3(0f, 0f, squareMagnitude));
	}

	bool fullyTilted() {
		return faceRightParent.transform.rotation.eulerAngles.z >= 45;
	}
	
	class MouthAnimator : Repeater {
		Sprite mouthLeft;
		Sprite mouthRight;
		int sceneFrame = 0;
		
		const int totalFramesInScene = 24;		
		
		// the speed is eight note triplets because of the lilting rhythm of the lyrics
		public MouthAnimator(Sprite mouthLeft, Sprite mouthRight) : base(0.16666666f) {
			this.mouthLeft = mouthLeft;
			this.mouthRight = mouthRight;
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
			if (sceneFrame >= 2 && sceneFrame <= 3) return sayIts();
			if (sceneFrame >= 4 && sceneFrame <= 6) return sayInside();
			if (sceneFrame >= 9 && sceneFrame <= 10) return saySo();
			if (sceneFrame >= 12 && sceneFrame <= 16) return sayUnder();
			if (sceneFrame >= 17 && sceneFrame <= 19) return sayStand();
			
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
			setMouthFrame(15);
			return new List<Sprite>{ mouthLeft, mouthRight };
		}

		private ICollection<Sprite> sayIts() {
			if (sceneFrame == 1)	{
				setMouthFrame(0);
			}
			else nextMouthFrame();
			
			return new List<Sprite>{ mouthLeft, mouthRight };
		}

		private ICollection<Sprite> sayInside() {
			if (sceneFrame == 4)	{
				setMouthFrame(2);
			}
			else if (sceneFrame == 6) {
				setMouthFrame(2);
			}
			else if (sceneFrame == 7) {
				setMouthFrame(4);
			}
			else nextMouthFrame();
			
			return new List<Sprite>{ mouthLeft, mouthRight };
		}

		private ICollection<Sprite> saySo() {
			if (sceneFrame == 9)	{
				setMouthFrame(5);
			}
			else nextMouthFrame();
			return new List<Sprite>{ mouthLeft, mouthRight };
		}
		
		private ICollection<Sprite> sayUnder() {
			if (sceneFrame == 12) {
				setMouthFrame(7);
			}
			else nextMouthFrame();
			return new List<Sprite>{ mouthLeft, mouthRight };
		}
		
		private ICollection<Sprite> sayStand() {
			if (sceneFrame == 17) {
				setMouthFrame(11);
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
