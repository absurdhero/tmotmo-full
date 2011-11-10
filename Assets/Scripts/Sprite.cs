using UnityEngine;

/* The Sprite class draws a texture on a quad that is sized so that each
 * pixel of the texture is displayed as exactly one pixel on the screen.
 * 
 * The calculation is made based on the first texture. The mesh is not
 * resized if the texture is changed to one of a different size.
 */
public class Sprite : MonoBehaviour {
	public Texture2D[] textures;
	
	private int texture_index = 0;
	private bool texture_dirty = true;

	private Mesh mesh;
	private ImageMaterial imageMaterial;
	
	void Awake() {
		Camera cam = Camera.main;
		// calculate world points that allow the texture width/height to
		// display pixel perfect on the screen
		Texture2D texture = textures[0];
		Vector3 pos = cam.WorldToScreenPoint(gameObject.transform.position);
		Vector3 tr = cam.ScreenToWorldPoint(new Vector3(pos.x + texture.width, pos.y + texture.height, pos.z));
		Vector3 tl = cam.ScreenToWorldPoint(new Vector3(pos.x, pos.y + texture.height, pos.z));
		Vector3 br = cam.ScreenToWorldPoint(new Vector3(pos.x + texture.width, pos.y, pos.z));
		Vector3 bl = cam.ScreenToWorldPoint(pos);

		mesh = new Mesh();
		// These corner points are in World space. The mesh needs to use local coordinates
		// so we fix that with InverseTransformPoint.
		mesh.vertices = new Vector3[] {
			transform.InverseTransformPoint(tr),
			transform.InverseTransformPoint(tl),
			transform.InverseTransformPoint(br),
			transform.InverseTransformPoint(bl)
		};
		mesh.triangles = new int[] {0, 2, 1, 1, 2, 3};		
	}

	void Start () {
		imageMaterial = new ImageMaterial(gameObject, mesh);
		imageMaterial.SetUVStretched();
	}
	
	void Update() {
		if (texture_dirty) {
			imageMaterial.SetTexture(textures[texture_index]);
			texture_dirty = false;
		}
	}
	
	public void NextTexture() {
		texture_index = (texture_index + 1) % textures.Length;
		texture_dirty = true;
	}
	
	public Vector3 Center() {
		Vector3 sum = Vector3.zero;
		foreach(Vector3 v in mesh.vertices) {
			sum += v;
		}
		return sum / (float) mesh.vertices.Length;
	}
	
	void OnDrawGizmos() {
		if (textures == null || textures.Length == 0) return;
		if (mesh == null) {
			Awake();
			Start();
		}
		GetComponent<MeshRenderer>().sharedMaterial.mainTexture = textures[0];
	}
	
	void OnDrawGizmosSelected() {
		OnDrawGizmos();
		if (textures.Length > 0) {
	        Gizmos.color = Color.yellow;
        	Gizmos.DrawWireCube(transform.position + mesh.bounds.size / 2.0f, mesh.bounds.size);
		}
    }
}
