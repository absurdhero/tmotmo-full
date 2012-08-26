using UnityEngine;

class SpeechBubble {
	Camera camera;
	float leftToRightSwitchOverPosition;

	GameObject speechBubble;
	GameObject speechBubbleLeft;
	GameObject speechBubbleRight;
	
	Dragger dragger;
	UnityInput input;

	public SpeechBubble(GameObjectFactory<string> resourceFactory, Camera camera, float leftToRightSwitchOverPosition) {
		this.camera = camera;
		this.leftToRightSwitchOverPosition = leftToRightSwitchOverPosition;
		speechBubble = resourceFactory.Create(this, "SpeechBubble");
		speechBubbleLeft = resourceFactory.Create(this, "BubbleTailLeft");
		speechBubbleRight = resourceFactory.Create(this, "BubbleTailRight");
		speechBubbleRight.active = false;
		
		speechBubble.GetComponent<Sprite>().setWorldPosition(-80f, 60f, -1f);
		speechBubbleLeft.GetComponent<Sprite>().setWorldPosition(-55f, 50f, -5f);
		speechBubbleRight.GetComponent<Sprite>().setWorldPosition(-55f, 50f, -5f);
		
		input = new UnityInput();
		dragger = new Dragger(input, speechBubble.GetComponent<Sprite>());
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
		GameObject.Destroy(speechBubble);
		GameObject.Destroy(speechBubbleLeft);
		GameObject.Destroy(speechBubbleRight);
	}

	public void snapToEnd() {
		var bubblePos = speechBubble.transform.position;
		speechBubble.transform.position = new Vector3(terminalPosition, bubblePos.y, bubblePos.z);
		var bubbleRightPos = speechBubbleRight.transform.position;
		float terminalTailPosition = camera.orthographicSize / 2.22f;
		speechBubbleRight.transform.position = new Vector3(terminalTailPosition, bubbleRightPos.y, bubbleRightPos.z); 
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

	public GameObject centerPivot() {
		GameObject pivot = speechBubble.GetComponent<Sprite>().createPivotOnCenter();
		speechBubbleLeft.transform.parent = pivot.transform;
		speechBubbleRight.transform.parent = pivot.transform;
		return pivot;
	}
	
	public void Update() {
		moveToLocation(dragger.movementIfDragged());
		
		if (Application.isEditor && input.GetMouseButtonUp(0)) {
			moveToLocation(new Vector3(40f, 0f, 0f));
		}
		chooseTail();
	}
}
