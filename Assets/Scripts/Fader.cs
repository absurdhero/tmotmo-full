using UnityEngine;

public class Fader {
  float startTime;
  bool  active = false;
  float duration; // seconds to fade
  Color oldColor;

  public Fader(float duration) {
    this.duration = duration;
  }
  
  // after GUI.color is set, *all* drawing is "tinted" with that color
  public void fadeOn() {
    if (active) {
      oldColor = GUI.color;
      float fade = Mathf.Min(1f, (Time.time - startTime)/duration);
//      Debug.Log("Fade:" + fade + " " + Time.time + "-" + startTime);
      GUI.color = GUI.color * new Color(1f, 1f, 1f, 1f - fade);   // * for "layered" GUI.color effects
    }
  }

  public void fadeOff() {
    if (active) {
      GUI.color = oldColor;
    }
  }

  public void fadeNow() {
    active = true;
    startTime = Time.time;
  }
}
