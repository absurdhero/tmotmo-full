using UnityEngine;
using System;

class SceneEight : Scene {
	BigHeadProp bigHeadProp;
	GameObject faceRightParent;
	
	Vector3 previousMousePosition;

	private UnityInput input;

	public SceneEight(SceneManager manager) : base(manager) {
		bigHeadProp = new BigHeadProp(resourceFactory);
		input = new UnityInput();
	}

	public override void Setup () {
		timeLength = 4.0f;
		
		bigHeadProp.Setup();
		faceRightParent = bigHeadProp.faceRight.createPivotOnTopLeftCorner();
	}

	public override void Update () {
		if(completed) return;
		setLocationToTouch();
	}

	public override void Destroy () {
		GameObject.Destroy(faceRightParent);
		bigHeadProp.Destroy();
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
		if (faceRightParent.transform.rotation.eulerAngles.z >= 45) {
			endScene();
			return;
		}

		float squareMagnitude = movementDelta.x + movementDelta.y;
		if (squareMagnitude < 0) return;
		
		faceRightParent.transform.Rotate(new Vector3(0f, 0f, squareMagnitude));
	}
}
