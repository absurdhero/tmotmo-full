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
		public Color (float r, float g, float b, float a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}
  
		public Color (float r, float g, float b) : this(r, g, b, 1.0f)
		{
		}
  

		// Methods
//  public virtual string ToString();
//  public string ToString(string format);
//  public virtual int GetHashCode();
//  public virtual bool Equals(object other);
//  public static Color Lerp(Color a, Color b, float t);

		// Properties
		public static Color red {
			get {
				return new Color (1f, 0f, 0f, 1f);
			}
		}

		public static Color green {
			get {
				return new Color (0f, 1f, 0f, 1f);
			}
		}

		public static Color blue {
			get {
				return new Color (0f, 0f, 1f, 1f);
			}
		}

		public static Color white {
			get {
				return new Color (1f, 1f, 1f, 1f);
			}
		}

		public static Color black {
			get {
				return new Color (0f, 0f, 0f, 1f);
			}
		}

		public static Color yellow {
			get {
				return new Color (1f, 0.921568632f, 0.0156862754f, 1f);
			}
		}

		public static Color cyan {
			get {
				return new Color (0f, 1f, 1f, 1f);
			}
		}

		public static Color magenta {
			get {
				return new Color (1f, 0f, 1f, 1f);
			}
		}

		public static Color gray {
			get {
				return new Color (0.5f, 0.5f, 0.5f, 1f);
			}
		}

		public static Color grey {
			get {
				return new Color (0.5f, 0.5f, 0.5f, 1f);
			}
		}

		public static Color clear {
			get {
				return new Color (0f, 0f, 0f, 0f);
			}
		}

//  public float grayscale { get; }
//  public float Item[int index] { get; set; }
	}

}