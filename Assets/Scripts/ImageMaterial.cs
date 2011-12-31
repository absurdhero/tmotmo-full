using UnityEngine;

public class ImageMaterial {
	private Mesh mesh;
	public Material material { private set; get; }

	public ImageMaterial (GameObject gameObject, Mesh mesh)
		: this(gameObject, mesh, DefaultMaterial()) {
	}
	
	public ImageMaterial (GameObject gameObject, Mesh mesh, Material material) {
		this.mesh = mesh;
		var meshFilter = gameObject.GetComponent<MeshFilter>();
		if (meshFilter == null) {
			meshFilter = gameObject.AddComponent<MeshFilter>();
		}
		meshFilter.mesh = mesh;

		var meshRenderer = gameObject.GetComponent<MeshRenderer>();
		if (meshRenderer == null) {
			meshRenderer = gameObject.AddComponent<MeshRenderer>();

			meshRenderer.material = material;
			meshRenderer.receiveShadows = false;
			meshRenderer.castShadows = false;
		} else {
			material = meshRenderer.sharedMaterial;
		}
		
		this.material = material;
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
	
	public static Material DefaultMaterial() {
		var shader = Shader.Find("Mobile/Transparent/Vertex Color");
		if (shader == null) {
			Debug.LogError("Shader (" + shader.name + ") not found.");
		}
			
		Material material = new Material(shader);
		material.SetColor("_Color", new Color(0.5f, 0.5f, 0.5f, 1.0f));
		return material;
	}
}
