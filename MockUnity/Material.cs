using System;
using System.Runtime.CompilerServices;
namespace UnityEngine
{
	public class Material : Object
	{
		public Shader shader
		{
			get;
			set;
		}
		public Color color
		{
			get;
			set;
		}
		public Texture mainTexture
		{
			get;
			set;
		}
		public Vector2 mainTextureOffset
		{
			get;
			set;
		}
		public Vector2 mainTextureScale
		{
			get;
			set;
		}
		public Material (string contents)
		{
		}
		public Material (Shader shader)
		{
		}
		public Material (Material source)
		{
		}
		public void SetColor (string propertyName, Color color)
		{
            throw new InvalidOperationException();
		}
		public Color GetColor (string propertyName)
        {
            throw new InvalidOperationException();
        }
		public void SetVector (string propertyName, Vector4 vector)
		{
            throw new InvalidOperationException();
		}
		public Vector4 GetVector (string propertyName)
		{
            throw new InvalidOperationException();
		}
		public void SetTexture (string propertyName, Texture texture) { throw new InvalidOperationException(); }
		public Texture GetTexture (string propertyName) { throw new InvalidOperationException(); }
		//public void SetTextureOffset (string propertyName, Vector2 offset);
		//public Vector2 GetTextureOffset (string propertyName);
		//public void SetTextureScale (string propertyName, Vector2 scale);
		//public Vector2 GetTextureScale (string propertyName);
		//public void SetMatrix (string propertyName, Matrix4x4 matrix);
		//public extern Matrix4x4 GetMatrix (string propertyName);
		//public extern void SetFloat (string propertyName, float value);
		//public extern float GetFloat (string propertyName);
		//public extern bool HasProperty (string propertyName);
		//public extern string GetTag (string tag, bool searchFallbacks, string defaultValue);
		//public string GetTag (string tag, bool searchFallbacks);
		//public extern void Lerp (Material start, Material end, float t);
		//public extern bool SetPass (int pass);
	}
}

