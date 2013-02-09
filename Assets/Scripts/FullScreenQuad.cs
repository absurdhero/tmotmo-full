using UnityEngine;

public class FullScreenQuad : MonoBehaviour {
	const float DEPTH = 10;
	public Texture2D texture;
	public bool tiled = false;
	
	ImageMaterial imageMaterial;

	public static FullScreenQuad create(object obj, string textureName) {
		var resourcePrefix = obj.GetType().ToString();
		return create(resourcePrefix + "/" + textureName);
	}

	public static FullScreenQuad create(string texturePath) {
		return create((Texture2D) Resources.Load(texturePath));
	}

	public static FullScreenQuad create(Texture2D texture) {
		var spriteObject = new GameObject(texture.name + " fullscreen");
		var quad = spriteObject.AddComponent<FullScreenQuad>();

		quad.texture = texture;

		quad.Start();

		return quad;
	}
	
	public void Start () {
		if (imageMaterial == null) {
			imageMaterial = createImageMaterial();
		}
		imageMaterial.RenderTo(gameObject);
	}
	
	private ImageMaterial createImageMaterial() {
		Camera cam = Camera.main;
	    var mesh = fullScreenMesh(cam);
		
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
		
		return img;
	}

	Mesh fullScreenMesh(Camera camera) {
		Mesh mesh = new Mesh();
		Vector3 tr = camera.ViewportToWorldPoint(new Vector3(1, 1, DEPTH));
		Vector3 tl = camera.ViewportToWorldPoint(new Vector3(0, 1, DEPTH));
		Vector3 br = camera.ViewportToWorldPoint(new Vector3(1, 0, DEPTH));
		Vector3 bl = camera.ViewportToWorldPoint(new Vector3(0, 0, DEPTH));
		mesh.vertices = new Vector3[] {tr, tl, br, bl};
		mesh.triangles = new int[] {0, 2, 1, 1, 2, 3};
		return mesh;
	}
	
	public void visible(bool enable) {
		this.gameObject.active = enable;
	}
}
