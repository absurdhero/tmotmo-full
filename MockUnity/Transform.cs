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

		public void DetachChildren() { throw new InvalidOperationException(); }

        public Vector3 InverseTransformPoint(Vector3 v) { throw new InvalidOperationException(); }
		public void Translate(Vector3 pos) { throw new InvalidOperationException(); }
		public void Translate(float x, float y, float z) { throw new InvalidOperationException(); }

		public void Rotate(Vector3 eulerAngles) { throw new InvalidOperationException(); }
		public void Rotate(Vector3 eulerAngles, Space relativeTo) { throw new InvalidOperationException(); }
		public void Rotate(float xAngle, float yAngle, float zAngle) { throw new InvalidOperationException(); }
		public void Rotate(float xAngle, float yAngle, float zAngle, Space relativeTo) { throw new InvalidOperationException(); }
		public void Rotate(Vector3 axis, float angle) { throw new InvalidOperationException(); }
		public void Rotate(Vector3 axis, float angle, Space relativeTo) { throw new InvalidOperationException(); }

		public Vector3 TransformPoint(Vector3 v) { throw new InvalidOperationException(); }
    }
}
