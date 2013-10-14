using System;

namespace UnityEngine
{
	public class Screen {
		public static int width { get; private set; }
		public static int height { get; private set; }
		
		public static bool autorotateToPortrait { get; set; }
		public static bool autorotateToPortraitUpsideDown { get; set; }
		public static bool autorotateToLandscapeRight { get; set; }
		public static bool autorotateToLandscapeLeft { get; set; }
	}
}

