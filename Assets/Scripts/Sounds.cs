using UnityEngine;
using System;
using System.Collections.Generic;

// Handles all sounds/stems in the app
// This is a MonoBehaviour class only to use the Unity features to initialize AudioSource members
public class Sounds : MarshalByRefObject {
	const float trackStartingOffset = 8.0f;
	
	public AudioSource guitar1;
	public AudioSource keys;
	public AudioSource bass_beatsfx;
	public AudioSource guitars2_beats;
	public AudioSource lead_bgvocals;
	public AudioSource vocalsfx_livedrums;

	public AudioSource[] orderedStems;
	private AudioSource playingStem; // we need this to do playingStem.time
	public static string playingStems = "??";

	const int SAMPLE_RATE = 44100; // samples per second
	
	GameObject gameObject;
	
	public Sounds(GameObject gameObject) {
		this.gameObject = gameObject;
	}
	
	virtual public void Start () {
		guitar1 = gameObject.AddComponent<AudioSource>();
		guitar1.clip = (AudioClip)Resources.Load ("Sounds/GUITARS 1 STEM", typeof(AudioClip));
		keys = gameObject.AddComponent<AudioSource>();
		keys.clip = (AudioClip)Resources.Load ("Sounds/KEYS STEM_01", typeof(AudioClip));
		bass_beatsfx = gameObject.AddComponent<AudioSource>();
		bass_beatsfx.clip = (AudioClip)Resources.Load("Sounds/bass_beatsfx", typeof(AudioClip));
		guitars2_beats = gameObject.AddComponent<AudioSource>();
		guitars2_beats.clip = (AudioClip)Resources.Load ("Sounds/guitars2_beats", typeof(AudioClip));
		lead_bgvocals = gameObject.AddComponent<AudioSource>();
		lead_bgvocals.clip = (AudioClip)Resources.Load ("Sounds/lead_bgvocals", typeof(AudioClip));
		vocalsfx_livedrums = gameObject.AddComponent<AudioSource>();
		vocalsfx_livedrums.clip = (AudioClip)Resources.Load ("Sounds/vocalsfx_livedrums", typeof(AudioClip));
		
		
		orderedStems = new AudioSource[]
		{ bass_beatsfx, vocalsfx_livedrums, guitars2_beats, lead_bgvocals, guitar1, keys };
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
		stem.time = when + trackStartingOffset;
	}

	public float getAudioTime () {
		if (playingStem == null) {
			return 0f;
		}
		return ((float)playingStem.timeSamples) / SAMPLE_RATE - trackStartingOffset;
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
			stem.Stop ();
			stem.time = 0f;
			stem.Play ();
		}
		playingStem = bass_beatsfx;
	}

	public void playAllStems () {
		playStems (new List<AudioSource> (orderedStems));
	}

	public void playStems (List<AudioSource> toPlay) {
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
		if (stem == guitar1)
			return "guitar1 ";
		if (stem == keys)
			return "keys ";
		if (stem == bass_beatsfx)
			return "bass_beatsfx ";
		if (stem == guitars2_beats)
			return "guitars2_beats ";
		if (stem == lead_bgvocals)
			return "lead_bgvocals ";
		if (stem == vocalsfx_livedrums)
			return "vocalsfx_livedrums ";

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
