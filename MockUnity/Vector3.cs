namespace UnityEngine
{
public struct Vector3
{
  // Fields
  public static float kEpsilon = 0.0000000001f;
  public float x;
  public float y;
  public float z;

  // Constructors
  public Vector3(float x, float y, float z) {
    this.x = x;
    this.y = y;
    this.z = z;
  }
  public Vector3(float x, float y) {
    this.x = x;
    this.y = y;
    this.z = 0f;
  }

//
//  // Methods
//  public static Vector3 Lerp(Vector3 from, Vector3 to, float t);
//  public static Vector3 Slerp(Vector3 from, Vector3 to, float t);
//  public static void OrthoNormalize(ref ref Vector3 normal, ref ref Vector3 tangent);
//  public static void OrthoNormalize(ref ref Vector3 normal, ref ref Vector3 tangent, ref ref Vector3 binormal);
//  public static Vector3 MoveTowards(Vector3 current, Vector3 target, float maxDistanceDelta);
//  public static Vector3 RotateTowards(Vector3 current, Vector3 target, float maxRadiansDelta, float maxMagnitudeDelta);
//  public static Vector3 SmoothDamp(Vector3 current, Vector3 target, ref ref Vector3 currentVelocity, float smoothTime, float maxSpeed);
//  public static Vector3 SmoothDamp(Vector3 current, Vector3 target, ref ref Vector3 currentVelocity, float smoothTime);
//  public static Vector3 SmoothDamp(Vector3 current, Vector3 target, ref ref Vector3 currentVelocity, float smoothTime, float maxSpeed, float deltaTime);
//  public static Vector3 Scale(Vector3 a, Vector3 b);
//  public void Scale(Vector3 scale);
//  public static Vector3 Cross(Vector3 lhs, Vector3 rhs);
//  public virtual int GetHashCode();
//  public static Vector3 Reflect(Vector3 inDirection, Vector3 inNormal);
//  public static Vector3 Normalize(Vector3 value);
//  public void Normalize();
//  public virtual string ToString();
//  public string ToString(string format);
//  public static float Dot(Vector3 lhs, Vector3 rhs);
//  public static Vector3 Project(Vector3 vector, Vector3 onNormal);
//  public static Vector3 Exclude(Vector3 excludeThis, Vector3 fromThat);
//  public static float Angle(Vector3 from, Vector3 to);
//  public static float Distance(Vector3 a, Vector3 b);
//  public static Vector3 ClampMagnitude(Vector3 vector, float maxLength);
//  public static float Magnitude(Vector3 a);
//  public static float SqrMagnitude(Vector3 a);
//  public static Vector3 Min(Vector3 lhs, Vector3 rhs);
//  public static Vector3 Max(Vector3 lhs, Vector3 rhs);
//  public static float AngleBetween(Vector3 from, Vector3 to);
//
//  // Properties
//  public float Item[int index] { get; set; }
//  public Vector3 normalized { get; }
//  public float magnitude { get; }
  public static Vector3 zero = new Vector3(0f, 0f, 0f);
  public static Vector3 one = new Vector3(1f, 1f, 1f);
//  public static Vector3 forward { get; }
//  public static Vector3 back { get; }
//  public static Vector3 up { get; }
//  public static Vector3 down { get; }
//  public static Vector3 left { get; }
//  public static Vector3 right { get; }
//  public static Vector3 fwd { get; }

        public static float SqrMagnitude(Vector3 v) {
            return v.x * v.x + v.y * v.y + v.z * v.z;
        }

        public override bool Equals(object other) {
            if (!(other is Vector3)) return false;
            return this == (Vector3) other;
        }

		public override int GetHashCode ()
		{
			return (x + y + z).GetHashCode();
        }
        
		public static Vector3 operator + (Vector3 a, Vector3 b)
		{
			return new Vector3 (a.x + b.x, a.y + b.y, a.z + b.z);
		}
		public static Vector3 operator - (Vector3 a, Vector3 b)
		{
			return new Vector3 (a.x - b.x, a.y - b.y, a.z - b.z);
		}
		public static Vector3 operator - (Vector3 a)
		{
			return new Vector3 (-a.x, -a.y, -a.z);
		}
		public static Vector3 operator * (Vector3 a, float d)
		{
			return new Vector3 (a.x * d, a.y * d, a.z * d);
		}
		public static Vector3 operator * (float d, Vector3 a)
		{
			return new Vector3 (a.x * d, a.y * d, a.z * d);
		}
		public static Vector3 operator / (Vector3 a, float d)
		{
			return new Vector3 (a.x / d, a.y / d, a.z / d);
		}
		public static bool operator == (Vector3 lhs, Vector3 rhs)
		{
			return Vector3.SqrMagnitude (lhs - rhs) < kEpsilon;
		}
		public static bool operator != (Vector3 lhs, Vector3 rhs)
		{
			return Vector3.SqrMagnitude (lhs - rhs) >= kEpsilon;
		}
	}
  
}

