using System;
namespace UnityEngine
{
    public abstract class Resources
    {
        // Constructors
        public Resources () {}

        // Methods
        public static Object[] FindObjectsOfTypeAll (Type type) { throw new InvalidOperationException(); }
        public static Object Load (string path) { throw new InvalidOperationException(); }
        public static Object Load (string path, Type type) { throw new InvalidOperationException(); }
        public static Object[] LoadAll (string path, Type type) { throw new InvalidOperationException(); }
        public static Object[] LoadAll (string path) { throw new InvalidOperationException(); }
        public static Object GetBuiltinResource (Type type, string path) { throw new InvalidOperationException(); }
        public static Object LoadAssetAtPath (string assetPath, Type type) { throw new InvalidOperationException(); }
        public static AsyncOperation UnloadUnusedAssets () { throw new InvalidOperationException(); }
    }

    public interface AsyncOperation {
    }
}
