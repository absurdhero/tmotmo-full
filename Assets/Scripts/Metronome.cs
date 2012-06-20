class Metronome {
	float startTime;
	public float interval { get; private set; }
	int lastNewTick = -1;

	public Metronome(float startTime, float interval) {
		this.startTime = startTime;
		this.interval = interval;
	}

	public int currentTick(float currentTime) {
		return (int) ((currentTime - startTime) / interval);
	}
	
	public bool isNextTick(float time) {
		var tick = currentTick(time);
		if (lastNewTick < tick) {
			lastNewTick = tick;
			return true;
		}
		return false;
	}
}
