using UnityEngine;
using System;

class SceneTwo : Scene {
	public HospitalRoom room { get; private set; }
	
	public SceneTwo(SceneManager manager) : base(manager) {
	}

	public override void Setup() {
		timeLength = 8.0f;
		room = new HospitalRoom(resourceFactory, camera);
		room.addZzz();
		room.addHeartRate();
		room.addFootboard();
		room.addCover();
		room.addPerson();
	}

	public override void Destroy() {
		// Handled by next scene
		//room.Destroy();
	}

	public override void Update () {		
		bool touched = false;
		for (int i = 0; i < Input.touchCount; i++) {
			var touch = Input.GetTouch(i);
			if (touch.phase == TouchPhase.Began) {
				touched |= room.cover.GetComponent<Sprite>().Contains(touch.position);
			}
		}

		if (Application.isEditor && Input.GetMouseButtonUp(0)) {
			var pos = Input.mousePosition;
			touched |= room.cover.GetComponent<Sprite>().Contains(pos);
		}
	
		if (touched) {
			room.openEyes();
		}
		
		if (room.eyesTotallyOpen && !completed) {
			room.removeCover();
			room.doubleHeartRate();
			endScene();
		}
		
		room.Update();
	}
	
	
}