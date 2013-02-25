using UnityEngine;
using System;
using System.Collections.Generic;

class SceneSix : Scene {
	BigHeadProp bigHeadProp;
	BigMouthAnimator bigMouthAnimator;
	OffsetCamera wrapCam;
	
	HeadScroller firstLeftHeadScroller;
	HeadScroller firstRightHeadScroller;
	
	Vector3 initialHeadPosition;
	
	bool leftHeadTouched, rightHeadTouched;
	
	private UnityInput input;
	
	public SceneSix(SceneManager manager) : base(manager) {
		timeLength = 4.0f;
		permitUnloadResources = false;
		bigHeadProp = new BigHeadProp(resourceFactory);
		input = new UnityInput();
	}

	public override void Setup (float startTime) {
		bigHeadProp.Setup();
		wrapCam = new OffsetCamera(new Vector3(0, 200, -10), 2);
		
		initialHeadPosition = bigHeadProp.faceLeftObject.transform.position;
		
		firstLeftHeadScroller = new HeadScroller(bigHeadProp.faceLeftObject, 3.0f);
		firstRightHeadScroller = new HeadScroller(bigHeadProp.faceRightObject, 5.0f);
		
		var mouthLeftGameObject = resourceFactory.Create("SceneSix/MouthLeft-KeepMeInPlace");
		var mouthLeft = mouthLeftGameObject.GetComponent<Sprite>();
		mouthLeft.setWorldPosition(-29.5f, -56f, -5f);
		mouthLeftGameObject.transform.parent = bigHeadProp.faceLeft.transform;
		
		var mouthRightGameObject = resourceFactory.Create("SceneSix/MouthRight-KeepMeInPlace");
		var mouthRight = mouthRightGameObject.GetComponent<Sprite>();
		mouthRight.setWorldPosition(10f, -56f, -5f);
		mouthRightGameObject.transform.parent = bigHeadProp.faceRight.transform;
		
		bigMouthAnimator = new BigMouthAnimator(startTime, mouthLeft, mouthRight);
	}

	public override void Update () {
		bigMouthAnimator.Update(Time.time);

		for (int i = 0; i < input.touchCount; i++) {
			var touch = input.GetTouch(i);
			leftHeadTouched |= bigHeadProp.faceLeft.Contains(Camera.main, touch.position);
			rightHeadTouched |= bigHeadProp.faceRight.Contains(Camera.main, touch.position);
			leftHeadTouched |= bigHeadProp.faceLeft.Contains(wrapCam.camera, touch.position);
			rightHeadTouched |= bigHeadProp.faceRight.Contains(wrapCam.camera, touch.position);
		}
		
		if(!leftHeadTouched) {
			firstLeftHeadScroller.Update();
		}
		
		if(!rightHeadTouched) {
			firstRightHeadScroller.Update();
		}
		
		if (leftHeadTouched && rightHeadTouched) {
			messagePromptCoordinator.solve(this, "stop both halves");
			endScene();

			firstLeftHeadScroller.gotoTargetBeforeEnd(initialHeadPosition.y, sceneManager.timeLeftInCurrentLoop());
			firstRightHeadScroller.gotoTargetBeforeEnd(initialHeadPosition.y, sceneManager.timeLeftInCurrentLoop());
		}
	}

	public override void Destroy () {
		bigHeadProp.Destroy();
		wrapCam.Destroy();
	}
	
	class HeadScroller {
		GameObject head;
		float scrollTime;
		
		Sprite headSprite { get { return head.GetComponent<Sprite>(); } }
		
		public float currentVerticalPosition { get { return head.transform.position.y; } }
		
		public HeadScroller(GameObject head, float scrollTime) {
			this.head = head;
			this.scrollTime = scrollTime;
		}
		
		public void Update() {
			moveTo(verticalDestination(pixelsToMove));
		}

		float verticalDestination(float speed)
		{
			Vector3 headPosition = head.transform.position;
			headPosition.y += speed;
			if (verticalPosition > Camera.main.pixelHeight) {
				float positionAtBottomOfScreen = 0;
				headPosition.y = Camera.main.ScreenToWorldPoint(new Vector3(0, positionAtBottomOfScreen, 0)).y;
			}
			return headPosition.y;
		}
		
		public void gotoTargetBeforeEnd(float targetPosition, float timeLeft) {
			Vector3 headPosition = head.transform.position;
			//float movement = Time.deltaTime * cameraWorldHeight;
			
			if (timeLeft < 0.1f) {
				moveTo(targetPosition);
				return;
			}
			
			float speed = Time.deltaTime * ((targetPosition - currentVerticalPosition) / timeLeft);
			float movement = verticalDestination(Math.Max(speed, pixelsToMove));

			if (headPosition.y <= targetPosition && movement >= targetPosition) return;

			moveTo(movement);
		}
		
		public void moveTo(float verticalPosition) {
			Vector3 headPosition = head.transform.position;
			headPosition.y = verticalPosition;
			head.transform.position = headPosition;
		}

		float pixelsToMove
		{
			get {
				if (scrollTime == 0f) return 0f;
				return Time.deltaTime * cameraWorldHeight / scrollTime;
			}
		}
		
		public float cameraWorldHeight {
			get {
				return Camera.main.ScreenToWorldPoint(new Vector3(0f, Camera.main.pixelHeight, 0f)).y - 
					Camera.main.ScreenToWorldPoint(Vector3.zero).y;
			}
		}
		
		public float verticalPosition {
			get {
				return headSprite.getScreenPosition().y + pixelsToMove;
			}
		}
		
		public int height {
			get {
				return head.GetComponent<Sprite>().PixelHeight();
			}
		}
	}
	
	class BigMouthAnimator : Repeater {
		Sprite mouthLeft;
		Sprite mouthRight;
		int sceneFrame = 0;
		
		const int totalFramesInScene = 16;		
		
		public BigMouthAnimator(float startTime, Sprite mouthLeft, Sprite mouthRight) : base(0.25f, 0, startTime) {
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
			if (sceneFrame >= 2 && sceneFrame <= 2) return sayTake();
			if (sceneFrame >= 4 && sceneFrame <= 9) return sayMe();
			if (sceneFrame >= 10 && sceneFrame <= 11) return sayIn();
			if (sceneFrame >= 12 && sceneFrame <= 15) return sayPlace();
			
			return new List<Sprite>();
		}
		
		private void incrementFrame() {
			sceneFrame = (sceneFrame + 1) % totalFramesInScene;
		}
		

		private ICollection<Sprite> initialMouthFrame()
		{
			setMouthFrame(0);
			return new List<Sprite>{ mouthLeft, mouthRight };
		}

		private ICollection<Sprite> sayTake() {
			if (sceneFrame == 2)	{
				setMouthFrame(1);
			}
			else nextMouthFrame();
			
			return new List<Sprite>{ mouthLeft, mouthRight };
		}

		private ICollection<Sprite> sayMe() {
			if (sceneFrame == 4)	{
				setMouthFrame(4);
			}
			else if (sceneFrame >= 9) {
				setMouthFrame(4); //close the mouth again
			}
			else if (sceneFrame > 4) {
				setMouthFrame(5); // open mouth
			}
			return new List<Sprite>{ mouthLeft, mouthRight };
		}
		
		private ICollection<Sprite> sayIn() {
			if (sceneFrame == 10) {
				setMouthFrame(6);
			}
			else nextMouthFrame();
			return new List<Sprite>{ mouthLeft, mouthRight };
		}

		private ICollection<Sprite> sayPlace() {
			if (sceneFrame == 12) {
				setMouthFrame(8);
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
