using System;
using System.Collections;
using UnityEngine;

public abstract class Scene : MarshalByRefObject {
	protected float timeLength = 0.0f;
	
	protected SceneManager sceneManager;

	public virtual float TimeLength() {
		return timeLength;
	}
	
	public bool completed { get; set; }
	
	public Scene(SceneManager manager) {
		sceneManager = manager;
		completed = false;
	}

	public abstract void Setup();
	public abstract void Update();
	public abstract void Destroy();
	
	public virtual void Transition() {
		sceneManager.NextScene();
	}

	protected virtual void ConsumeTouches() {
		for (int i = 0; i < Input.touchCount; i++) {
			Input.GetTouch(i);
		}
	}
	
	protected void MoveToScreenXY(GameObject obj, int x, int y) {
		var layoutpos = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 0));
		obj.transform.position = new Vector3(layoutpos.x, layoutpos.y, obj.transform.position.z);
	}
}
