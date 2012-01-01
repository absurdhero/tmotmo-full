using UnityEngine;
using System.Collections.Generic;

//Cycles through a set of images, for animations
public class Cycler
{
	protected float interval;
	protected float length;
	protected IList<Sprite> sprites;
	protected float startTime;
	protected int currentIndex = 0;

	public Cycler (float interval, float length)
	{
		this.interval = interval;
		this.length = length;
		startTime = Time.time;
		sprites = new List<Sprite>();
	}
	
	public Cycler(float interval) : this(interval, 0.0f) {
	}
	
	public void AddSprite(Sprite sprite) {
		sprites.Add(sprite);
	}

	public void AddSprite(GameObject spriteObject) {
		var sprite = spriteObject.GetComponent<Sprite>();
		if (sprites == null) {
			throw new MissingComponentException("Expected object to have a Sprite component");
		}
		sprites.Add(sprite);
	}
		
	public int FrameIndex() {
		return currentIndex;
	}
	
	public virtual void Update ()
	{
		float run_time = (Time.time - startTime);
		if (length != 0.0f && run_time > length) {
			return;
		}

		int index = (int)(run_time / interval);
		if (index != currentIndex) {
			currentIndex = index;
			foreach(var sprite in sprites) {
				sprite.NextTexture();
			}
		}
	}
}
