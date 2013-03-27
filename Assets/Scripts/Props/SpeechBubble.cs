using UnityEngine;

class SpeechBubble {
	Camera camera;
	float leftToRightSwitchOverPosition;

	Sprite speechBubble;
	Sprite speechBubbleLeft;
	Sprite speechBubbleRight;
	
	Dragger dragger;
	UnityInput input;

	public SpeechBubble(GameObjectFactory<string> resourceFactory, Camera camera, float leftToRightSwitchOverPosition) {
		this.camera = camera;
		this.leftToRightSwitchOverPosition = leftToRightSwitchOverPosition;
		speechBubble = resourceFactory.Create(this, "SpeechBubble").GetComponent<Sprite>();
		speechBubbleLeft = resourceFactory.Create(this, "BubbleTailLeft").GetComponent<Sprite>();
		speechBubbleRight = resourceFactory.Create(this, "BubbleTailRight").GetComponent<Sprite>();
		speechBubbleRight.visible(false);
		
		speechBubble.setWorldPosition(-80f, 60f, -1f);
		speechBubbleLeft.setWorldPosition(-55f, 50f, -5f);
		speechBubbleRight.setWorldPosition(-55f, 50f, -5f);
		speechBubbleRight.transform.Rotate(Vector3.forward * 5);
		
		input = new UnityInput();
		dragger = new Dragger(input, speechBubble);
	}
	
	private float terminalPosition {
		get { return camera.orthographicSize / 5f; }
	}
	
	public bool inTerminalPosition {
		get {
			return speechBubble.transform.position.x >= terminalPosition;
		}
	}
	
	public void Destroy() {
		Sprite.Destroy(speechBubble);
		Sprite.Destroy(speechBubbleLeft);
		Sprite.Destroy(speechBubbleRight);
	}

	public void snapToEnd() {
		var bubblePos = speechBubble.transform.position;
		speechBubble.transform.position = new Vector3(terminalPosition, bubblePos.y, bubblePos.z);
		var bubbleRightPos = speechBubbleRight.transform.position;
		float terminalTailPosition = camera.orthographicSize / 2.22f;
		speechBubbleRight.transform.position = new Vector3(terminalTailPosition, bubbleRightPos.y, bubbleRightPos.z); 
		chooseTail();
	}

	public bool onRightSide() {
		return speechBubble.ScreenCenter().x > leftToRightSwitchOverPosition;
	}

	private void chooseTail() {
		if (onRightSide()) {
			speechBubbleLeft.visible(false);
			speechBubbleRight.visible(true);
		} else {
			speechBubbleLeft.visible(true);
			speechBubbleRight.visible(false);
		}
	}

	private void moveToLocation(Vector3 movementDelta) {
		var currentBubblePosition = camera.WorldToScreenPoint(speechBubble.transform.position);

		// don't move off the left side of the screen
		if (movementDelta.x < 0 && (currentBubblePosition.x + movementDelta.x) <= 0)
			return;

		speechBubble.transform.position = camera.ScreenToWorldPoint(currentBubblePosition + movementDelta);
		var currentBubbleLeftTailPosition = camera.WorldToScreenPoint(speechBubbleLeft.transform.position);
		speechBubbleLeft.transform.position = camera.ScreenToWorldPoint(currentBubbleLeftTailPosition + movementDelta);
		var currentBubbleRightTailPosition = camera.WorldToScreenPoint(speechBubbleRight.transform.position);
		speechBubbleRight.transform.position = camera.ScreenToWorldPoint(currentBubbleRightTailPosition + movementDelta);
	}

	public GameObject centerPivot() {
		GameObject pivot = speechBubble.createPivotOnCenter();
		speechBubbleLeft.transform.parent = pivot.transform;
		speechBubbleRight.transform.parent = pivot.transform;
		return pivot;
	}
	
	public void Update() {
		if(inTerminalPosition)
			return;

		moveToLocation(dragger.movementIfDragged().x * Vector3.right);
		chooseTail();
	}

	public bool hasMoved() {
		return dragger.movementIfDragged().x != 0;
	}
}
