using UnityEngine;

/* The Sprite class draws a texture on a quad that is sized so that each
 * pixel of the texture is displayed as exactly one pixel on the screen.
 * 
 * The calculation is made based on the first texture. The mesh is not
 * resized if the texture is changed to one of a different size.
 */
public class Sprite : MonoBehaviour {
	public Texture2D[] textures;
	public int height = 0;
	public int width = 0;
	
	private int texture_index = 0;
	private bool texture_dirty = true;

	private Mesh mesh;
	private ImageMaterial imageMaterial;
	
	public Material material { get { return imageMaterial.material; } }
	
	void Awake() {
		Camera cam = Camera.main;
		// if width/height were not specified, base it on the first texture
		if (this.width == 0 || this.height == 0) {
			width = textures[0].width;
			height = textures[0].height;
		}

		// calculate world points that allow the texture width/height to
		// display pixel perfect on the screen
		Vector3 pos = cam.WorldToScreenPoint(gameObject.transform.position);
		Vector3 tr = cam.ScreenToWorldPoint(new Vector3(pos.x + width, pos.y + height, pos.z));
		Vector3 tl = cam.ScreenToWorldPoint(new Vector3(pos.x, pos.y + height, pos.z));
		Vector3 br = cam.ScreenToWorldPoint(new Vector3(pos.x + width, pos.y, pos.z));
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
		imageMaterial.SetUVTiled();
	}
	
	void Update() {
		if (texture_dirty) {
			imageMaterial.SetTexture(textures[texture_index]);
			texture_dirty = false;
		}
	}
	
	public bool LastTexture() {
		return texture_index == (textures.Length - 1);
	}
	
	public virtual void DrawNextFrame() {
		nextFrame();
		Animate();
	}
	
	public void nextFrame() {
		texture_index = (texture_index + 1) % textures.Length;
	}
	
	public void Animate() {
		texture_dirty = true;
	}
	
	public void setFrame(int index) {
		texture_index = index;
	}
	
	///  <summary>
	/// Center of sprite in World space
	/// </summary>
	public Vector3 Center() {
		Vector3 sum = Vector3.zero;
		foreach(Vector3 v in mesh.vertices) {
			sum += v;
		}
		return sum / (float) mesh.vertices.Length;
	}
	
	public int PixelWidth() {
		return width;
	}
	
	public int PixelHeight() {
		return height;
	}
	
	public Rect ScreenRect() {
		Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
		return new Rect(pos.x, pos.y, width, height);
	}
	
	public Vector2 ScreenCenter() {
		Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
		return new Vector2(pos.x + width / 2, pos.y + height / 2);
	}

	public bool Contains(Vector2 position) {
		return ScreenRect().Contains(position);
	}
	
	public bool Contains(Vector3 position) {
		return ScreenRect().Contains(new Vector2(position.x, position.y));
	}

	public void setScreenPosition(int x, int y) {
		Vector3 pos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		pos.x = x;
		pos.y = y;
		gameObject.transform.position = Camera.main.ScreenToWorldPoint(pos);
	}
	
	/* In viewport space, 0 and 1 are the edges of the screen. */
	public void setCenterToViewportCoord(Camera camera, float x, float y) {
		var layoutpos = Camera.main.ViewportToWorldPoint(new Vector3(x, y, 0.0f));
		gameObject.transform.position = new Vector3(layoutpos.x, layoutpos.y, gameObject.transform.position.z) - Center();
	}
}
