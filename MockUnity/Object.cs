using System;

namespace UnityEngine
{
	public class Object
	{
		public string name;
		private static  bool CompareBaseObjects (Object lhs, Object rhs)
			{ throw new InvalidOperationException(); }
		public int GetInstanceID () { throw new InvalidOperationException(); }
		//public static Object Instantiate (Object original, Vector3 position, Quaternion rotation)
		//	 { throw new InvalidOperationException(); }
		public static Object Instantiate (Object original) { throw new InvalidOperationException(); }
		public static  void Destroy (Object obj, float t)
			 { throw new InvalidOperationException(); }
		public static void Destroy (Object obj) { throw new InvalidOperationException(); }
		public static  void DestroyImmediate (Object obj, bool allowDestroyingAssets)
			 { throw new InvalidOperationException(); }
		public static void DestroyImmediate (Object obj) { throw new InvalidOperationException(); }
		public static  Object[] FindObjectsOfType (Type type)
			{ throw new InvalidOperationException(); }
		public static Object FindObjectOfType (Type type)
			{ throw new InvalidOperationException(); }
		public static  void DontDestroyOnLoad (Object target)
			{ throw new InvalidOperationException(); }
		public static  void DestroyObject (Object obj, float t)
			{ throw new InvalidOperationException(); }
		public static void DestroyObject (Object obj) { throw new InvalidOperationException(); }
		public static  Object[] FindSceneObjectsOfType (Type type)
			{ throw new InvalidOperationException(); }
		public static  Object[] FindObjectsOfTypeIncludingAssets (Type type)
			 { throw new InvalidOperationException(); }
	}
}

