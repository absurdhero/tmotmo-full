using System;
using System.Collections;
using UnityEngine;

public abstract class Scene : MarshalByRefObject {
	protected float timeLength = 0.0f;
	protected Camera camera;
	protected SceneManager sceneManager;
	protected GameObjectFactory<string> resourceFactory;
	public float rewindTime { get; protected set; }

	public virtual float TimeLength() {
		return timeLength;
	}
	
	public bool completed { get; private set; }
	
	public void endScene() {
		completed = true;
	}
	
	public Scene(SceneManager manager) : this(manager, new ResourceFactory()){
	}

	public Scene(SceneManager manager, GameObjectFactory<string> resourceFactory) {
		sceneManager = manager;
		this.resourceFactory = resourceFactory;
		completed = false;
		camera = Camera.main;
	}

	public abstract void Setup();
	public abstract void Update();
	public abstract void Destroy();
	
	public virtual void Transition() {
		sceneManager.NextScene();
	}
	
	protected void rewindLoop(float seconds) {
		rewindTime = seconds;
	}
	protected virtual void ConsumeTouches() {
		for (int i = 0; i < Input.touchCount; i++) {
			Input.GetTouch(i);
		}
	}
	
	protected void MoveToScreenXY(GameObject obj, int x, int y) {
		var layoutpos = camera.ScreenToWorldPoint(new Vector3(x, y, 0));
		obj.transform.position = new Vector3(layoutpos.x, layoutpos.y, obj.transform.position.z);
	}
}
