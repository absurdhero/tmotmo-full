using UnityEngine;
using System;

class SceneFour : Scene {
	HospitalRoom room;
	
	const int MAX_SPLIT = 40;
	
	public SceneFour(SceneManager manager, HospitalRoom room) : base(manager) {
		this.room = room;
	}

	public override void Setup() {
		timeLength = 8.0f;
		room.separateHalves(MAX_SPLIT);
		room.addSpeechBubble();
	}

	public override void Destroy() {
		room.Destroy();
	}

	public override void Update () {
		room.Update();

		setBubbleLocationToTouch();
		
		if (Application.isEditor && Input.GetMouseButtonUp(0)) {
			moveBubbleUnderFinger(new Vector3(40f, 0f, 0f));
		}
		room.chooseBubbleTail();
	}
	
	private void moveBubbleUnderFinger(Vector3 movementDelta) {
		room.speechBubble.transform.position = Camera.main.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(room.speechBubble.transform.position) + movementDelta);
		room.speechBubbleLeft.transform.position = Camera.main.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(room.speechBubbleLeft.transform.position) + movementDelta);
		room.speechBubbleRight.transform.position = Camera.main.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(room.speechBubbleRight.transform.position) + movementDelta);
	}
	
	private void setBubbleLocationToTouch() {
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
			if (!room.speechBubble.GetComponent<Sprite>().Contains(Input.GetTouch(0).position)) return;
			var movementDelta = new Vector3(Input.GetTouch(0).deltaPosition.x, 0f, 0f);
			moveBubbleUnderFinger(movementDelta);
		}
	
	}
}
