using System;
using UnityEngine;

public class BigHeadProp {
	GameObjectFactory<string> resourceFactory;
	
	GameObject background;
	public GameObject faceLeftObject { get; private set; }
	public GameObject faceRightObject { get; private set; }
	
	public Sprite faceLeft {
		get {
			return faceLeftObject.GetComponent<Sprite>();
		}
	}
	public Sprite faceRight {
		get {
			return faceRightObject.GetComponent<Sprite>();
		}
	}
	
	public BigHeadProp(GameObjectFactory<string> resourceFactory) {
		this.resourceFactory = resourceFactory;
	}

	public void Setup () {
		background = resourceFactory.Create(this, "PurpleQuad");

		faceLeftObject = resourceFactory.Create(this, "FaceLeft");
		faceLeftObject.layer = 1;
		faceLeft.setWorldPosition(-50f, -60f, -1f);

		faceRightObject = resourceFactory.Create(this, "FaceRight");
		faceRightObject.layer = 1;
		faceRight.setWorldPosition(10f, -60f, -1f);
	}
	
	public void Destroy () {
		GameObject.Destroy(background);
		GameObject.Destroy(faceLeftObject);
		GameObject.Destroy(faceRightObject);
	}

}
