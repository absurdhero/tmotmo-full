using UnityEngine;

class SceneFour : Scene {
	HospitalRoom hospitalRoom;
	Wiggler wiggler;

	Sprite leftMouth;
	Sprite rightMouth;
	
	SpeechBubble speechBubble;
	
	MouthAnimator mouthMovement;
	
	const int END_POSITION = 120;
	
	public SceneFour(SceneManager manager, HospitalRoom room) : base(manager) {
		timeLength = 4.0f;
		this.hospitalRoom = room;
	}
	
	public override void LoadAssets() {
		addMouth();
	}

	public override void Setup(float startTime) {
		hospitalRoom.separateHalves(SceneThree.MAX_SPLIT);
		speechBubble = new SpeechBubble(camera, hospitalRoom.guyCenterPoint);
		mouthMovement = new MouthAnimator(startTime, leftMouth, rightMouth);
		wiggler = new Wiggler(startTime, timeLength, new[] {speechBubble.centerPivot()});
	}

	public override void Destroy() {
		wiggler.Destroy();
		speechBubble.Destroy();
		Sprite.Destroy(leftMouth);
		Sprite.Destroy(rightMouth);
		hospitalRoom.Destroy();
	}

	public override void Update() {
		hospitalRoom.Update();
		wiggler.Update(Time.time);
		mouthMovement.Update(Time.time);
		
		speechBubble.Update();

		if (solved) return;

		if (speechBubble.hasMoved()) {
			if (speechBubble.onRightSide())
				messagePromptCoordinator.progress("give not same a voice");
			else
				messagePromptCoordinator.progress("give same a voice");
		}

		if(speechBubble.inTerminalPosition) {
			speechBubble.snapToEnd();
			messagePromptCoordinator.solve(this, "give not same a voice");
			endScene();
			return;
		}

		hospitalRoom.hintWhenTouched(GameObject => {}, messagePromptCoordinator, new TouchSensor(input, gameObjectFinder));
	}
		
	private void addMouth() {
		leftMouth = resourceFactory.Create(this, "LeftMouth").GetComponent<Sprite>();
		rightMouth = resourceFactory.Create(this, "RightMouth").GetComponent<Sprite>();
		leftMouth.visible(false);
		rightMouth.visible(false);
		
		leftMouth.setWorldPosition(-19f, 25f, -4f);
		rightMouth.setWorldPosition(22.5f, 24f, -4f);
	}
	
	class MouthAnimator : Repeater {
		Sprite leftMouth;
		Sprite rightMouth;
		int delay = 2;
		int frame = 0;
		
		const int totalFrames = 16;		
		
		public MouthAnimator(float startTime, Sprite leftMouth, Sprite rightMouth) : base(0.25f, 0, startTime) {
			this.leftMouth = leftMouth;
			this.rightMouth = rightMouth;
		}
		
		public override void OnTick() {
			if (delay > 0) {
				delay -= 1;
				return;
			}

			moveMouth(leftMouth, 0, 4);
			moveMouth(rightMouth, 8, 8 + 4);
			
			incrementFrame();
		}
		
		private void incrementFrame() {
			frame = (frame + 1) % totalFrames;
		}

		private void moveMouth(Sprite mouth, int start, int end) {
			// combinations of nextTexture and activating/deactivating the mouth overlays
			if (frame == start) {
				mouth.visible(true);
			}
			else if(frame > start && frame < end) {
				mouth.DrawNextFrame();
			}
			else if(frame == end) {
				mouth.DrawNextFrame();
				mouth.visible(false);
			}
		}
	}
}