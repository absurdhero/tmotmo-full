using UnityEngine;
using System.Collections;

public class MainController : MonoBehaviour {
  public Texture2D image1;
  public Texture2D image2;
  public Texture2D Background1;

  Rect backgroundRect = new Rect(0, 0, 480, 320);
  Rect skipRect = new Rect(360, 280, 100, 28);
  
  private int level;
  private Puzzle currentPuzzle;
  private int repetition;
 
  // Use this for initialization
  void Start() {
  // Stop reorientation weirdness 
  // http://answers.unity3d.com/questions/14655/unity-iphone-black-rect-when-i-turn-the-iphone
    iPhoneKeyboard.autorotateToPortrait = false; 
    iPhoneKeyboard.autorotateToPortraitUpsideDown = false; 
    iPhoneKeyboard.autorotateToLandscapeRight = false; 
    iPhoneKeyboard.autorotateToLandscapeLeft = false;
  
    Sounds.obj.startPlaying();
    setLevel(1);
    Sounds.obj.setAudioTime(ClipStart);
  }
  
  // Update is called once per frame
  void Update () {
//    Debug.Log(Sounds.obj.getAudioTime() + " vs " + ClipEnd);
    if (Sounds.obj.getAudioTime() > ClipEnd) {
      if (currentPuzzle.isSolved()) {
        setLevel(level+1);
      } else {
        repetition++;
        Sounds.obj.pickStemsFor(repetition);
        Sounds.obj.setAudioTime(ClipStart);
        currentPuzzle.Repeat();
      }
    }
    currentPuzzle.Update();
  }
  
  void OnGUI() {
    GUI.Box(backgroundRect, Background1);
  
    currentPuzzle.OnGUI(Sounds.obj.getAudioTime() - ClipStart);
    if (GUI.Button(skipRect, "skip")) {
      setLevel(level+1);
      Sounds.obj.setAudioTime(ClipStart);
    }
//    string msg = "" + avgU.firmX() + " " + avgG.firmX(); //Sounds.obj.times();
//    GUI.Label(new Rect(10, 10, 300, 24), msg);
  }
  
  public void restart() {
    Start();
  }
  
  private void setLevel(int newLevel) {
    level = newLevel;
    computeStartAndEnd();
    currentPuzzle = Puzzle.ForLevel(level, this);

    repetition = 0;
    Sounds.obj.pickStemsFor(repetition);
  }
 
  float ClipStart, ClipEnd;
  
  private void computeStartAndEnd() {
    ClipStart = 8.0f; // Silent offset at start
    for (int i = 1; i <level; i++) {
      ClipStart += PuzzleLength(i);
    }
    ClipEnd = ClipStart + PuzzleLength(level);
  }
  
  float PuzzleLength(int puzzle) {
    if (puzzle == 1) return 8.0f;
    
    return 4.0f;
  }
}