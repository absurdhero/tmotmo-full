using UnityEngine;

public class ImageMaterial {
	private Mesh mesh;
	private Material material;
	
	public ImageMaterial (GameObject gameObject, Mesh mesh) {
		this.mesh = mesh;
		var meshFilter = gameObject.GetComponent<MeshFilter>();
		if (meshFilter == null) {
			meshFilter = gameObject.AddComponent<MeshFilter>();
		}
		meshFilter.mesh = mesh;

		var meshRenderer = gameObject.GetComponent<MeshRenderer>();
		if (meshRenderer == null) {
			meshRenderer = gameObject.AddComponent<MeshRenderer>();
			material = new Material(Shader.Find("Unlit/Transparent"));
			meshRenderer.material = material;
			meshRenderer.receiveShadows = false;
			meshRenderer.castShadows = false;
		} else {
			material = meshRenderer.sharedMaterial;
		}
	}
	
	public void SetTexture(Texture2D texture) {
		material.mainTexture = texture;
	}
	
	public void SetUVTiled() {
		// set uv coordinates so the texture repeats
		Vector2[] uvs = new Vector2[mesh.vertices.Length];
        int i = 0;
        while (i < uvs.Length) {
            uvs[i] = new Vector2(mesh.vertices[i].x, mesh.vertices[i].y);
            i++;
        }
        mesh.uv = uvs;
	}

	public void SetUVStretched() {
		// set uv coordinates so the texture stretches across the surface
		Vector2[] uvs = new Vector2[4];
        uvs[0] = new Vector2(1, 1);
        uvs[1] = new Vector2(0, 1);
        uvs[2] = new Vector2(1, 0);
        uvs[3] = new Vector2(0, 0);
        mesh.uv = uvs;
	}
}
