using System;
namespace UnityEngine
{
	public struct Vector4
	{
		public const float kEpsilon = 0.0000001f;
		public float x;
		public float y;
		public float z;
		public float w;

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
			x = new_x;
			y = new_y;
			z = new_z;
			w = new_w;
		}

		public override int GetHashCode ()
		{
			return (x.GetHashCode() * 2) ^ y.GetHashCode() ^ z.GetHashCode() ^ w.GetHashCode();
		}
		
		public override bool Equals (object other)
		{
			if (!(other is Vector4)) return false;
			Vector4 v = (Vector4) other;
			return x == v.x && y == v.y && z == v.z && w == v.w;
		}

		public override string ToString ()
		{
			return string.Format ("({0:F1}, {1:F1}, {2:F1}, {3:F1})", x, y, z, w );
		}

		public string ToString (string format)
		{
			return string.Format ("({0}, {1}, {2}, {3})",
				x.ToString(format), y.ToString(format), z.ToString(format), w.ToString(format)
			);
		}
    }
}
