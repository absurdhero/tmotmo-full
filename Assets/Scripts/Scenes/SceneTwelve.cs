using UnityEngine;

public class SceneTwelve : Scene {
	GameObject background, bottomDither;
	
	Sprite topHand, bottomArm;
	
	Sprite bottomHandFingers, indexOpen, indexClosed, littleFingerOpen,
		   middleFingerOpen, otherFingerOpen, thumbOpen;
	
	Sprite[] openFingers;
	int nextFinger = 0;
	
	TouchSensor touchSensor;
	
	bool gripReleased = false;
	Metronome armMovement;

	public SceneTwelve(SceneManager manager) : base(manager) {
		touchSensor = new TouchSensor(new UnityInput());
	}
	
	public override void Setup () {
		timeLength = 8.0f;
		
		background = resourceFactory.Create(this, "TealBackground");
		bottomDither = resourceFactory.Create(this, "BottomDither");
		
		topHand = Sprite.create(this, "top_hand");
		topHand.setScreenPosition(150, 66);
		
		bottomArm = Sprite.create(this, "hand_base");
		bottomArm = bottomArm.GetComponent<Sprite>();
		bottomArm.setScreenPosition(120, -20);
		bottomArm.setDepth(0);

		bottomHandFingers = Sprite.create(this, "bottom_hand_fingers");
		bottomHandFingers.setScreenPosition(170, 100);
		bottomHandFingers.setDepth(3);
		
		indexClosed = Sprite.create(this, "index_closed");
		indexClosed.setScreenPosition(186, 52);
		indexClosed.setDepth(2);

		indexOpen = Sprite.create(this, "index_open");
		indexOpen.setScreenPosition(190, 9);
		middleFingerOpen = Sprite.create(this, "middle_finger_open");
		middleFingerOpen.setScreenPosition(258, 22);
		otherFingerOpen = Sprite.create(this, "other_finger_open");
		otherFingerOpen.setScreenPosition(283, 42);
		littleFingerOpen = Sprite.create(this, "little_finger_open");
		littleFingerOpen.setScreenPosition(300, 70);
		thumbOpen = Sprite.create(this, "thumb_open");
		thumbOpen.setScreenPosition(110, 80);
		
		openFingers = new Sprite[] { thumbOpen, indexOpen, middleFingerOpen, otherFingerOpen, littleFingerOpen };
		initializeOpenFingers(openFingers);
	}

	public override void Update () {
		if (gripReleased) {
			if (armMovement.currentTick(Time.time) > 10) {
				endScene();
			}
			if (armMovement.isNextTick(Time.time)) moveArmUp();
			return;
		}

		if (openFingers.Length == nextFinger) {
			gripReleased = true;
			armMovement = new Metronome(Time.time, 0.1f);
			return;
		}
		
		if (touchSensor.insideSprite(topHand)) {
			openFingers[nextFinger].visible(true);
			
			if (openFingers[nextFinger] == indexOpen) {
				indexClosed.visible(false);
			}
			
			nextFinger++;
		}
	}

	private void moveArmUp() {
		topHand.move(-2, 10);
		foreach(Sprite finger in openFingers) {
			finger.move(-2, 10);
		}
	}

	public override void Destroy () {
		GameObject.Destroy(background);
		GameObject.Destroy(bottomDither);
		Sprite.Destroy(topHand);
		Sprite.Destroy(bottomArm);
		Sprite.Destroy(bottomHandFingers);
		Sprite.Destroy(indexClosed);

		foreach(var finger in openFingers) {
			Sprite.Destroy(finger);
		}
	}
	
	private void initializeOpenFingers(Sprite[] openFingers) {
		foreach(var finger in openFingers) {
			finger.setDepth(3); // put it on top of the hand
			finger.visible(false);
		}
	}
}
