using UnityEngine;

class SpeechBubble {
	Camera camera;
	float leftToRightSwitchOverPosition;

	GameObject speechBubble;
	GameObject speechBubbleLeft;
	GameObject speechBubbleRight;

	public SpeechBubble(GameObjectFactory<string> resourceFactory, Camera camera, float leftToRightSwitchOverPosition) {
		this.camera = camera;
		this.leftToRightSwitchOverPosition = leftToRightSwitchOverPosition;
		speechBubble = resourceFactory.Create(this, "SpeechBubble");
		speechBubbleLeft = resourceFactory.Create(this, "BubbleTailLeft");
		speechBubbleRight = resourceFactory.Create(this, "BubbleTailRight");
		speechBubbleRight.active = false;
		
		var bubblePos = speechBubble.transform.position;
		bubblePos.x = -8f;
		bubblePos.y = 6f;
		speechBubble.transform.position = bubblePos;
		
		var leftTailPos = speechBubbleLeft.transform.position;
		leftTailPos.x = -5.5f;
		leftTailPos.y = 5.0f;
		leftTailPos.z = -0.5f;
		speechBubbleLeft.transform.position = leftTailPos;

		var rightTailPos = speechBubbleRight.transform.position;
		rightTailPos.x = -5.5f;
		rightTailPos.y = 5.0f;
		rightTailPos.z = -0.5f;
		speechBubbleRight.transform.position = rightTailPos;
	}
	
	public bool inTerminalPosition {
		get {
			return speechBubble.transform.position.x >= 2f;
		}
	}
	
	public void Destroy() {
		GameObject.Destroy(speechBubble);
		GameObject.Destroy(speechBubbleLeft);
		GameObject.Destroy(speechBubbleRight);
	}

	public void snapToEnd() {
		var bubblePos = speechBubble.transform.position;
		speechBubble.transform.position = new Vector3(2f, bubblePos.y, bubblePos.z);
		var bubbleRightPos = speechBubbleRight.transform.position;
		speechBubbleRight.transform.position = new Vector3(4.5f, bubbleRightPos.y, bubbleRightPos.z);
	}

	public void chooseTail() {
		if (speechBubble.GetComponent<Sprite>().ScreenCenter().x > leftToRightSwitchOverPosition) {
			speechBubbleLeft.active = false;
			speechBubbleRight.active = true;
		} else {
			speechBubbleLeft.active = true;
			speechBubbleRight.active = false;			
		}
	}

	public void moveToLocation(Vector3 movementDelta) {
		var currentBubblePosition = camera.WorldToScreenPoint(speechBubble.transform.position);
		speechBubble.transform.position = camera.ScreenToWorldPoint(currentBubblePosition + movementDelta);
		var currentBubbleLeftTailPosition = camera.WorldToScreenPoint(speechBubbleLeft.transform.position);
		speechBubbleLeft.transform.position = camera.ScreenToWorldPoint(currentBubbleLeftTailPosition + movementDelta);
		var currentBubbleRightTailPosition = camera.WorldToScreenPoint(speechBubbleRight.transform.position);
		speechBubbleRight.transform.position = camera.ScreenToWorldPoint(currentBubbleRightTailPosition + movementDelta);
	}

	public void setLocationToTouch() {
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
			if (!speechBubble.GetComponent<Sprite>().Contains(Input.GetTouch(0).position)) return;
			var movementDelta = new Vector3(Input.GetTouch(0).deltaPosition.x, 0f, 0f);
			moveToLocation(movementDelta);
		}
	}
	
	public void Update() {
		setLocationToTouch();
		
		if (Application.isEditor && Input.GetMouseButtonUp(0)) {
			moveToLocation(new Vector3(10f, 0f, 0f));
		}
		chooseTail();

	}
}