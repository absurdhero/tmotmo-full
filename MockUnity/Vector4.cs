using System;
namespace UnityEngine
{
	public struct Vector4
	{
		public const float kEpsilon = 1E-05f;
		public float x;
		public float y;
		public float z;
		public float w;
		public float this [int index]
		{
			get
			{
				switch (index)
				{
				case 0:

					{
						return this.x;
					}
				case 1:

					{
						return this.y;
					}
				case 2:

					{
						return this.z;
					}
				case 3:

					{
						return this.w;
					}
				default:

					{
						throw new IndexOutOfRangeException ("Invalid Vector4 index!");
					}
				}
			}
			set
			{
				switch (index)
				{
				case 0:

					{
						this.x = value;
						break;
					}
				case 1:

					{
						this.y = value;
						break;
					}
				case 2:

					{
						this.z = value;
						break;
					}
				case 3:

					{
						this.w = value;
						break;
					}
				default:

					{
						throw new IndexOutOfRangeException ("Invalid Vector4 index!");
					}
				}
			}
		}

		public static Vector4 zero
		{
			get
			{
				return new Vector4 (0f, 0f, 0f, 0f);
			}
		}
		public static Vector4 one
		{
			get
			{
				return new Vector4 (1f, 1f, 1f, 1f);
			}
		}
		public Vector4 (float x, float y, float z, float w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}
		public Vector4 (float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = 0f;
		}
		public Vector4 (float x, float y)
		{
			this.x = x;
			this.y = y;
			this.z = 0f;
			this.w = 0f;
		}
		public void Set (float new_x, float new_y, float new_z, float new_w)
		{
			this.x = new_x;
			this.y = new_y;
			this.z = new_z;
			this.w = new_w;
		}

		public override int GetHashCode ()
		{
			return this.x.GetHashCode () ^ this.y.GetHashCode () << 2 ^ this.z.GetHashCode () >> 2 ^ this.w.GetHashCode () >> 1;
		}
		public override bool Equals (object other)
		{
			if (!(other is Vector4))
			{
				return false;
			}
			Vector4 vector = (Vector4)other;
			return this.x.Equals (vector.x) && this.y.Equals (vector.y) && this.z.Equals (vector.z) && this.w.Equals (vector.w);
		}

		public override string ToString ()
		{
			return string.Format ("({0:F1}, {1:F1}, {2:F1}, {3:F1})", new object[]
			{
				this.x,
				this.y,
				this.z,
				this.w
			});
		}
		public string ToString (string format)
		{
			return string.Format ("({0}, {1}, {2}, {3})", new object[]
			{
				this.x.ToString (format),
				this.y.ToString (format),
				this.z.ToString (format),
				this.w.ToString (format)
			});
		}
    }
}
