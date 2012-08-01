using UnityEngine;

public class FallingGuyProp {
	OffsetCamera wrapCam;
	public Sprite guyWithArmOut { get; private set; }
	public Sprite otherArm { get; private set; }
	bool armIsMovingAway = false;
	bool leftSideStopped, rightSideStopped;

	public FallingGuyProp() {
	}

	public void LoadAssets() {
		guyWithArmOut = Sprite.create("SceneTwelve/guy_armout");
		otherArm = Sprite.create("SceneTwelve/other_arm");

		guyWithArmOut.visible(false);
		otherArm.visible(false);
	}

	public void Setup() {
		if (wrapCam != null) return; // skip if prop was already set up

		armIsMovingAway = true;

		// construct additional camera that is positioned above the screen to show vertical wrapping
		wrapCam = new OffsetCamera(new Vector3(0, 200, -10), 2);

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
			} else {
				if (!armIsOnLeftOfScreen()) {
					otherArm.move(-10, -5);
				}
				if (armIsOnLeftOfScreen()) {
					armIsMovingAway = false;
					scrollFall();
				}
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
	
	public void stopLefSide() {
		leftSideStopped = true;
	}
	
	public void stopRightSide() {
		rightSideStopped = true;
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

	public void Destroy() {
		Sprite.Destroy(guyWithArmOut);
		Sprite.Destroy(otherArm);
		wrapCam.Destroy();
	}
}
