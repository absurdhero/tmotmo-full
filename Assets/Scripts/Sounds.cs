using UnityEngine;
using System.Collections.Generic;

// Handles all sounds/stems in the app
// This is a MonoBehaviour class only to use the Unity features to initialize AudioSource members
public class Sounds : MonoBehaviour {
  public static Sounds obj; // The Singleton
  
  public AudioSource Combo_Bs_Vfx;
  public AudioSource Combo_Bt_BgV;
  public AudioSource Combo_G1_LD;
  public AudioSource Combo_G2_Ky;
  public AudioSource Combo_Voc_Dfx;
    
  private AudioSource[] orderedStems;
  private AudioSource playingStem; // we need this to do playingStem.time
  
  public static string playingStems = "??";
  
  const int SAMPLE_RATE = 44100; // samples per second

  // Use this for initialization
  void Start () {
    orderedStems = new AudioSource[] {Combo_Bs_Vfx, Combo_Bt_BgV, Combo_G1_LD, Combo_G2_Ky, Combo_Voc_Dfx};
    
    obj = this;
  }

  void Update () {} // this is not really a MonoBehaviour object

  public void playAudio() {
    foreach (var stem in orderedStems) {
      stem.Play();
    }
  }
  
  public void setAudioTime(float when) {
    foreach (AudioSource stem in orderedStems) {
      setTime(stem, when);
    }
  }
  
  private void setTime(AudioSource stem, float when) {
    stem.time = when;
  }
  
  public float getAudioTime() {
    if (playingStem == null) {
      return 0f;
    }
    return ((float)playingStem.timeSamples)/SAMPLE_RATE;
  }
  
  // Strategy: Round 1: Play all stems
  // Then remove the last (highest array index) stem each round...
  // ...until only the [0] is left, after that start adding in the highest numbers again...
  // ...until we're back to playing all again, and then we start over
  public void pickStemsFor(int repetition) {
    var toPlay = new List<AudioSource>();
  
    toPlay.Add(orderedStems[0]);
      
    int cycle = 2*(orderedStems.Length -1);
    int n = repetition % cycle;
    
    if (n < cycle/2) {
      for (int i = 1; i <= cycle/2-n; i++) {
        toPlay.Add(orderedStems[i]);
      }
    } else {
      for (int i = 1; i <= n-cycle/2; i++) {
        toPlay.Add(orderedStems[orderedStems.Length - i]);
      }
    }
    stemStatus(toPlay);
    //Debug.Log(playingStems);
    playStems(toPlay);
  }
  
  public void startPlaying() {
    foreach (var stem in orderedStems) {
      stem.Play();
    }
    playingStem = Combo_Bs_Vfx;
  }
  
  public void playAllStems() {
    playStems(new List<AudioSource>(orderedStems));
  }
  
  private void playStems(List<AudioSource> toPlay) {
    logStems(toPlay);
      
    foreach (AudioSource stem in orderedStems) {
      stem.volume = (toPlay.Contains(stem) ? 1f : 0f);
    }
  }
  
  private void logStems(List<AudioSource> toPlay) {
    string msg = playingStems +  " - ";
    foreach (var stem in toPlay) {
      msg += print(stem);
    }
    //Debug.Log(msg);
  }
  
  private string print(AudioSource stem) {
    if (stem == Combo_Bs_Vfx)  return "Bs+Vfx ";
    if (stem == Combo_Bt_BgV)  return "Bt+BgV ";
    if (stem == Combo_G1_LD)   return "G1+LD ";
    if (stem == Combo_G2_Ky)   return "G2+Ky ";
    if (stem == Combo_Voc_Dfx) return "Voc+Dfx ";

    return "SOMeTHING'S MISSING!!";
  }
  
  private void stemStatus(List<AudioSource> toPlay) {
    string result = "";
    foreach (AudioSource stem in orderedStems) {
      result +=  (toPlay.Contains(stem) ? ":" : ".");
    }
    playingStems = result;
  }
}