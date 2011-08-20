using System;
using UnityEngine;

public class Dragger
{
  public Vector2 location;
  bool dragging;
  bool fixedVertical;
    
  public Dragger(float startX, float startY, bool horizontal = false) {
    location = new Vector2(startX, startY);
    dragging = false;
    fixedVertical = horizontal;
  }
  
  public static Dragger horizontal(float startX, float startY) {
    return new Dragger(startX, startY, true);
  }
  
  public void Update(Rect dragRect, Touch[] touches) {
    if (Input.touchCount == 0) {
      dragging = false;
    }
    foreach (var touch in touches) {
      if (Puzzle.containsTouch(dragRect, touch)) {
        dragging = true;
      }
        
      if (dragging && touch.phase == TouchPhase.Moved) {
        location.x += touch.deltaPosition.x;
        if (!fixedVertical) {
          location.y -= touch.deltaPosition.y;
        }
      }
    }
  }
}

