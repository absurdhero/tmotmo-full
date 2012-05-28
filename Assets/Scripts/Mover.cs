using UnityEngine;
using System;
using System.Collections.Generic;

class Mover : Animatable {
	protected ICollection<Sprite> sprites;
	protected int startingTick, endingTick;
	Vector3 velocity;
	
	public Mover(ICollection<Sprite> sprites, Vector3 velocity, int startingTick, int endingTick) {
		this.sprites = sprites;
		this.startingTick = startingTick;
		this.endingTick = endingTick;
		this.velocity = velocity;
	}

	public void animate(int tick) {
		if (tick >= startingTick && tick <= endingTick) {
			foreach(var sprite in sprites) {
				sprite.move(velocity);
			}
		}
	}
}
