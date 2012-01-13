using UnityEngine;
class LoopTracker {
	int repetition;
	float loopStart = 0.0f;
	float loopEnd = 0.0f;
	
	Sounds sounds;

	public LoopTracker(Sounds sounds) {
		this.sounds = sounds;
	}
	public void Repeat() {
		if (loopEnd == 0.0f) return;

		repetition++;
		PlayRepetition();
	}

	public void NextLoop(float loopLength) {
		repetition = 0;
		loopStart = loopEnd;
		loopEnd += loopLength;
		PlayRepetition();
	}
	
	public bool IsLoopOver() {
		return (loopEnd == 0.0f) || (sounds.getAudioTime() > loopEnd);
	}
	
	private void PlayRepetition() {
		sounds.pickStemsFor(repetition);
		sounds.setAudioTime(loopStart);
	}
}