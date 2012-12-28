using UnityEngine;
using System;

class SceneTwo : Scene {
	public HospitalRoom room { get; private set; }
	
	private Wiggler wiggler;
	private UnityInput input;
	private TouchSensor sensor;
	private bool eyesOpened = false;
	
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
		
		wiggler = new Wiggler(startTime, timeLength, room.cover.GetComponent<Sprite>());
		sensor = new TouchSensor(input);
	}

	public override void Destroy() {
		wiggler.Destroy();
		// Handled by next scene
		//room.Destroy();
	}

	public override void Update () {
		wiggler.Update(Time.time);

		if (room.eyesTotallyOpen && touchedBed(sensor)) {
			eyesOpened = true;
		}

		if (touchedBed(sensor) && !completed) {
			room.openEyes();
			prompt.progress("prod guy");
		}
		
		if (eyesOpened && !completed) {
			room.removeCover();
			room.doubleHeartRate(Time.time);
			prompt.solve(this, "remove covers");
			endScene();
		}
		
		room.Update();
	}

	bool touchedBed(TouchSensor touch) {
		return touch.insideSprite(Camera.main, room.cover.GetComponent<Sprite>(), new[] {TouchPhase.Began});
	}
}