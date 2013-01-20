using System;

namespace UnityEngine
{
	public class GUIElement : Behaviour
	{
		public GUIElement ()
		{
		}

		public Rect GetScreenRect(Camera camera) { throw new InvalidOperationException(); }
		public Rect GetScreenRect() { throw new InvalidOperationException(); }
	}
}

