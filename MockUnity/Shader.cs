using System;
using System.Runtime.CompilerServices;
namespace UnityEngine
{
	public sealed class Shader : Object
	{
        public string name;
		public bool isSupported;
		public int maximumLOD;
		public static int globalMaximumLOD;

		public static Shader Find (string name) { throw new InvalidOperationException(); }
		//internal static extern Shader FindBuiltin (string name);
		//public static extern void EnableKeyword (string keyword);
		//public static extern void DisableKeyword (string keyword);
		//public static void SetGlobalColor (string propertyName, Color color)
 		//public static void SetGlobalVector (string propertyName, Vector4 vec)
		//public static extern void SetGlobalFloat (string propertyName, float value);
		//public static extern void SetGlobalTexture (string propertyName, Texture tex);
		//public static void SetGlobalMatrix (string propertyName, Matrix4x4 mat)
		//public static extern void SetGlobalTexGenMode (string propertyName, TexGenMode mode);
		//public static extern void SetGlobalTextureMatrixName (string propertyName, string matrixName);
		//public static extern int PropertyToID (string name);
		//public static extern void WarmupAllShaders ();
	}
}

