using System;
using System.Runtime.CompilerServices;
namespace UnityEngine
{
	public sealed class Mesh : Object
	{
		public Vector3[] vertices
		{
			get;
			set;
		}
		public Vector3[] normals
		{
			get;
			set;
		}
		public Vector4[] tangents
		{
			get;
			set;
		}
		public Vector2[] uv
		{
			get;
			set;
		}
		public Vector2[] uv2
		{
			get;
			set;
		}
		public Vector2[] uv1
		{
			get
			{
				return this.uv2;
			}
			set
			{
				this.uv2 = value;
			}
		}
		public Bounds bounds;

		public Color[] colors
		{
			get;
			set;
		}
		public int[] triangles
		{
			get;
			set;
		}
		public Mesh ()
		{
		}
	}
}

