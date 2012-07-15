using UnityEngine;
using System;

class SceneFour : Scene {
	HospitalRoom hospitalRoom;

	GameObject leftMouth;
	GameObject rightMouth;
	
	SpeechBubble speechBubble;
	
	MouthAnimator mouthMovement;
	
	const int END_POSITION = 120;
	
	public SceneFour(SceneManager manager, HospitalRoom room) : base(manager) {
		this.hospitalRoom = room;
	}
	
	public override void LoadAssets() {
		addMouth();
	}

	public override void Setup() {
		timeLength = 4.0f;
		
		hospitalRoom.separateHalves(SceneThree.MAX_SPLIT);
		speechBubble = new SpeechBubble(resourceFactory, camera, hospitalRoom.guyCenterPoint);
		mouthMovement = new MouthAnimator(leftMouth, rightMouth);
	}

	public override void Destroy() {
		speechBubble.Destroy();
		GameObject.Destroy(leftMouth);
		GameObject.Destroy(rightMouth);
		hospitalRoom.Destroy();
	}

	public override void Update() {
		hospitalRoom.Update();
		mouthMovement.Update(Time.time);
		
		speechBubble.Update();

		if(speechBubble.inTerminalPosition) {
			speechBubble.snapToEnd();
			endScene();
			return;
		}		
	}
		
	private void addMouth() {
		leftMouth = resourceFactory.Create(this, "LeftMouth");
		rightMouth = resourceFactory.Create(this, "RightMouth");
		leftMouth.active = false;
		rightMouth.active = false;
		
		leftMouth.GetComponent<Sprite>().setWorldPosition(-24f, 24f, -4f);
		rightMouth.GetComponent<Sprite>().setWorldPosition(28f, 24f, -4f);		
	}
	
	class MouthAnimator : Repeater {
		GameObject leftMouth;
		GameObject rightMouth;
		int delay = 2;
		int frame = 0;
		
		const int totalFrames = 16;		
		
		public MouthAnimator(GameObject leftMouth, GameObject rightMouth) : base(0.25f) {
			this.leftMouth = leftMouth;
			this.rightMouth = rightMouth;
		}
		
		public override void OnTick() {
			if (delay > 0) {
				delay -= 1;
				return;
			}

			moveMouth(this.leftMouth, 0, 4);
			moveMouth(this.rightMouth, 8, 8 + 4);
			
			incrementFrame();
		}
		
		private void incrementFrame() {
			frame = (frame + 1) % totalFrames;
		}

		private void moveMouth(GameObject mouth, int start, int end) {
			// combinations of nextTexture and activating/deactivating the mouth overlays
			if (frame == start) {
				mouth.active = true;
			}
			else if(frame > start && frame < end) {
				mouth.GetComponent<Sprite>().DrawNextFrame();
			}
			else if(frame == end) {
				mouth.GetComponent<Sprite>().DrawNextFrame();
				mouth.active = false;	
			}
		}
	}
}