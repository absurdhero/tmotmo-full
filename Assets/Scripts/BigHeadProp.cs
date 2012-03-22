using System;
using UnityEngine;

public class BigHeadProp {
	GameObjectFactory<string> resourceFactory;
	
	GameObject background;
	public GameObject faceLeftObject { get; private set; }
	public GameObject faceRightObject { get; private set; }
	
	public GameObject faceParent;
	
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
		var leftPosition = faceLeftObject.transform.position;
		leftPosition.x = -5f;
		leftPosition.y = -6f;
		faceLeftObject.transform.position = leftPosition;
		
		faceParent = new GameObject();
		faceRightObject = resourceFactory.Create(this, "FaceRight");
		faceRightObject.transform.parent = faceParent.transform;
		faceParent.transform.Translate(1f, Camera.main.ScreenToWorldPoint(new Vector3(0f, faceRight.PixelHeight(), 0f)).y + 3, 0f);
//		var rightPosition = faceRightObject.transform.position;
//		rightPosition.x = 1f;
//		rightPosition.y = -6f;
//		faceRightObject.transform.position = rightPosition;
		var rightPosition = faceRightObject.transform.localPosition;
		rightPosition.x = 0f;
		rightPosition.y = -Camera.main.ScreenToWorldPoint(new Vector3(0f, faceRight.PixelHeight(), 0f)).y - 9f;
		faceRightObject.transform.localPosition = rightPosition;
	}
	
	public void Destroy () {
		GameObject.Destroy(background);
		GameObject.Destroy(faceLeftObject);
		GameObject.Destroy(faceRightObject);
	}
	
	
}
