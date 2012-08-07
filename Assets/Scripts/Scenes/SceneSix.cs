using UnityEngine;
using System;

class SceneSix : Scene {
	BigHeadProp bigHeadProp;
	OffsetCamera wrapCam;
	
	HeadScroller firstLeftHeadScroller;
	HeadScroller firstRightHeadScroller;
	
	Vector3 initialHeadPosition;
	
	bool leftHeadTouched, rightHeadTouched;
	
	private UnityInput input;
	
	public SceneSix(SceneManager manager) : base(manager) {
		timeLength = 4.0f;
		bigHeadProp = new BigHeadProp(resourceFactory);
		input = new UnityInput();
	}

	public override void Setup (float startTime) {
		bigHeadProp.Setup();
		wrapCam = new OffsetCamera(new Vector3(0, 200, -10), 2);
		
		initialHeadPosition = bigHeadProp.faceLeftObject.transform.position;
		
		firstLeftHeadScroller = new HeadScroller(bigHeadProp.faceLeftObject, 3.0f);
		firstRightHeadScroller = new HeadScroller(bigHeadProp.faceRightObject, 5.0f);
	}

	public override void Update () {
		for (int i = 0; i < input.touchCount; i++) {
			var touch = input.GetTouch(i);
			leftHeadTouched |= bigHeadProp.faceLeft.Contains(Camera.main, touch.position);
			rightHeadTouched |= bigHeadProp.faceRight.Contains(Camera.main, touch.position);
			leftHeadTouched |= bigHeadProp.faceLeft.Contains(wrapCam.camera, touch.position);
			rightHeadTouched |= bigHeadProp.faceRight.Contains(wrapCam.camera, touch.position);
		}
		
		if (Application.isEditor && input.GetMouseButtonUp(0)) {
			if (leftHeadTouched) rightHeadTouched = true;
			leftHeadTouched = true;
		}
		
		if(!leftHeadTouched) {
			firstLeftHeadScroller.Update();
		}
		
		if(!rightHeadTouched) {
			firstRightHeadScroller.Update();
		}
		
		if (leftHeadTouched && rightHeadTouched) {
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
}
