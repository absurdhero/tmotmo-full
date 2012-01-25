using UnityEngine;

// This variant of the Cycler inserts a delay between sequences.
// Unlike its superclass, the totalCycles must be non-zero.
public class DelayedCycler : Cycler {
	float delay;
	float delayStart;
	
	public DelayedCycler (float interval, int totalCycles, float delay, float startTime) : base(interval, totalCycles, startTime) {
		this.delay = delay;
		// the last delay time would be this long before the start if this object had existed then
		delayStart = startTime - delay;
	}
	
	public DelayedCycler (float interval, int totalCycles, float delay) : this(interval, totalCycles, delay, Time.time) {
	}
	
	public override void Update(float currentTime) {
		float timeSinceStart = currentTime - startTime;
	
		// the cycler finished
		if (timeSinceStart > Length()) { 
			// new delay cycle hasn't happened yet
			if (delayStart < startTime) {
				delayStart = startTime + Length();
			}
			
			float timeSinceDelay = currentTime - delayStart;
			if (timeSinceDelay > delay) {
				// set the cycler to cycle again
				startTime = delayStart + delay;
				currentCycle = 0;
			}
		}
		
		base.Update(currentTime);
	}
}