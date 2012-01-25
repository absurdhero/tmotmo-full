using UnityEngine;
using System.Collections;
using System;

public delegate void SceneEndEventHandler(Scene sender, EventArgs e);

public abstract class Scene {
	protected float timeLength = 0.0f;

	public virtual float TimeLength() {
		return timeLength;
	}
	
	public event SceneEndEventHandler SceneEnded;
	
	public bool completed { get; set; }
	
	public Scene() {
		completed = false;
	}

	public abstract void Setup();
	public abstract void Update();
	public abstract void Destroy();
	
	public virtual void Transition() {
		if (SceneEnded != null) {
			SceneEnded(this, EventArgs.Empty);
		}
	}
	
	protected static bool ContainsTouch(Rect rect, Touch touch) {
		return rect.Contains(new Vector2(touch.position.x, touch.position.y));
	}

	protected static bool SpriteContainsTouch(GameObject obj, Touch touch) {
		return ContainsTouch(obj.GetComponent<Sprite>().ScreenRect(), touch);
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
