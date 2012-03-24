using UnityEngine;
using System;

class SceneEight : Scene {
	BigHeadProp bigHeadProp;
	GameObject faceRightParent;
	
	Vector3 previousMousePosition;

	public SceneEight(SceneManager manager) : base(manager) {
		bigHeadProp = new BigHeadProp(resourceFactory);
	}

	public override void Setup () {
		timeLength = 4.0f;
		
		bigHeadProp.Setup();
		faceRightParent = bigHeadProp.faceRight.createPivotOnTopLeftCorner();
	}

	public override void Update () {
		setLocationToTouch();
	}

	public override void Destroy () {
		GameObject.Destroy(faceRightParent);
		bigHeadProp.Destroy();
	}

	public void moveToLocation(Vector3 movementDelta) {
		if (faceRightParent.transform.rotation.eulerAngles.z >= 45) return;

		float squareMagnitude = movementDelta.x + movementDelta.y;
		if (squareMagnitude < 0) return;
		
		faceRightParent.transform.Rotate(new Vector3(0f, 0f, squareMagnitude));
	}

	public void setLocationToTouch() {
		Vector3 movementDelta = Vector3.zero;
		
		if (Application.isEditor && Input.GetMouseButton(0)) {
			movementDelta = Input.mousePosition - previousMousePosition;
		}
		previousMousePosition = Input.mousePosition;
		
		if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) {
			if (!bigHeadProp.faceRight.Contains(Input.GetTouch(0).position)) return;
			movementDelta = new Vector3(Input.GetTouch(0).deltaPosition.x, Input.GetTouch(0).deltaPosition.y, 0f);
		}
		moveToLocation(movementDelta);
	}
}
