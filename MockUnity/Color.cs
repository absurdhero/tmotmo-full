namespace UnityEngine
{
//[DefaultMember("Item")]
public struct Color
{
  // Fields
  public float r;
  public float g;
  public float b;
  public float a;

  // Constructors
  public Color(float r, float g, float b, float a) {
    this.r = r;
    this.g = g;
    this.b = b;
    this.a = a;
  }
  
  public Color(float r, float g, float b) : this(r, g, b, 1.0f) {}
  

  // Methods
//  public virtual string ToString();
//  public string ToString(string format);
//  public virtual int GetHashCode();
//  public virtual bool Equals(object other);
//  public static Color Lerp(Color a, Color b, float t);

  // Properties
//  public static Color red { get; }
//  public static Color green { get; }
//  public static Color blue { get; }
//  public static Color white { get; }
//  public static Color black { get; }
//  public static Color yellow { get; }
//  public static Color cyan { get; }
//  public static Color magenta { get; }
//  public static Color gray { get; }
//  public static Color grey { get; }
//  public static Color clear { get; }
//  public float grayscale { get; }
//  public float Item[int index] { get; set; }
}

}