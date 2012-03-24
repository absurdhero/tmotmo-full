using System;

namespace UnityEngine
{
	public class Transform
	{
        public Vector3 position;
		public Quaternion rotation;
		public Vector3 scale;
		
		public Vector3 localPosition;
		public Quaternion localRotation;
		public Vector3 localScale;
		
		public Transform parent;

        public Vector3 InverseTransformPoint(Vector3 v) { throw new InvalidOperationException(); }
		public void Translate(Vector3 pos) { throw new InvalidOperationException(); }
		public void Translate(float x, float y, float z) { throw new InvalidOperationException(); }

		public void Rotate(Vector3 v) { throw new InvalidOperationException(); }
    }
}
