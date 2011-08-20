using System;

namespace UnityEngine
{

public struct Rect
{
  private float left, top, width, height;
  
  // Constructors
  public Rect(float left, float top, float width, float height) {this.left = left; this.top = top; this.width = width; this.height = height; }
//  public Rect(Rect source);

  // Methods
//  public static Rect MinMaxRect(float left, float top, float right, float bottom);
//  public virtual string ToString();
//  public string ToString(string format);
  public bool Contains(Vector2 point) { throw new InvalidOperationException(); }
//  public bool Contains(Vector3 point);
//  public virtual int GetHashCode();
//  public virtual bool Equals(object other);

  // Properties
  public float x { get { return top; } set { top = value; } }
  public float y { get { return left; } set { left = value; } }
//  public float width { get; set; }
//  public float height { get; set; }
//  public float left { get; }
//  public float right { get; }
//  public float top { get; }
//  public float bottom { get; }
//  public float xMin { get; set; }
//  public float yMin { get; set; }
//  public float xMax { get; set; }
//  public float yMax { get; set; }
}

}