using System;

namespace UnityEngine
{
	public sealed class Camera
    {
        public static Camera main;
        public Vector3 WorldToScreenPoint(Vector3 v) { throw new InvalidOperationException(); }
        public Vector3 ScreenToWorldPoint(Vector3 v) { throw new InvalidOperationException(); }
        public Vector3 ViewportToWorldPoint(Vector3 v) { throw new InvalidOperationException(); }
    }
}

