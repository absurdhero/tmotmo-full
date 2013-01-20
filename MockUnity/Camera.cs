using System;

namespace UnityEngine
{
	public sealed class Camera : Behaviour
    {
        public static Camera main;
		public static Camera current;

		public float fov
		{
			get
			{
				throw new InvalidOperationException();
			}
			set
			{
				throw new InvalidOperationException();
			}
		}
		public float near
		{
			get
			{
				throw new InvalidOperationException();
			}
			set
			{
				throw new InvalidOperationException();
			}
		}
		public float far
		{
			get
			{
				throw new InvalidOperationException();
			}
			set
			{
				throw new InvalidOperationException();
			}
		}
		public float fieldOfView
		{
			get
			{
				throw new InvalidOperationException();
			}
			set
			{
				throw new InvalidOperationException();
			}
		}
		public float nearClipPlane
		{
			get
			{
				throw new InvalidOperationException();
			}
			set
			{
				throw new InvalidOperationException();
			}
		}
		public float farClipPlane
		{
			get
			{
				throw new InvalidOperationException();
			}
			set
			{
				throw new InvalidOperationException();
			}
		}
//		public RenderingPath renderingPath
//		{
//			get
//			{
//				throw new InvalidOperationException();
//			}
//			set
//			{
//				throw new InvalidOperationException();
//			}
//		}
//		public RenderingPath actualRenderingPath
//		{
//			get
//			{
//				throw new InvalidOperationException();
//			}
//			set
//			{
//				throw new InvalidOperationException();
//			}
//		}
		public bool hdr
		{
			get
			{
				throw new InvalidOperationException();
			}
			set
			{
				throw new InvalidOperationException();
			}
		}
		public float orthographicSize
		{
			get
			{
				throw new InvalidOperationException();
			}
			set
			{
				throw new InvalidOperationException();
			}
		}
		public bool orthographic
		{
			get
			{
				throw new InvalidOperationException();
			}
			set
			{
				throw new InvalidOperationException();
			}
		}
//		public TransparencySortMode transparencySortMode
//		{
//			get
//			{
//				throw new InvalidOperationException();
//			}
//			set
//			{
//				throw new InvalidOperationException();
//			}
//		}
		public bool isOrthoGraphic
		{
			get
			{
				throw new InvalidOperationException();
			}
			set
			{
				throw new InvalidOperationException();
			}
		}
		public float depth
		{
			get
			{
				throw new InvalidOperationException();
			}
			set
			{
				throw new InvalidOperationException();
			}
		}
		public float aspect
		{
			get
			{
				throw new InvalidOperationException();
			}
			set
			{
				throw new InvalidOperationException();
			}
		}
		public int cullingMask
		{
			get
			{
				throw new InvalidOperationException();
			}
			set
			{
				throw new InvalidOperationException();
			}
		}
		public Color backgroundColor
		{
			get
			{
				throw new InvalidOperationException();
			}
			set
			{
				throw new InvalidOperationException();
			}
		}
		public Rect rect
		{
			get
			{
				throw new InvalidOperationException();
			}
			set
			{
				throw new InvalidOperationException();
			}
		}
		public Rect pixelRect
		{
			get
			{
				throw new InvalidOperationException();
			}
			set
			{
				throw new InvalidOperationException();
			}
		}
		public float pixelHeight
		{
			get
			{
				throw new InvalidOperationException();
			}
			set
			{
				throw new InvalidOperationException();
			}
		}
		public float pixelWidth
		{
			get
			{
				throw new InvalidOperationException();
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		public CameraClearFlags clearFlags
		{
			get
			{
				throw new InvalidOperationException();
			}
			set
			{
				throw new InvalidOperationException();
			}
		}

		public float GetScreenWidth () { throw new InvalidOperationException(); }
		public float GetScreenHeight () { throw new InvalidOperationException(); }
        public Vector3 WorldToScreenPoint(Vector3 v) { throw new InvalidOperationException(); }
        public Vector3 ScreenToWorldPoint(Vector3 v) { throw new InvalidOperationException(); }
        public Vector3 ViewportToWorldPoint(Vector3 v) { throw new InvalidOperationException(); }
    }
}

