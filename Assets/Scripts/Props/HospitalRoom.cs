using UnityEngine;
using System;
using System.Collections.Generic;

public class HospitalRoom {
	GameObjectFactory<string> resourceFactory;

	FullScreenQuad room;
	
	int guyCenterOffset = 6;
	public Sprite guyLeft, guyRight;
	Vector3 guyLeftInitialPosition;
	Vector3 guyRightInitialPosition;
	public float guySplitDistance { get; private set; }
	public float guyCenterPoint {
		get {
			return camera.pixelWidth / 2.0f + (float) guyCenterOffset;
		}
	}
	
	public GameObject clipBoard;
	public GameObject heartRate;
	GameObject footBoard;
	GameObject eyes;
	Camera camera;
	Cycler heartRateCycler;

	// the state of hospital room interactions is mutable so this is generated once and re-used
	public Dictionary<GameObject, ActionResponsePair[]> interactions {
		get {
				return new Dictionary<GameObject, ActionResponsePair[]> {
					{clipBoard, new [] {new ActionResponsePair("look at chart", new [] {"even the doctors don't understand the test results"})}},
					{heartRate, new [] {new ActionResponsePair("look at monitor", new []{"things are stable, for now"})}},
				};
		}
	}

	public HospitalRoom(GameObjectFactory<string> resourceFactory, Camera camera) {
		this.resourceFactory = resourceFactory;
		this.camera = camera;
	}

	public void createBackground() {
		room = FullScreenQuad.create(this, "hospital_bg");
	}
	
	private void DestroyIfNotNull(GameObject gobj) {
		if (gobj != null) GameObject.Destroy(gobj);
	}

	private void DestroyIfNotNull(Sprite sprite) {
		if (sprite != null) Sprite.Destroy(sprite);
	}

	public void Destroy() {
		DestroyIfNotNull(room);
		DestroyIfNotNull(guyLeft);
		DestroyIfNotNull(guyRight);
		DestroyIfNotNull(eyes);
		DestroyIfNotNull(footBoard);
		DestroyIfNotNull(clipBoard);
		DestroyIfNotNull(heartRate);
		
		heartRateCycler = null;
	}
	
	public void addPerson() {
		guyLeft = resourceFactory.Create(this, "GuyLeft").GetComponent<Sprite>();
		guyRight = resourceFactory.Create(this, "GuyRight").GetComponent<Sprite>();
		
		guyLeft.setScreenPosition((int) camera.pixelWidth / 2 - guyLeft.PixelWidth() + guyCenterOffset,
									 (int) camera.pixelHeight / 2 - guyLeft.PixelHeight() / 2);
		guyRight.setScreenPosition((int) camera.pixelWidth / 2 + guyCenterOffset,
									  (int) camera.pixelHeight / 2 - guyRight.PixelHeight() / 2);
		guyLeftInitialPosition = guyLeft.getScreenPosition();
		guyRightInitialPosition = guyRight.getScreenPosition();
		
		eyes = resourceFactory.Create(this, "EyesOpening");
		eyes.GetComponent<Sprite>().setWorldPosition(-5.5f, 36.5f, -1f);
	}
	
	public void openEyes() {
		if (eyes.GetComponent<Sprite>().LastTexture()) {
			eyes.SetActive(false);
			return;
		}
		eyes.GetComponent<Sprite>().DrawNextFrame();
	}
	
	public bool eyesTotallyOpen {
		get { return !eyes.activeSelf; }
	}
	
	public void addFootboard() {
		addClipBoard();

		footBoard = resourceFactory.Create(this, "Footboard");
		footBoard.GetComponent<Sprite>().setWorldPosition(-44.875f, -92f, -1f);
	}
	
	private void addClipBoard() {
		clipBoard = resourceFactory.Create(this, "ClipBoard");
		clipBoard.GetComponent<Sprite>().setWorldPosition(-12f, -80f, -1f);
	}
	
	public void addHeartRate(float startTime) {
		heartRate = resourceFactory.Create(this, "HeartRate");
		heartRate.GetComponent<Sprite>().setWorldPosition(-115f, 17f, -1f);
		
		// Repeat 7 frame animation every 2 seconds
		// That's 30bpm which might mean he is dying!
		heartRateCycler = new Cycler(1f/7f*2f, 0, startTime);
		heartRateCycler.AddSprite(heartRate);
	}
	
	public void doubleHeartRate(float startTime) {
		heartRateCycler = new Cycler(1f/7f, 0, startTime);
		heartRateCycler.AddSprite(heartRate);
	}
	
	
	public Rect guyBoundsPlus(float inflation) {
		var leftRect = guyLeft.ScreenRect();
		var rightRect = guyRight.ScreenRect();
		var rect = new Rect(leftRect.x - inflation,
						leftRect.y - inflation,
						leftRect.width + rightRect.width + inflation * 2f,
					    leftRect.height + rightRect.height + inflation * 2f);
	
		return rect;
	}
	
	public bool guyContains(Vector2 position) {
		return guyLeft.Contains(Camera.main, position)
			|| guyRight.Contains(Camera.main, position);
	}
	
	public void separateHalves(float distance) {
		guySplitDistance = distance;
	}

	private void setGuySplit() {
		guyLeft.setScreenPosition((int) (guyLeftInitialPosition.x - guySplitDistance), (int) guyLeftInitialPosition.y);
		guyRight.setScreenPosition((int) (guyRightInitialPosition.x + guySplitDistance), (int) guyRightInitialPosition.y);
	}
	
	public void addSplitLine() {
		separateHalves(1);
	}

	public void Update() {		
		if(heartRateCycler != null) {
			heartRateCycler.Update(Time.time);
		}
		
		setGuySplit();
	}

	public void hintWhenTouched(Action<GameObject> onCompleted, MessagePromptCoordinator messagePromptCoordinator, TouchSensor touch) {
		messagePromptCoordinator.hintWhenTouched(onCompleted, touch, Time.time, interactions);
	}
	
}