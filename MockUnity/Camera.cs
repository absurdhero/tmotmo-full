using System;

namespace UnityEngine
{
	public sealed class Camera : Behaviour
    {
        public static Camera main;
		public static Camera current;
		public float pixelHeight;
		public float pixelWidth;
		public float orthographicSize;
		public float GetScreenWidth () { throw new InvalidOperationException(); }
		public float GetScreenHeight () { throw new InvalidOperationException(); }
        public Vector3 WorldToScreenPoint(Vector3 v) { throw new InvalidOperationException(); }
        public Vector3 ScreenToWorldPoint(Vector3 v) { throw new InvalidOperationException(); }
        public Vector3 ViewportToWorldPoint(Vector3 v) { throw new InvalidOperationException(); }
    }
}

