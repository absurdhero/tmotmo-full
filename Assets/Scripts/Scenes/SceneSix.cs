using UnityEngine;
using System;

class SceneSix : Scene {
	BigHeadProp bigHeadProp;
	BigHeadProp otherBigHeadProp;
	
	HeadScroller firstLeftHeadScroller;
	HeadScroller secondLeftHeadScroller;
	HeadScroller firstRightHeadScroller;
	HeadScroller secondRightHeadScroller;
	
	public SceneSix(SceneManager manager) : base(manager) {
		bigHeadProp = new BigHeadProp(resourceFactory);
		otherBigHeadProp = new BigHeadProp(resourceFactory);
	}

	public override void Setup () {
		timeLength = 4.0f;
		bigHeadProp.Setup();
		otherBigHeadProp.Setup();
		
		firstLeftHeadScroller = new HeadScroller(bigHeadProp.faceLeftObject, 3.0f);
		secondLeftHeadScroller = new HeadScroller(otherBigHeadProp.faceLeftObject, 3.0f);
		secondLeftHeadScroller.moveHeadDownOneScreenLength();

		firstRightHeadScroller = new HeadScroller(bigHeadProp.faceRightObject, 5.0f);
		secondRightHeadScroller = new HeadScroller(otherBigHeadProp.faceRightObject, 5.0f);
		secondRightHeadScroller.moveHeadDownOneScreenLength();
	}

	public override void Update () {
		firstLeftHeadScroller.Update(Time.time);
		secondLeftHeadScroller.Update(Time.time);
		firstRightHeadScroller.Update(Time.time);
		secondRightHeadScroller.Update(Time.time);
	}

	public override void Destroy () {
		bigHeadProp.Destroy();
		otherBigHeadProp.Destroy();
	}
	
	class HeadScroller {
		GameObject head;
		float loopTime;
		
		public HeadScroller(GameObject head, float loopTime) {
			this.head = head;
			this.loopTime = loopTime;
		}
		public void Update(float time) {
			Vector3 headPosition = head.transform.position;
			headPosition.y += pixelsToMove;
			if (verticalPosition > Camera.main.pixelHeight) {
				float positionAtBottomOfScreen = -Camera.main.pixelHeight;
				headPosition.y = Camera.main.ScreenToWorldPoint(new Vector3(0, positionAtBottomOfScreen, 0)).y;
			}
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
				if (loopTime == 0f) return 0f;
				return Time.deltaTime * cameraWorldHeight / loopTime;
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
