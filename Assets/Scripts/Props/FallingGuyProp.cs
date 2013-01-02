using UnityEngine;

public class FallingGuyProp {
	OffsetCamera offsetCamera;
	public Camera wrapCam { get; private set; }
	public Sprite guyWithArmOut { get; private set; }
	public Sprite otherArm { get; private set; }
	private Sprite guyWithFist, otherFist;
	
	bool armIsMovingAway = false;
	bool leftSideStopped, rightSideStopped;
	int fallAndStopSpeed = 100;
	bool armsAreReadyToMoveInward = false;
	
	const int leftFinalBottomHeight = -50;
	const int rightFinalBottomHeight = -30;

	float scrollSpeed;
	float normalScrollSpeed;

	public FallingGuyProp() {
	}

	public void LoadAssets() {
		guyWithArmOut = Sprite.create("SceneTwelve/guy_armout");
		otherArm = Sprite.create("SceneTwelve/other_arm");

		guyWithArmOut.visible(false);
		otherArm.visible(false);
		
		guyWithFist = Sprite.create("SceneTwelve/guy_armout_fist");
		otherFist = Sprite.create("SceneTwelve/other_arm_fist");
		
		guyWithFist.visible(false);
		otherFist.visible(false);
	}

	public void Setup() {
		if (offsetCamera != null) return; // skip if prop was already set up

		armIsMovingAway = true;

		// construct additional camera that is positioned above the screen to show vertical wrapping
		offsetCamera = new OffsetCamera(new Vector3(0, 200, -10), 2);
		wrapCam = offsetCamera.camera;

		// travel one half screen length per second and take the overlapping offsetCamera into account
		scrollSpeed = normalScrollSpeed = (Camera.main.pixelHeight + 100) / 2;
		
		// put sprites in their own layer so the other camera doesn't render the background
		guyWithArmOut.gameObject.layer = 1;
		otherArm.gameObject.layer = 1;
		guyWithFist.gameObject.layer = 1;
		otherFist.gameObject.layer = 1;

		guyWithArmOut.setScreenPosition(180, -30);
		guyWithArmOut.visible(true);
		otherArm.setScreenPosition(100, -70);
		otherArm.visible(true);
	}

	public void updateFallingGuy(Metronome metronome) {
		if (!armIsMovingAway) {
			if (!leftSideStopped) scrollLeftSide();
			if (!rightSideStopped) scrollRightSide();
			
			if (leftSideStopped && rightSideStopped) {
				alignArmsAtBottomOfScreen();
			}
		} else {
			if (!armIsOnLeftOfScreen()) {
				otherArm.smoothMove(-100, -50);
			}
			if (armIsOnLeftOfScreen()) {
				armIsMovingAway = false;
				scrollFall();
			}
		}
		
		if (armsAreStoppedAtBottom() && armsAreReadyToMoveInward) {
			moveArmsInward();
		}
	}

	public void updateFallingGuyAndMoveApart(Metronome metronome, float time) {
		scrollFall();
		if (metronome.currentTick(time) < 30) {
			moveArmsApart();
		}
	}

	public bool armIsOnLeftOfScreen() {
		return otherArm.getScreenPosition().x <= 0;
	}
	
	public void ensureArmIsOnLeftOfScreen() {
		if (!armIsOnLeftOfScreen()) {
			otherArm.move(-100, 0);
		}
	}

	public void moveArmsApart() {
		otherArm.smoothMove(-10, 0);
		guyWithArmOut.smoothMove(50, 0);
	}
	
	public void stopLeftSide() {
		leftSideStopped = true;
	}
	
	public void stopRightSide() {
		rightSideStopped = true;
	}
	
	public bool armsAreStoppedAtBottom() {
		if (leftSideStopped
			&& rightSideStopped
			&& otherArm.getScreenPosition().y <= leftFinalBottomHeight
			&& guyWithArmOut.getScreenPosition().y <= rightFinalBottomHeight)
			return true;
		return false;
	}
	
	public void readyToMoveArmsInward() {
		armsAreReadyToMoveInward = true;
	}
	
	public bool handsAreAlmostTouching() {
		if (otherArm.getScreenPosition().x > 75
			&& guyWithArmOut.getScreenPosition().x <= 170)
			return true;
		return false;
	}

	private void scrollLeftSide() {
		otherArm.smoothMove(0, -scrollSpeed);
		if (otherArm.getScreenPosition().y <= 0) {
			otherArm.move(0, Camera.main.pixelHeight);
		}
	}
	
	private void scrollRightSide() {
		guyWithArmOut.smoothMove(0, scrollSpeed);
		if (guyWithArmOut.getScreenPosition().y >= Camera.main.pixelHeight) {
			guyWithArmOut.move(0, -Camera.main.pixelHeight);
		}
	}

	// slow down vertical scrolling as the arms get near based on the beat
	public void adjustScrollSpeed(float startTime) {
		// change the scroll speed based on the beat
		float p = Mathf.Repeat(Time.time - startTime, 1f);

		// oscillate the speed while the average speed is normalScrollSpeed.
		// Ideally the speed is slowest when the hands are close together
		// but being on the beat is more important.
		scrollSpeed = normalScrollSpeed + Mathf.Sin(p * 2f * Mathf.PI) * 100f;
	}

	public void makeFistsWhenTogether(Metronome metronome) {
		// flap the hands twice per second on the beat
		if ((leftSideStopped && rightSideStopped)
		    || (int) (metronome.currentTick(Time.time) * metronome.interval * 2f) % 2 == 1) {
			guyWithArmOut.visible(true);
			otherArm.visible(true);
			otherFist.visible(false);
			guyWithFist.visible(false);
		} else {
			otherFist.transform.position = otherArm.transform.position;
			guyWithFist.transform.position = guyWithArmOut.transform.position;
			otherFist.visible(true);
			guyWithFist.visible(true);
			guyWithArmOut.visible(false);
			otherArm.visible(false);
		}
	}

	private void scrollFall() {
		scrollLeftSide();
		scrollRightSide();
	}
	
	private void alignArmsAtBottomOfScreen() {
		var otherArmHeight = otherArm.getScreenPosition().y;
		if (otherArmHeight > leftFinalBottomHeight) {
			otherArm.smoothMove(0, -Mathf.Min(fallAndStopSpeed, otherArmHeight - leftFinalBottomHeight + 16));
		}

		var guyArmHeight = guyWithArmOut.getScreenPosition().y;
		if (guyArmHeight > rightFinalBottomHeight) {
			guyWithArmOut.smoothMove(0, -Mathf.Min(fallAndStopSpeed, guyArmHeight - rightFinalBottomHeight + 16));
		}
	}
	
	private void moveArmsInward() {
		if (handsAreAlmostTouching()) return;
		
		otherArm.smoothMove(40, 0);
		guyWithArmOut.smoothMove(-60, 0);
	}

	public void Destroy() {
		Sprite.Destroy(guyWithArmOut);
		Sprite.Destroy(otherArm);
		Sprite.Destroy(guyWithFist);
		Sprite.Destroy(otherFist);
		offsetCamera.Destroy();
	}
}
