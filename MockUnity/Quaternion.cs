using System;

namespace UnityEngine {
	public struct Quaternion {
		public float x;
		public float y;
		public float z;
		public float w;

		public Quaternion(float x, float y, float z, float w) {
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}
		
		public Vector3 eulerAngles {
			get {
				throw new InvalidOperationException(); 
			}
		}
		
		public override bool Equals (object other)
		{
			if (!(other is Quaternion)) return false;
			var v = (Quaternion) other;
			return x == v.x && y == v.y && z == v.z && w == v.w;
		}
		
		public override int GetHashCode ()
		{
			return (x.GetHashCode() * 2) ^ y.GetHashCode() ^ z.GetHashCode() ^ w.GetHashCode();
		}
	}
}

