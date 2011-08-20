using System;
using System.Collections.Generic;
using UnityEngine;

//Cycles through a set of images, for animations
public class Cycler {
  float startTime;
  float timeToShow;
  IList<Texture2D> images;
  
  public Cycler(float timeToShow, IList<Texture2D> images) {
    this.timeToShow = timeToShow;
    this.images = images;
    startTime = Time.time;
  }
  
  public void draw(Rect rect) {
    int index = (int) ((Time.time - startTime)/timeToShow);
    GUI.DrawTexture(rect, images[index % images.Count]);
  }
}