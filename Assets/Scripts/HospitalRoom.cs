using UnityEngine;
using System;

public class HospitalRoom {
	GameObjectFactory<string> resourceFactory;

	GameObject room;
	GameObject guyLeft;
	GameObject guyRight;
	public GameObject cover;
	GameObject footBoard;
	GameObject clipBoard;	
	GameObject eyes;
	
	GameObject heartRate;
	Cycler heartRateCycler;
	
	public HospitalRoom(GameObjectFactory<string> resourceFactory) {
		this.resourceFactory = resourceFactory;
		room = resourceFactory.Create("Scene 2/HospitalRoomQuad");
	}
	
	public HospitalRoom() : this(new ResourceFactory()) {
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
	}
	
	public void addPerson() {
		guyLeft = resourceFactory.Create("Scene 2/GuyLeft");
		guyRight = resourceFactory.Create("Scene 2/GuyRight");
		
		var leftSprite = guyLeft.GetComponent<Sprite>();
		var rightSprite = guyRight.GetComponent<Sprite>();
		
		var bodyOffset = 6;
		leftSprite.setScreenPosition((int) Camera.main.pixelWidth / 2 - leftSprite.PixelWidth() + bodyOffset,
									 (int) Camera.main.pixelHeight / 2 - leftSprite.PixelHeight() / 2);
		rightSprite.setScreenPosition((int) Camera.main.pixelWidth / 2 + bodyOffset,
									  (int) Camera.main.pixelHeight / 2 - rightSprite.PixelHeight() / 2);
		
		eyes = resourceFactory.Create("Scene 2/EyesOpening");
		var pos = eyes.transform.position;
		pos.x = -0.55f;
		pos.y = 3.68f;
		pos.z -= 1;
		eyes.transform.position = pos;
	}
	
	public void openEyes() {
		if (eyes.GetComponent<Sprite>().LastTexture()) {
			eyes.active = false;
			return;
		}
		
		eyes.GetComponent<Sprite>().NextTexture();
	}
	
	public bool eyesTotallyOpen {
		get { return !eyes.active; }
	}
	
	public void addCover() {
		cover = resourceFactory.Create("Scene 2/Cover");
		var coverSprite = cover.GetComponent<Sprite>();
		coverSprite.setCenterToViewportCoord(Camera.main, 0.515f, 0.34f);
		
	}
	
	public void removeCover() {
		cover.active = false;
	}
	
	public void addFootboard() {
		addClipBoard();

		footBoard = resourceFactory.Create("Scene 2/Footboard");
		var pos = footBoard.transform.position;
		pos.y = -9.2f;
		pos.x = -4.4875f;
		footBoard.transform.position = pos;
	}
	
	private void addClipBoard() {
		clipBoard = resourceFactory.Create("Scene 2/ClipBoard");
		var pos = clipBoard.transform.position;
		pos.y = -8.0f;
		pos.x = -1.2f;
		clipBoard.transform.position = pos;
	}
	
	public void addHeartRate() {
		heartRate = resourceFactory.Create("Scene 2/HeartRate");
		var pos = heartRate.transform.position;
		pos.y = 1.7f;
		pos.x = -11.5f;
		heartRate.transform.position = pos;
		
		// Repeat 7 frame animation every 2 seconds
		// That's 30bpm which might mean he is dying!
		heartRateCycler = new Cycler(1f/7f*2f, 0);
		heartRateCycler.AddSprite(heartRate);
	}
	
	public void Update() {		
		if(heartRateCycler != null) {
			heartRateCycler.Update(Time.time);
		}
	}
}