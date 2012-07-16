using UnityEngine;

public class SceneTwelve : Scene {
	GameObject background, bottomDither;
	
	Sprite topHand, bottomPalm, bottomWrist;
	
	Sprite bottomHandFingers, indexOpen, indexClosed, littleFingerOpen,
		   middleFingerOpen, otherFingerOpen, thumbOpen;
	
	Sprite[] openFingers;
	int nextFinger = 0;
	
	ArmSwinger armSwinger;

	TouchSensor touchSensor;
	
	bool gripReleased = false;
	Metronome armMovement;
	
	GameObject wrapCamObject;
	Sprite guyWithArmOut, otherArm;
	bool showingFallingGuy = false;

	public SceneTwelve(SceneManager manager) : base(manager) {
		touchSensor = new TouchSensor(new UnityInput());
	}
	
	public override void LoadAssets() {
		background = resourceFactory.Create(this, "TealBackground");
		bottomDither = resourceFactory.Create(this, "BottomDither");

		topHand = Sprite.create(this, "top_hand");
		bottomPalm = Sprite.create(this, "hand_base_top");
		bottomWrist = Sprite.create(this, "hand_base_bottom");
		bottomHandFingers = Sprite.create(this, "bottom_hand_fingers");
		
		indexClosed = Sprite.create(this, "index_closed");
		indexOpen = Sprite.create(this, "index_open");
		middleFingerOpen = Sprite.create(this, "middle_finger_open");
		otherFingerOpen = Sprite.create(this, "other_finger_open");
		littleFingerOpen = Sprite.create(this, "little_finger_open");
		thumbOpen = Sprite.create(this, "thumb_open");
		
		background.active = false;
		bottomDither.active = false;

		topHand.visible(false);
		bottomPalm.visible(false);
		bottomWrist.visible(false);
		bottomHandFingers.visible(false);

		indexClosed.visible(false);
		indexOpen.visible(false);
		middleFingerOpen.visible(false);
		otherFingerOpen.visible(false);
		littleFingerOpen.visible(false);
		thumbOpen.visible(false);
		
		// second part of the scene: zoomed out
		guyWithArmOut = Sprite.create(this, "guy_armout");
		otherArm = Sprite.create(this, "other_arm");

		guyWithArmOut.visible(false);
		otherArm.visible(false);

	}
	
	public override void Setup () {
		timeLength = 8.0f;
		
		background.active = true;
		bottomDither.active = true;
		topHand.visible(true);
		bottomPalm.visible(true);
		bottomWrist.visible(true);
		bottomHandFingers.visible(true);

		indexClosed.visible(true);
		indexOpen.visible(true);
		middleFingerOpen.visible(true);
		otherFingerOpen.visible(true);
		littleFingerOpen.visible(true);
		thumbOpen.visible(true);

		topHand.setScreenPosition(150, 66);
		
		bottomPalm.setScreenPosition(100, -10);
		bottomPalm.setDepth(0);
		
		bottomWrist.setScreenPosition(100, -10);
		bottomWrist.setDepth(0);
		
		bottomHandFingers.setScreenPosition(143, 82);
		bottomHandFingers.setDepth(3);
		
		armSwinger = new ArmSwinger(Time.time, bottomWrist);
		
		indexClosed.setScreenPosition(186, 52);
		indexClosed.setDepth(2);

		indexOpen.setScreenPosition(190, 9);
		middleFingerOpen.setScreenPosition(258, 22);
		otherFingerOpen.setScreenPosition(283, 42);
		littleFingerOpen.setScreenPosition(300, 70);
		thumbOpen.setScreenPosition(110, 80);
		
		openFingers = new Sprite[] { thumbOpen, indexOpen, middleFingerOpen, otherFingerOpen, littleFingerOpen };
		initializeOpenFingers(openFingers);

		wrapCamObject = new GameObject("vertial wrap Camera");
	}

	public override void Update () {
		if (showingFallingGuy) {
			if (armMovement.isNextTick(Time.time)) {
				guyWithArmOut.move(0, 20);
				otherArm.move(0, -20);
				if (guyWithArmOut.getScreenPosition().y >= Camera.main.pixelHeight) {
					guyWithArmOut.move(0, -Camera.main.pixelHeight);
				}
				if (otherArm.getScreenPosition().y <= 0) {
					otherArm.move(0, Camera.main.pixelHeight);
				}
			}
			return;
		}
		
		armSwinger.Update();
		
		if (gripReleased) {
			if (armMovement.currentTick(Time.time) > 2) {
				// cut to guy falling
				hideLargeSceneProps();
				showFallingGuy();
				endScene();
			}
			if (armMovement.isNextTick(Time.time)) {
				moveTopArmUp();
			}
			return;
		}

		if (openFingers.Length == nextFinger) {
			gripReleased = true;
			armMovement = new Metronome(Time.time, 0.1f);
			return;
		}
		
		if (touchSensor.insideSprite(topHand)) {
			openFingers[nextFinger].visible(true);
			
			if (nextFinger < 4) {
				armSwinger.swing(Time.time);
			}
			
			if (openFingers[nextFinger] == indexOpen) {
				indexClosed.visible(false);
			}
			
			nextFinger++;
		}
	}

	private void moveTopArmUp() {
		topHand.move(-2, 10);
		foreach(Sprite finger in openFingers) {
			finger.move(-2, 10);
		}
	}

	public override void Destroy () {
		GameObject.Destroy(background);
		GameObject.Destroy(bottomDither);
		Sprite.Destroy(topHand);
		Sprite.Destroy(bottomPalm);
		Sprite.Destroy(bottomWrist);
		Sprite.Destroy(bottomHandFingers);
		Sprite.Destroy(indexClosed);

		foreach(var finger in openFingers) {
			Sprite.Destroy(finger);
		}

		Sprite.Destroy(guyWithArmOut);
		Sprite.Destroy(otherArm);
		GameObject.Destroy(wrapCamObject);

	}
	
	private void hideLargeSceneProps() {
		topHand.visible(false);
		bottomPalm.visible(false);
		bottomWrist.visible(false);
		bottomHandFingers.visible(false);
		foreach(var finger in openFingers) {
			finger.visible(false);
		}
	}
	
	private void showFallingGuy() {
		showingFallingGuy = true;

		// construct additional camera that is positioned above the screen to show vertical wrapping
		wrapCamObject.AddComponent<Camera>();
		var wrapCam = wrapCamObject.GetComponent<Camera>();
		wrapCam.depth = 0;
		wrapCam.cullingMask = 2;
		wrapCam.clearFlags = CameraClearFlags.Depth;
		wrapCam.orthographic = true;
		wrapCam.orthographicSize = 100;
		wrapCam.transform.Translate(0, 200, -10);
		//wrapCam.pixelRect = new Rect(0, 0, 480, 334);
		
		// put sprites in their own layer so the other camera doesn't render the background
		guyWithArmOut.gameObject.layer = 1;
		otherArm.gameObject.layer = 1;

		background.active = true;
		bottomDither.active = true;

		guyWithArmOut.setScreenPosition(180, 0);
		guyWithArmOut.visible(true);
		otherArm.setScreenPosition(0, 0);
		otherArm.visible(true);
	}
	
	private void initializeOpenFingers(Sprite[] openFingers) {
		foreach(var finger in openFingers) {
			finger.setDepth(3); // put it on top of the hand
			finger.visible(false);
		}
	}
	
	class ArmSwinger {
		private const int swingLength = 8;
		private const float swingIncrement = 0.4f;

		bool swinging;
		Sprite bottomArm;
		GameObject swingPivot;
		Metronome swingInterval;
		
		public ArmSwinger(float startTime, Sprite bottomArm) {
			this.bottomArm = bottomArm;
			swingPivot = bottomArm.createPivotOnTopLeftCorner();
			
			swingInterval = new Metronome(startTime, 0.1f);
		}
		
		public void swing(float time) {
			swinging = true;
			swingInterval = new Metronome(Time.time, 0.1f);
		}
		
		public void Update() {
			if (swinging && swingInterval.isNextTick(Time.time)) {
				if (swingInterval.nextTick < swingLength)
					swingPivot.transform.Rotate(0f, 0f, swingIncrement);
				else if (swingInterval.nextTick >= swingLength && swingInterval.nextTick < swingLength * 2)
					swingPivot.transform.Rotate(0f, 0f, -swingIncrement);
				else if (swingInterval.nextTick == swingLength * 2) {
					swinging = false;
					swingInterval = new Metronome(Time.time, 0.1f);
				}

			}
		}
	}
}
