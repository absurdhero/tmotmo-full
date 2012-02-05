using UnityEngine;
using System;

class SceneTwo : Scene {
	public HospitalRoom room { get; private set; }
	
	public SceneTwo(SceneManager manager) : base(manager) {
	}

	public override void Setup() {
		timeLength = 8.0f;
		room = new HospitalRoom();
		room.addHeartRate();
		room.addFootboard();
		room.addCover();
		room.addPerson();
	}

	public override void Destroy() {
		room.Destroy();
	}

	public override void Update () {
		room.Update();
		
		bool touched = false;
		for (int i = 0; i < Input.touchCount; i++) {
			var touch = Input.GetTouch(i);
			touched |= room.cover.GetComponent<Sprite>().Contains(touch.position);
		}
		
		if (Input.GetMouseButtonUp(0)) {
			var pos = Input.mousePosition;
			touched |= room.cover.GetComponent<Sprite>().Contains(pos);
		}
	
		if (touched) {
			room.openEyes();
		}
		
		if (room.eyesTotallyOpen) {
			room.removeCover();
		}
	}
	
	
}