using System;

namespace UnityEngine
{

public class Vector2
{
  const float EPSILON = 0.0000001f;

  // Fields
  public static float kEpsilon;
  public float x;
  public float y;

  // Constructors
  public Vector2(float x, float y) {throw new InvalidOperationException(); }

  // Methods
  public static Vector2 Lerp(Vector2 from, Vector2 to, float t) {throw new InvalidOperationException(); }
  public static Vector2 MoveTowards(Vector2 current, Vector2 target, float maxDistanceDelta) {throw new InvalidOperationException(); }
  public static Vector2 Scale(Vector2 a, Vector2 b) {throw new InvalidOperationException(); }
  public void Scale(Vector2 scale) {throw new InvalidOperationException(); }
  public void Normalize() {throw new InvalidOperationException(); }
//  public virtual string ToString() {throw new InvalidOperationException(); }
//  public string ToString(string format) {throw new InvalidOperationException(); }
//  public virtual int GetHashCode() {throw new InvalidOperationException(); }
//  public virtual bool Equals(object other) {throw new InvalidOperationException(); }
  public static float Dot(Vector2 lhs, Vector2 rhs) {throw new InvalidOperationException(); }
  public static float Angle(Vector2 from, Vector2 to) {throw new InvalidOperationException(); }
  public static float Distance(Vector2 a, Vector2 b) {throw new InvalidOperationException(); }
  public static Vector2 ClampMagnitude(Vector2 vector, float maxLength) {throw new InvalidOperationException(); }
  public static float SqrMagnitude(Vector2 a) {throw new InvalidOperationException(); }
  public float SqrMagnitude() {throw new InvalidOperationException(); }
  public static Vector2 Min(Vector2 lhs, Vector2 rhs) {throw new InvalidOperationException(); }
  public static Vector2 Max(Vector2 lhs, Vector2 rhs) {throw new InvalidOperationException(); }

  // Properties
//  public float Item[int index] { get { throw new InvalidOperationException(); } set; }
  public Vector2 normalized { get { throw new InvalidOperationException(); } }
  public float magnitude { get { throw new InvalidOperationException(); } }
  public float sqrMagnitude { get { throw new InvalidOperationException(); } }
  public static Vector2 zero { get { throw new InvalidOperationException(); } }
  public static Vector2 one { get { throw new InvalidOperationException(); } }
  public static Vector2 up { get { throw new InvalidOperationException(); } }
  public static Vector2 right { get { throw new InvalidOperationException(); } }

	public static Vector2 operator+ (Vector2 a, Vector2 b) {
		return new Vector2 (a.x + b.x, a.y + b.y);
	}
	
	public static Vector2 operator- (Vector2 a, Vector2 b) {
		return new Vector2 (a.x - b.x, a.y - b.y);
	}
	
	public static bool operator== (Vector2 lhs, Vector2 rhs) {
		return Vector2.SqrMagnitude(lhs - rhs) < EPSILON;
	}
	
	public static bool operator!= (Vector2 lhs, Vector2 rhs) {
		return Vector2.SqrMagnitude(lhs - rhs) >= EPSILON;
	}
		
	public override bool Equals (object other) {
		if (other.GetType() != typeof(Vector2)) return false;
		return this == (Vector2) other;
	}
}

}