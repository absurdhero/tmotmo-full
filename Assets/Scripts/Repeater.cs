using UnityEngine;

/// Repeats an action on timed intervals by calling OnTick()
public abstract class Repeater
{
	protected float interval;
	protected int ticks;
	protected float startTime;
	protected int currentTick = 0;

	public Repeater (float tickInterval, int totalTicks, float startTime)
	{
		this.interval = tickInterval;
		this.ticks = totalTicks;
		this.startTime = startTime;
	}
	
	public Repeater (float tickInterval, int totalTicks) : this(tickInterval, totalTicks, Time.time) {
	}
	
	public Repeater(float tickInterval) : this(tickInterval, 0) {
	}
	
	public int Count() {
		return currentTick;
	}

	public virtual void Update (float currentTime)
	{
		float run_time = (currentTime - startTime);
		if (ticks != 0 && currentTick >= ticks) {
			return;
		}

		int index = (int)(run_time / interval);
		if (index != currentTick) {
			currentTick = index;
			OnTick();
		}
	}
	
	public abstract void OnTick();
	
	public float Length() {
		return interval * ticks;
	}
	
	public void Reset(float time) {
		currentTick = 0;
		startTime = time;
	}
}

