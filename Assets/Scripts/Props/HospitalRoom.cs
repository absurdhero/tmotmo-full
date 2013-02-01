using UnityEngine;
using System;
using System.Collections.Generic;

public class HospitalRoom {
	GameObjectFactory<string> resourceFactory;

	GameObject room;
	
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

	public GameObject cover;
	
	GameObject zzz;
	ZzzAnimator zzzAnimator;
	GameObject footBoard;
	GameObject clipBoard;
	GameObject eyes;
	Camera camera;
	GameObject heartRate;
	Cycler heartRateCycler;

	Dictionary<GameObject, List<String[]>> interactions;

	public HospitalRoom(GameObjectFactory<string> resourceFactory, Camera camera) {
		this.resourceFactory = resourceFactory;
		this.camera = camera;
	}

	public void createBackground() {
		room = resourceFactory.Create(this, "HospitalRoomQuad");
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
		DestroyIfNotNull(cover);
		DestroyIfNotNull(footBoard);
		DestroyIfNotNull(clipBoard);
		DestroyIfNotNull(heartRate);
		
		heartRateCycler = null;
		removeZzz();
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
	
	public void addZzz() {
		zzz = resourceFactory.Create(this, "Zzz");
		zzz.GetComponent<Sprite>().setWorldPosition(50f, 60f, -1f);
		zzzAnimator = new ZzzAnimator(zzz);
	}
	
	public void removeZzz() {
		DestroyIfNotNull(zzz);
		zzzAnimator = null;
	}
	
	public void openEyes() {
		if (eyes.GetComponent<Sprite>().LastTexture()) {
			eyes.active = false;
			return;
		}
		eyes.GetComponent<Sprite>().DrawNextFrame();
	}
	
	public bool eyesTotallyOpen {
		get { return !eyes.active; }
	}
	
	public void addCover() {
		if (cover != null) {
			cover.active = true;
			return;
		}
		cover = resourceFactory.Create(this, "Cover");
		var coverSprite = cover.GetComponent<Sprite>();
		coverSprite.setCenterToViewportCoord(0.515f, 0.34f);
		
	}
	
	public void removeCover() {
		cover.active = false;
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
		
		if (zzzAnimator != null && !eyesTotallyOpen) {
			zzzAnimator.Update(Time.time);
		}
	}

	public void hintWhenTouched(Action<GameObject> onCompleted, Prompt prompt, TouchSensor touch) {
		if (interactions == null) {
			interactions = new Dictionary<GameObject, List<String[]>> {
				{clipBoard, new List<String[]> {new String[] {"look at chart", "even the doctors don't understand the test results"}}},
				{zzz, new List<String[]> {new String[] {"catch z", "that's not going to wake him up"}}},
				{heartRate, new List<String[]> {new String[] {"look at monitor", "things are stable, for now"}}},
				{cover, new List<String[]> {new String[] {"prod him", "He doesn't want to wake up"},
						new String[] {"prod him again", "OK"},
						new String[] {"prod him again", "OK"}}},
			};
		}
		prompt.hintWhenTouched(onCompleted, touch, interactions);
	}
	
	public bool touchedBed(TouchSensor touch) {
		return touch.insideSprite(Camera.main, cover.GetComponent<Sprite>(), new[] {TouchPhase.Began});
	}

	class ZzzAnimator : Repeater {
		GameObject zzz;
		Vector3 initialPosition;
		int frame = 0;
		
		const int totalFrames = 4;
		
		
		public ZzzAnimator(GameObject zzz) : base(0.5f) {
			this.zzz = zzz;
			initialPosition = zzz.transform.position;
		}
		
		public override void OnTick() {
			frame = (frame + 1) % totalFrames;
			zzz.transform.position = initialPosition + new Vector3(frame * 1, frame * 1, 0);
		}
		
		public void Reset() {
			zzz.transform.position = initialPosition;
		}
	}
}