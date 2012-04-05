using UnityEngine;

public class FullScreenQuad : MonoBehaviour {
	public Texture2D texture;
	public bool tiled = false;
	
	void Start () {
		Camera cam = Camera.main;
		Mesh mesh = new Mesh();
		float z = gameObject.transform.position.z;
		Vector3 tr = cam.ViewportToWorldPoint(new Vector3(1, 1, z));
		Vector3 tl = cam.ViewportToWorldPoint(new Vector3(0, 1, z));
		Vector3 br = cam.ViewportToWorldPoint(new Vector3(1, 0, z));
		Vector3 bl = cam.ViewportToWorldPoint(new Vector3(0, 0, z));
		mesh.vertices = new Vector3[] {tr, tl, br, bl};
		mesh.triangles = new int[] {0, 2, 1, 1, 2, 3};
		//mesh.triangles = new int[] {1, 0, 3, 3, 0, 2};
		
		var shader = Shader.Find("Mobile/Unlit (Supports Lightmap)");
		if (shader == null) {
			Debug.LogError("Shader (" + shader.name + ") not found.");
		}

		var material = new Material(shader);
		material.SetColor("_Color", new Color(0.5f, 0.5f, 0.5f, 1.0f));

		ImageMaterial img = new ImageMaterial(mesh, material);
		img.SetTexture(texture);

		if(tiled)
			img.SetUVTiled();
		else
			img.SetUVStretched();

		img.RenderTo(gameObject);
	}
}
