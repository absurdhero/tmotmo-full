using UnityEngine;
using System;

class SceneSix : Scene {
	BigHeadProp bigHeadProp;
	BigHeadProp otherBigHeadProp;
	
	HeadScroller firstLeftHeadScroller;
	HeadScroller secondLeftHeadScroller;
	HeadScroller firstRightHeadScroller;
	HeadScroller secondRightHeadScroller;
	
	Vector3 initialHeadPosition;
	
	bool leftHeadTouched, rightHeadTouched;
	
	HeadScroller lowerLeftHead, upperLeftHead;
	HeadScroller lowerRightHead, upperRightHead;

	private UnityInput input;
	
	public SceneSix(SceneManager manager) : base(manager) {
		bigHeadProp = new BigHeadProp(resourceFactory);
		otherBigHeadProp = new BigHeadProp(resourceFactory);
		input = new UnityInput();
	}

	public override void Setup () {
		timeLength = 4.0f;
		bigHeadProp.Setup();
		otherBigHeadProp.Setup();
		
		initialHeadPosition = bigHeadProp.faceLeftObject.transform.position;
		
		firstLeftHeadScroller = new HeadScroller(bigHeadProp.faceLeftObject, 3.0f);
		secondLeftHeadScroller = new HeadScroller(otherBigHeadProp.faceLeftObject, 3.0f);
		secondLeftHeadScroller.moveHeadDownOneScreenLength();

		firstRightHeadScroller = new HeadScroller(bigHeadProp.faceRightObject, 5.0f);
		secondRightHeadScroller = new HeadScroller(otherBigHeadProp.faceRightObject, 5.0f);
		secondRightHeadScroller.moveHeadDownOneScreenLength();
	}

	public override void Update () {
		for (int i = 0; i < input.touchCount; i++) {
			var touch = input.GetTouch(i);
			leftHeadTouched |= bigHeadProp.faceLeft.Contains(touch.position);
			leftHeadTouched |= otherBigHeadProp.faceLeft.Contains(touch.position);
			rightHeadTouched |= bigHeadProp.faceRight.Contains(touch.position);
			rightHeadTouched |= otherBigHeadProp.faceRight.Contains(touch.position);
		}
		
		if (Application.isEditor && input.GetMouseButtonUp(0)) {
			if (leftHeadTouched) rightHeadTouched = true;
			leftHeadTouched = true;
		}
		
		if(!leftHeadTouched) {
			firstLeftHeadScroller.Update();
			secondLeftHeadScroller.Update();			
		}
		
		if(!rightHeadTouched) {
			firstRightHeadScroller.Update();
			secondRightHeadScroller.Update();
		}
		
		if (leftHeadTouched && rightHeadTouched) {
			if(!completed) {
				if (firstLeftHeadScroller.currentVerticalPosition < secondLeftHeadScroller.currentVerticalPosition) {
					lowerLeftHead = firstLeftHeadScroller;
					upperLeftHead = secondLeftHeadScroller;
				} else {				
					lowerLeftHead = secondLeftHeadScroller;
					upperLeftHead = firstLeftHeadScroller;
				}
				if (firstRightHeadScroller.currentVerticalPosition < secondRightHeadScroller.currentVerticalPosition) {
					lowerRightHead = firstRightHeadScroller;
					upperRightHead = secondRightHeadScroller;
				} else {				
					lowerRightHead = secondRightHeadScroller;
					upperRightHead = firstRightHeadScroller;
				}
			}
			
			endScene();

			lowerLeftHead.gotoTargetBeforeEnd(initialHeadPosition.y, sceneManager.timeLeftInCurrentLoop());
			upperLeftHead.gotoTargetBeforeEnd(initialHeadPosition.y - upperLeftHead.cameraWorldHeight, sceneManager.timeLeftInCurrentLoop());
			lowerRightHead.gotoTargetBeforeEnd(initialHeadPosition.y, sceneManager.timeLeftInCurrentLoop());
			upperRightHead.gotoTargetBeforeEnd(initialHeadPosition.y - upperRightHead.cameraWorldHeight, sceneManager.timeLeftInCurrentLoop());
		}
	}

	public override void Destroy () {
		bigHeadProp.Destroy();
		otherBigHeadProp.Destroy();
	}
	
	class HeadScroller {
		GameObject head;
		float scrollTime;
		
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
				float positionAtBottomOfScreen = -Camera.main.pixelHeight;
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
		
		public void moveHeadDownOneScreenLength() {
			Vector3 pos = head.transform.position;
			pos.y -= cameraWorldHeight;
			head.transform.position = pos;
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
					Vector3 headPosition = head.transform.position;
					headPosition.y += pixelsToMove;
					return Camera.main.WorldToScreenPoint(headPosition).y;
			}
		}
		
		public int height {
			get {
				return head.GetComponent<Sprite>().PixelHeight();
			}
		}
	}
}
