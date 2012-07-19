using System;
using UnityEngine;

public class LoopTracker : MarshalByRefObject {
	int repetition;
	float loopStart = 0.0f;
	float loopEnd = 0.0f;
	
	Sounds sounds;

	public LoopTracker(Sounds sounds) {
		this.sounds = sounds;
	}
	
	public void startPlaying() {
		sounds.startPlaying();
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
	
	public void ChangeLoopLength(float loopLength) {
		loopEnd = loopStart + loopLength;
	}

	public void Rewind(float seconds) {
		loopStart -= seconds;
		loopEnd -= seconds;
	}
	
	public bool IsLoopOver() {
		return (loopEnd == 0.0f) || (sounds.getAudioTime() > loopEnd);
	}
	
	public float TimeLeftInCurrentLoop() {
		if (IsLoopOver()) return 0.0f;
		return loopEnd - sounds.getAudioTime();
	}
	
	private void PlayRepetition() {
		sounds.pickStemsFor(repetition);
		sounds.setAudioTime(loopStart);
	}
}