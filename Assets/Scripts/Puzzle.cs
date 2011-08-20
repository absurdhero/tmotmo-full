using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//-------------------------------------------
// Let's call these 'Puzzle' since Scene and Level are taken by core Unity classes
abstract class Puzzle
{
  private bool solved = false;

  public virtual void OnGUI(float puzzleTime) { // puzzleTime is how many seconds into the time for this puzzle we are. Resets when we start over.
  }
  
  public virtual void Update() {
  }

  public virtual void Repeat() { // Just so you know, dear puzzle, you're being restarted
  }

  public bool isSolved() {
    return solved;
  }
  
  protected void setSolved() {
    solved = true;
    Sounds.obj.playAllStems();
  }

  protected delegate void GuiCode();
  protected void highlight(bool isOn, Color color, GuiCode guiCode) {
    var previousColor = GUI.color;
    if (isOn) {
      GUI.color = GUI.color * color;  // * for "layered" GUI.color effects
    }
    guiCode();
    GUI.color = previousColor;
  }

  public static Puzzle ForLevel(int level, MainController m) {
    switch (level) {
      case 1: return new Puzzle1();
      case 2: return new Puzzle2();
      case 3: return new Puzzle3();
      case 4: return new Puzzle4();
      case 5: return new Puzzle5();
      case 6: return new DragPuzzle();
      default: return new NullPuzzle(level, m);
    }
  }
  
  // the touch and drawing coordinates have inverted Y axes, so we subtract from 320
  public static bool containsTouch(Rect rect, Touch touch) {
    return rect.Contains(new Vector2(touch.position.x, 320 - touch.position.y));
  }

  protected void drawCentered(Texture2D image, float x, float y, float size) {
    GUI.DrawTexture(centeredSquare(x, y, size), image);
  }
  
  static protected Rect centeredSquare(float x, float y, float size) {
    return new Rect(x - size/2, y - size/2, size, size);
  }
}

//-------------------------------------------
// Placeholder for puzzles not yet built
class NullPuzzle : Puzzle
{
  static Rect   solveRect = new Rect(160, 50, 160, 28);
  static Rect restartRect = new Rect( 20, 280, 100, 28);

  int level;
  MainController mc;

  public NullPuzzle(int level, MainController mc) {
    this.level = level;
    this.mc = mc;
  }

  public override void OnGUI(float puzzleTime) {
    var msg = isSolved() ? "Level " + level + " solved" : "Solve level " + level;
    if (GUI.Button(solveRect, msg)) {
      setSolved();
    }
    if (GUI.Button(restartRect, "restart")) {
      mc.restart();
    }
  }
}

//-------------------------------------------
class Puzzle1 : Puzzle
{
  Rect circle = new Rect(70, 135, 120, 120);
  Cycler circles = new Cycler(0.5f, new List<Texture2D>(){Images.it.p1_Circle1, Images.it.p1_Circle2, Images.it.p1_Circle3, Images.it.p1_Circle4, Images.it.p1_Circle5, Images.it.transparent});
  
  Rect triangle = new Rect(280, 120, 120, 120);
  Cycler triangles = new Cycler(0.5f, new List<Texture2D>(){Images.it.p1_Triangle1, Images.it.p1_Triangle2, Images.it.p1_Triangle3, Images.it.transparent});

  Color hi = new Color(0.8f, 0.8f, 0.8f, 1f);
  Fader fader = new Fader(1.0f);

  bool touched1 = false;
  bool touched2 = false;
 
  public override void OnGUI(float puzzleTime) {
    fader.fadeOn();
      highlight(touched1, hi, delegate() {circles.draw(circle);});
      highlight(touched2, hi, delegate() {triangles.draw(triangle);});
    fader.fadeOff();
  }

  public override void Update() {
    touched1 = false;
    touched2 = false;
    for (int i = 0; i < Input.touchCount; i++) {
      var touch = Input.GetTouch(i);
      
      touched1 |= containsTouch(circle, touch);
      touched2 |= containsTouch(triangle, touch);
    }
    
    if (touched1 && touched2) {
      setSolved();
      fader.fadeNow();
    }
  }
}

//-------------------------------------------
class Puzzle2 : Puzzle
{
  Rect tapRect = new Rect(220, 100, 40, 120);
  Rect faceRect = centeredSquare(240, 120, 40);

  bool tapping = false;
  
  Rect bedRect = new Rect(240 - Images.it.p3_LeftBed.width, 70, 175, 250);

  int tapCount = 0;
  float lastPuzzleTime;

  public override void OnGUI(float puzzleTime) {
    GUI.DrawTexture(bedRect, Images.it.p3_FullBed);
    
    switch (tapCount) {
      case 0:
      case 1:
        GUI.DrawTexture(faceRect, Images.it.p2_Face1);
        break;
      case 2:
        GUI.DrawTexture(faceRect, Images.it.p2_Face2);
        break;
      case 3:
        GUI.DrawTexture(faceRect, Images.it.p2_Face3);
        break;
    }
    
    if (tapCount == 0) {
      lastPuzzleTime = puzzleTime;
    }
    
    drawZ(lastPuzzleTime);
    drawZ(lastPuzzleTime + 1f);
    drawZ(lastPuzzleTime + 2f);
    drawZ(lastPuzzleTime + 3f);
  }
  
  private void drawZ(float time) {
    float useTime = time % 4f;
    float delta = 30 * useTime;
    float size = 30*(1 - Mathf.Abs(2 - useTime)/2);
    drawCentered(Images.it.p2_Z, 260 + delta, 100 - delta, size);
  }

  public override void Update() {
    if (isSolved()) {
      return;
    }
    countTaps();
      
    if (tapCount == 3) {
      setSolved();
    }
  }
 
  private void countTaps() {
    var touched = false;
    for (int i = 0; i < Input.touchCount; i++) {
      var touch = Input.GetTouch(i);
      
      touched |= containsTouch(tapRect, touch);
    }
    
    if (!tapping && touched) {
      tapping = true;
      tapCount++; // count when tap starts
    }
    if (tapping && !touched) {
      tapping = false;
    }
  }
  
  public override void Repeat() {
    tapCount = 0;
  }
}

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++
class DragPuzzle : Puzzle // not used, but I want to keep the code around
{
  Fader fader = new Fader(1.0f);
  Dragger dragger = new Dragger(150, 250);

  Rect goal = new Rect(330, 150, 40, 40);

  public override void OnGUI(float puzzleTime) {
    fader.fadeOn();
      GUI.DrawTexture(dragRect(), Images.it.img1);
      GUI.Box(goal, "X");
    fader.fadeOff();
  }

  public override void Update() {
    if (isSolved()) {
      return;
    }
    
    dragger.Update(dragRect(), Input.touches);
      
    if (goal.Contains(dragger.location)) {
      setSolved();
      fader.fadeNow();
    }
  }
  
  private Rect dragRect() {
    return new Rect(dragger.location.x - 50, dragger.location.y - 50, 100.0f, 100.0f);
  }
}

//-------------------------------------------
enum State {BED, NO_BED, SPLIT};
class Puzzle3 : Puzzle
{
  // the locations are the upper left corners
  Vector2  leftLocation = new Vector2(240 - Images.it.p3_LeftBed.width, 70);
  Vector2 rightLocation = new Vector2(240, 70);
 
  Rect bedRect = new Rect(240 - Images.it.p3_LeftBed.width, 70, 175, 250);

  Fader fader = new Fader(1.0f);
  
  State phase = State.BED; // we have 3 phases

  public override void OnGUI(float puzzleTime) {
    determinePhase(puzzleTime);
    
    switch (phase) {
      case State.BED:
        GUI.DrawTexture(bedRect, Images.it.p3_FullBed);
        break;
      case State.NO_BED:
        GUI.DrawTexture(bedRect, Images.it.p3_NoBed);
        break;
      case State.SPLIT:
        fader.fadeOn();
          GUI.DrawTexture( leftRect(), Images.it.p3_LeftBed);
          GUI.DrawTexture(rightRect(), Images.it.p3_RightBed);
        fader.fadeOff();
        break;
    }
  }
 
  public override void Update() {
    if (phase != State.SPLIT) {
      return;
    }
    for (int i = 0; i < Input.touchCount; i++) {
      var touch = Input.GetTouch(i);
      
      if (touch.phase == TouchPhase.Moved) {
        Debug.Log(touch.deltaPosition.x + ", " + touch.deltaPosition.y);
        if (containsTouch(leftRect(), touch) && touch.deltaPosition.x < 0) { //TODO use position before delta
          leftLocation.x += touch.deltaPosition.x;
        }
        if (containsTouch(rightRect(), touch) && touch.deltaPosition.x > 0) {
          rightLocation.x += touch.deltaPosition.x;
        }
      }
    }
      
    if (leftLocation.x < 30 && rightLocation.x > 350 && !isSolved()) {
      setSolved();
      fader.fadeNow();
    }
  }

  private void determinePhase(float puzzleTime) {
    switch (phase) {
      case State.BED:
        if (puzzleTime > 1.3f) phase = State.NO_BED;
        break;
      case State.NO_BED:
        if (puzzleTime > 3.2f) phase = State.SPLIT;
        break;
    }
  }

  
  private Rect leftRect() {
    return new Rect(leftLocation.x, leftLocation.y, 93, 250);
  }

  private Rect rightRect() {
    return new Rect(rightLocation.x, rightLocation.y, 82, 250);
  }
}

//-------------------------------------------
class Puzzle4 : Puzzle
{
  Dragger dragger = Dragger.horizontal(50, 50);

  Rect bedRect = new Rect(240 - Images.it.p3_LeftBed.width, 70f, 175f, 250f);
  Rect faceRect = centeredSquare(240, 120, 40);

  public override void OnGUI(float puzzleTime) {
    GUI.DrawTexture(bedRect, Images.it.p3_FullBed);
    
    var face = Images.it.p4_FaceClosed;
    if (puzzleTime > 1.0f && puzzleTime < 1.5f) face = Images.it.p4_FaceLeftOpen; // "something"
    if (puzzleTime > 3.0f && puzzleTime < 3.5f) face = Images.it.p4_FaceRightOpen;// "tells me"
    GUI.DrawTexture(faceRect, face);
    
    var bubble = (dragger.location.x < 240 ? Images.it.p4_BubbleLeft : Images.it.p4_BubbleRight);
    GUI.DrawTexture(dragRect(), bubble);
  }

  public override void Update() {
    dragger.Update(dragRect(), Input.touches);
      
    if (dragger.location.x > 300) {
      setSolved();
    }
  }
  
  private Rect dragRect() {
    return new Rect(dragger.location.x, dragger.location.y, 120, 80);
  }
}

//-------------------------------------------
class Puzzle5 : Puzzle
{
  Rect goal = new Rect(310, 60, 50, 50);
  DeShaker deShaker = new DeShaker();
  
  float x, y;
  float solveTime;

  public override void OnGUI(float puzzleTime) {
    float size = 100;
    if (isSolved()) {
      size = 100 - (Time.time - solveTime)*100;
    }
    if (size > 0) {
      drawCentered(Images.it.img2, x, y, size);
    }
    
    if (!isSolved()) {
      GUI.Box(goal, "X");
    }
  }

  public override void Update() {
    if (goal.Contains(new Vector2(x, y))) {
      setSolved();
      solveTime = Time.time;
    }
    
    deShaker.shakyPoint(Input.acceleration.x, Input.acceleration.y);
    
    x = 250 + 120 * deShaker.firmY();
    y = 190 + 120 * deShaker.firmX();
  }
}