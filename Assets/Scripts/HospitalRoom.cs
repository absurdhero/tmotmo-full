using UnityEngine;
using System;

public class HospitalRoom {
	GameObjectFactory<string> resourceFactory;

	GameObject room;
	
	int guyCenterOffset = 6;
	GameObject guyLeft;
	GameObject guyRight;
	float guySplitDelta;
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
		
	public HospitalRoom(GameObjectFactory<string> resourceFactory, Camera camera) {
		this.resourceFactory = resourceFactory;
		room = resourceFactory.Create(this, "HospitalRoomQuad");
		this.camera = camera;
	}
	
	private void DestroyIfNotNull(GameObject gobj) {
		if (gobj != null) GameObject.Destroy(gobj);
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
		guyLeft = resourceFactory.Create(this, "GuyLeft");
		guyRight = resourceFactory.Create(this, "GuyRight");
		
		var leftSprite = guyLeft.GetComponent<Sprite>();
		var rightSprite = guyRight.GetComponent<Sprite>();
		
		leftSprite.setScreenPosition((int) camera.pixelWidth / 2 - leftSprite.PixelWidth() + guyCenterOffset,
									 (int) camera.pixelHeight / 2 - leftSprite.PixelHeight() / 2);
		rightSprite.setScreenPosition((int) camera.pixelWidth / 2 + guyCenterOffset,
									  (int) camera.pixelHeight / 2 - rightSprite.PixelHeight() / 2);
		
		eyes = resourceFactory.Create(this, "EyesOpening");
		var pos = eyes.transform.position;
		pos.x = -0.55f;
		pos.y = 3.65f;
		pos.z -= 1;
		eyes.transform.position = pos;
	}
	
	public void addZzz() {
		zzz = resourceFactory.Create(this, "Zzz");
		var pos = zzz.transform.position;
		pos.x = 5;
		pos.y = 6;
		zzz.transform.position = pos;
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
		coverSprite.setCenterToViewportCoord(camera, 0.515f, 0.34f);
		
	}
	
	public void removeCover() {
		cover.active = false;
	}
	
	public void addFootboard() {
		addClipBoard();

		footBoard = resourceFactory.Create(this, "Footboard");
		var pos = footBoard.transform.position;
		pos.y = -9.2f;
		pos.x = -4.4875f;
		pos.z = -1;
		footBoard.transform.position = pos;
	}
	
	private void addClipBoard() {
		clipBoard = resourceFactory.Create(this, "ClipBoard");
		var pos = clipBoard.transform.position;
		pos.y = -8.0f;
		pos.x = -1.2f;
		pos.z = -1;
		clipBoard.transform.position = pos;
	}
	
	public void addHeartRate() {
		heartRate = resourceFactory.Create(this, "HeartRate");
		var pos = heartRate.transform.position;
		pos.y = 1.7f;
		pos.x = -11.5f;
		heartRate.transform.position = pos;
		
		// Repeat 7 frame animation every 2 seconds
		// That's 30bpm which might mean he is dying!
		heartRateCycler = new Cycler(1f/7f*2f, 0);
		heartRateCycler.AddSprite(heartRate);
	}
	
	public void doubleHeartRate() {
		heartRateCycler = new Cycler(1f/7f, 0);
		heartRateCycler.AddSprite(heartRate);
	}
	
	
	public Rect guyBounds(float inflation) {
		var leftRect = guyLeft.GetComponent<Sprite>().ScreenRect();
		var rightRect = guyRight.GetComponent<Sprite>().ScreenRect();
		var rect = new Rect(leftRect.x - inflation,
						leftRect.y - inflation,
						leftRect.width + rightRect.width + inflation * 2f,
					    leftRect.height + rightRect.height + inflation * 2f);
	
		return rect;
	}
	
	public bool guyContains(Vector2 position) {
		return guyLeft.GetComponent<Sprite>().Contains(position)
			|| guyRight.GetComponent<Sprite>().Contains(position);
	}
	
	public void separateHalves(float distance) {
		guySplitDelta = distance - guySplitDistance;
		guySplitDistance = distance;
		
	}

	private void setGuySplit() {
		var leftSprite = guyLeft.GetComponent<Sprite>();
		var rightSprite = guyRight.GetComponent<Sprite>();
		var leftRect = leftSprite.ScreenRect();
		var rightRect = rightSprite.ScreenRect();
		
		leftSprite.setScreenPosition((int) (leftRect.x - guySplitDelta), (int) leftRect.y);
		rightSprite.setScreenPosition((int) (rightRect.x + guySplitDelta), (int) rightRect.y);
		
		guySplitDelta = 0;
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