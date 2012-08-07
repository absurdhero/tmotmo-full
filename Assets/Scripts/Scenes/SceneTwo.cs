using UnityEngine;
using System;

class SceneTwo : Scene {
	public HospitalRoom room { get; private set; }
	
	private UnityInput input;
	
	public SceneTwo(SceneManager manager) : base(manager) {
		timeLength = 8.0f;
		input = new UnityInput();
		room = new HospitalRoom(resourceFactory, camera);
	}

	public override void Setup(float startTime) {
		room.createBackground();
		room.addZzz();
		room.addHeartRate(startTime);
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

		if (touch.insideSprite(Camera.main, room.cover.GetComponent<Sprite>())) {
			room.openEyes();
		}

		if (room.eyesTotallyOpen && !completed) {
			room.removeCover();
			room.doubleHeartRate(Time.time);
			endScene();
		}
		
		room.Update();
	}
	
	
}