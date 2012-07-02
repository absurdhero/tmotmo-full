using UnityEngine;
using System;

class SceneTwo : Scene {
	public HospitalRoom room { get; private set; }
	
	private UnityInput input;
	
	public SceneTwo(SceneManager manager) : base(manager) {
		input = new UnityInput();
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
		var touch = new TouchSensor(input);

		if (touch.insideSprite(room.cover.GetComponent<Sprite>())) {
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