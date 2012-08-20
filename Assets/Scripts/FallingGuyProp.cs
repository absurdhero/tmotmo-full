using UnityEngine;

public class FallingGuyProp {
	OffsetCamera offsetCamera;
	public Camera wrapCam { get; private set; }
	public Sprite guyWithArmOut { get; private set; }
	public Sprite otherArm { get; private set; }
	bool armIsMovingAway = false;
	bool leftSideStopped, rightSideStopped;
	int fallAndStopSpeed = 10;
	bool armsAreReadyToMoveInward = false;
	
	const int leftFinalBottomHeight = -50;
	const int rightFinalBottomHeight = -30;

	public FallingGuyProp() {
	}

	public void LoadAssets() {
		guyWithArmOut = Sprite.create("SceneTwelve/guy_armout");
		otherArm = Sprite.create("SceneTwelve/other_arm");

		guyWithArmOut.visible(false);
		otherArm.visible(false);
	}

	public void Setup() {
		if (offsetCamera != null) return; // skip if prop was already set up

		armIsMovingAway = true;

		// construct additional camera that is positioned above the screen to show vertical wrapping
		offsetCamera = new OffsetCamera(new Vector3(0, 200, -10), 2);

		// put sprites in their own layer so the other camera doesn't render the background
		guyWithArmOut.gameObject.layer = 1;
		otherArm.gameObject.layer = 1;

		guyWithArmOut.setScreenPosition(180, -30);
		guyWithArmOut.visible(true);
		otherArm.setScreenPosition(100, -70);
		otherArm.visible(true);
	}

	public void updateFallingGuy(Metronome metronome) {
		if (metronome.isNextTick(Time.time)) {
			if (!armIsMovingAway) {
				if (!leftSideStopped) scrollLeftSide();
				if (!rightSideStopped) scrollRightSide();
				
				if (leftSideStopped && rightSideStopped) {
					alignArmsAtBottomOfScreen();
				}
			} else {
				if (!armIsOnLeftOfScreen()) {
					otherArm.move(-10, -5);
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
	}
	
	public void updateFallingGuyAndMoveApart(Metronome metronome, float time) {
		if (metronome.isNextTick(time)) {
			scrollFall();
			if (metronome.currentTick(time) < 30) {
				moveArmsApart();
			}
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
		otherArm.move(-1, 0);
		guyWithArmOut.move(5, 0);
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
		otherArm.move(0, -20);
		if (otherArm.getScreenPosition().y <= 0) {
			otherArm.move(0, Camera.main.pixelHeight);
		}
	}
	
	private void scrollRightSide() {
		guyWithArmOut.move(0, 20);
		if (guyWithArmOut.getScreenPosition().y >= Camera.main.pixelHeight) {
			guyWithArmOut.move(0, -Camera.main.pixelHeight);
		}
	}
	
	private void scrollFall() {
		scrollLeftSide();
		scrollRightSide();
	}
	
	private void alignArmsAtBottomOfScreen() {
		var otherArmHeight = otherArm.getScreenPosition().y;
		if (otherArmHeight > leftFinalBottomHeight) {
			otherArm.move(0, -Mathf.Min(fallAndStopSpeed, otherArmHeight - leftFinalBottomHeight));
		}

		var guyArmHeight = guyWithArmOut.getScreenPosition().y;
		if (guyArmHeight > rightFinalBottomHeight) {
			guyWithArmOut.move(0, -Mathf.Min(fallAndStopSpeed, guyArmHeight - rightFinalBottomHeight));
		}
	}
	
	private void moveArmsInward() {
		if (handsAreAlmostTouching()) return;
		
		otherArm.move(4, 0);
		guyWithArmOut.move(-6, 0);
	}

	public void Destroy() {
		Sprite.Destroy(guyWithArmOut);
		Sprite.Destroy(otherArm);
		offsetCamera.Destroy();
	}
}
