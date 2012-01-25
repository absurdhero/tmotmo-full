using UnityEngine;
using System;
using System.Collections.Generic;

// Handles all sounds/stems in the app
// This is a MonoBehaviour class only to use the Unity features to initialize AudioSource members
public class Sounds : MarshalByRefObject {
	public AudioSource bass;
	public AudioSource vocalFX;
	public AudioSource beats;
	public AudioSource vocalBG;
	public AudioSource guitar1;
	public AudioSource liveDrums;
	public AudioSource guitar2;
	public AudioSource keyboard;
	public AudioSource vocals;
	public AudioSource drumBeats;	

	private AudioSource[] orderedStems;
	private AudioSource playingStem; // we need this to do playingStem.time
	public static string playingStems = "??";

	const int SAMPLE_RATE = 44100; // samples per second
	
	GameObject gameObject;
	
	public Sounds(GameObject gameObject) {
		this.gameObject = gameObject;
	}
	
	virtual public void Start () {
		bass = gameObject.AddComponent<AudioSource>();
		bass.clip = (AudioClip)Resources.Load("Sounds/BASS STEM_01", typeof(AudioClip));
		vocalFX = gameObject.AddComponent<AudioSource>();
		vocalFX.clip = (AudioClip)Resources.Load ("Sounds/LEAD VOCALS FX STEM_01", typeof(AudioClip));
		beats = gameObject.AddComponent<AudioSource>();
		beats.clip = (AudioClip)Resources.Load ("Sounds/BEATS STEM_01", typeof(AudioClip));
		vocalBG = gameObject.AddComponent<AudioSource>();
		vocalBG.clip = (AudioClip)Resources.Load ("Sounds/BG VOCALS STEM_01", typeof(AudioClip));
		guitar1 = gameObject.AddComponent<AudioSource>();
		guitar1.clip = (AudioClip)Resources.Load ("Sounds/GUITARS 1 STEM", typeof(AudioClip));
		liveDrums = gameObject.AddComponent<AudioSource>();
		liveDrums.clip = (AudioClip)Resources.Load ("Sounds/LIVE DRUM STEM_02", typeof(AudioClip));
		guitar2 = gameObject.AddComponent<AudioSource>();
		guitar2.clip = (AudioClip)Resources.Load ("Sounds/GUITARS 2 STEM_01", typeof(AudioClip));
		keyboard = gameObject.AddComponent<AudioSource>();
		keyboard.clip = (AudioClip)Resources.Load ("Sounds/KEYS STEM_01", typeof(AudioClip));
		vocals = gameObject.AddComponent<AudioSource>();
		vocals.clip = (AudioClip)Resources.Load ("Sounds/LEAD VOCALS STEM_01", typeof(AudioClip));
		drumBeats = gameObject.AddComponent<AudioSource>();
		drumBeats.clip = (AudioClip)Resources.Load ("Sounds/DRUM_BEATS FX STEM_01", typeof(AudioClip));
		
		
		orderedStems = new AudioSource[]
			{ bass, vocalFX, beats, vocalBG, guitar1, liveDrums, guitar2, keyboard, vocals, drumBeats };
	}

	void Update () {} // this is not really a MonoBehaviour object

	public void playAudio () {
		foreach (var stem in orderedStems) {
			stem.Play ();
		}
	}

	public void setAudioTime (float when) {
		foreach (AudioSource stem in orderedStems) {
			setTime (stem, when);
		}
	}

	private void setTime (AudioSource stem, float when) {
		stem.time = when + 8.0f;
	}

	public float getAudioTime () {
		if (playingStem == null) {
			return 0f;
		}
		return ((float)playingStem.timeSamples) / SAMPLE_RATE - 8.0f;
	}

	// Strategy: Round 1: Play all stems
	// Then remove the last (highest array index) pair of stems each round...
	// ...until only the first and second are left, after that start adding in the highest numbers again...
	// ...until we're back to playing all again, and then we start over
	public void pickStemsFor (int repetition) {
		var toPlay = new List<AudioSource> ();
		
		toPlay.Add (orderedStems[0]);
		
		int cycle = orderedStems.Length - 2;
		int n = repetition % cycle;
		
		if (n < cycle / 2) {
			for (int i = 1; i <= cycle / 2 - n; i++) {
				toPlay.Add (orderedStems[i*2-1]);
				toPlay.Add (orderedStems[i*2]);
			}
		} else {
			for (int i = 1; i <= n - cycle / 2; i++) {
				toPlay.Add (orderedStems[orderedStems.Length - i*2 + 1]);
				toPlay.Add (orderedStems[orderedStems.Length - i*2]);
			}
		}
		stemStatus (toPlay);
		//Debug.Log(playingStems);
		playStems (toPlay);
	}

	public void startPlaying () {
		foreach (var stem in orderedStems) {
			stem.Play ();
		}
		playingStem = bass;
	}

	public void playAllStems () {
		playStems (new List<AudioSource> (orderedStems));
	}

	private void playStems (List<AudioSource> toPlay) {
		logStems (toPlay);
		
		foreach (AudioSource stem in orderedStems) {
			stem.volume = (toPlay.Contains (stem) ? 1f : 0f);
		}
	}

	private void logStems (List<AudioSource> toPlay) {
		string msg = playingStems + " - ";
		foreach (var stem in toPlay) {
			msg += print (stem);
		}
		//Debug.Log(msg);
	}

	private string print (AudioSource stem) {
		if (stem == bass)
			return "bass ";
		if (stem == vocalFX)
			return "vocalFX ";
		if (stem == beats)
			return "beats ";
		if (stem == vocalBG)
			return "vocalBG ";
		if (stem == guitar1)
			return "guitar1 ";
		if (stem == liveDrums)
			return "liveDrums ";
		if (stem == guitar2)
			return "guitar2 ";
		if (stem == keyboard)
			return "keyboard ";
		if (stem == vocals)
			return "vocals ";
		if (stem == drumBeats)
			return "drumBeats ";

		return "SOMeTHING'S MISSING!!";
	}

	private void stemStatus (List<AudioSource> toPlay) {
		string result = "";
		foreach (AudioSource stem in orderedStems) {
			result += (toPlay.Contains (stem) ? ":" : ".");
		}
		playingStems = result;
	}
}
