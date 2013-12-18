using UnityEngine;

/* The Sprite class draws a texture on a quad that is sized so that each
 * pixel of the texture is displayed as exactly one pixel on the screen.
 * 
 * The calculation is made based on the first texture. The mesh is not
 * resized if the texture is changed to one of a different size.
 */
public class Sprite : MonoBehaviour {
	public static Sprite create(object obj, params string[] textureNames) {
		return createFor<Sprite>(obj, textureNames);
	}

	public static Sprite create(params string[] texturePaths) {
		return createFor<Sprite>(texturePaths);
	}
	
	public static Sprite create(params Texture2D[] textures) {
		return createFor<Sprite>(textures);
	}
	
	// these use generics so that subclasses of Sprite can reuse these factory methods
	public static T createFor<T>(object obj, params string[] textureNames) where T: Sprite {
		var resourcePrefix = obj.GetType().ToString();
		string[] texturePaths = new string[textureNames.Length];
		for (int i = 0; i < textureNames.Length; i++) {
			texturePaths[i] = resourcePrefix + "/" + textureNames[i];
	    }
		return createFor<T>(texturePaths);
	}

	public static T createFor<T>(params string[] texturePaths) where T : Sprite {
		Texture2D[] textures = new Texture2D[texturePaths.Length];
		for (int i = 0; i < texturePaths.Length; i++) {
			textures[i] = (Texture2D) Resources.Load(texturePaths[i]);
		}
		return createFor<T>(textures);
	}

	public static T createFor<T>(params Texture2D[] textures) where T : Sprite {
		var spriteObject = new GameObject(textures[0].name);
		var sprite = spriteObject.AddComponent<T>();

		sprite.height = textures[0].height;
		sprite.width = textures[0].width;
		sprite.textures = textures;

		sprite.Awake();
		sprite.Start();

		return sprite;
	}

	public Texture2D[] textures;
	public int height = 0;
	public int width = 0;
	
	private int texture_index = 0;
	private bool texture_dirty = true;

	private Mesh mesh;
	public ImageMaterial imageMaterial { get; private set; }
	
	public Material material { get { return imageMaterial.material; } }
	
	/// Destroys the underlying GameObject
	public static void Destroy(Sprite sprite) {
		GameObject.Destroy(sprite.gameObject);
	}
	
	///Avoid creating a new mesh and material by reusing existing ones
	public void InitializeWithExisting(Mesh mesh, Material material) {
		this.mesh = mesh;
		imageMaterial = new ImageMaterial(mesh, material);
	}
	
	public void Awake() {
		if (textures == null) return;

		gameObject.name =  textures[0].name + " " + this.GetType().Name.ToLower();

		// if width/height were not specified, base it on the first texture
		if (this.width == 0 || this.height == 0) {
			width = textures[0].width;
			height = textures[0].height;
		}
		
		if (mesh == null) mesh = createMesh(Camera.main);
	}

	virtual protected Mesh createMesh(Camera camera) {
		// calculate world points that allow the texture width/height to
		// display pixel perfect on the screen
		Vector3 pos = camera.WorldToScreenPoint(gameObject.transform.position);
		Vector3 tr = camera.ScreenToWorldPoint(new Vector3(pos.x + width, pos.y + height, pos.z));
		Vector3 tl = camera.ScreenToWorldPoint(new Vector3(pos.x, pos.y + height, pos.z));
		Vector3 br = camera.ScreenToWorldPoint(new Vector3(pos.x + width, pos.y, pos.z));
		Vector3 bl = camera.ScreenToWorldPoint(pos);

		Mesh mesh = new Mesh();
		// These corner points are in World space. The mesh needs to use local coordinates
		// so we fix that with InverseTransformPoint.
		mesh.vertices = new Vector3[] {
			transform.InverseTransformPoint(tr),
			transform.InverseTransformPoint(tl),
			transform.InverseTransformPoint(br),
			transform.InverseTransformPoint(bl)
		};
		mesh.triangles = new int[] {0, 2, 1, 1, 2, 3};

		return mesh;
	}

	public void Start () {
		if (imageMaterial == null) {
			imageMaterial = new ImageMaterial(mesh);
			imageMaterial.SetUVTiled();
			imageMaterial.SetTexture(textures[0]);
		}
		imageMaterial.RenderTo(gameObject);
	}
	
	void Update() {
		if (texture_dirty) {
			imageMaterial.SetTexture(textures[texture_index]);
			texture_dirty = false;
		}
	}
	
	public void visible(bool enable) {
		this.gameObject.SetActive(enable);
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
	
	/// Center of sprite in World space
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

	/// The screen rect based on texture dimensions
	public Rect ScreenRect() {
		return ScreenRect(Camera.main);
	}

	public Rect ScreenRect(Camera camera) {
		Vector3 pos = camera.WorldToScreenPoint(transform.position);
		return new Rect(pos.x, pos.y, width, height);
	}

	/// Takes scale and rotation into account
	public Rect WorldScreenRect(Camera camera) {
		Vector3 pos = camera.WorldToScreenPoint(transform.position);
		Vector3 outterPos = transform.TransformPoint(transform.position + new Vector3(width, height, transform.position.z));
		return new Rect(pos.x, pos.y, outterPos.x - pos.x, outterPos.y - pos.y);
	}

	public Vector2 ScreenCenter() {
		Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
		return new Vector2(pos.x + width / 2, pos.y + height / 2);
	}

	public bool Contains(Camera camera, Vector2 position) {
		return ScreenRect(camera).Contains(position);
	}
	
	public bool Contains(Camera camera, Vector3 position) {
		return ScreenRect(camera).Contains(new Vector2(position.x, position.y));
	}

	public void setScreenPosition(float x, float y) {
		Vector3 pos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		pos.x = x;
		pos.y = y;
		gameObject.transform.position = Camera.main.ScreenToWorldPoint(pos);
	}
	
	public void setScreenPosition(int x, int y) {
		setScreenPosition((float) x, (float) y);
	}
	
	public void setScreenPosition(Vector3 position) {
		setScreenPosition(position.x, position.y);
	}
	
	public Vector3 getScreenPosition() {
		return Camera.main.WorldToScreenPoint(gameObject.transform.position);
	}
	
	/* In viewport space, 0 and 1 are the edges of the screen. */
	public void setCenterToViewportCoord(float x, float y) {
		var layoutpos = snapToPixel(Camera.main.ViewportToWorldPoint(new Vector3(x, y, 0.0f)));
		gameObject.transform.position = new Vector3(layoutpos.x, layoutpos.y, gameObject.transform.position.z) - Center();
	}
	
	public void setWorldPosition(float x, float y, float z) {
		setWorldPosition(new Vector3(x, y, z));
	}

	public void setWorldPosition(Vector3 pos) {
		gameObject.transform.position = snapToPixel(pos);
	}
	
	public void setDepth(float z) {
		gameObject.transform.Translate(Vector3.back * z);
	}
	
	public void move(Vector3 pixels) {
		setScreenPosition(getScreenPosition() + pixels);
	}

	public void move(float x, float y) {
		move(new Vector3(x, y));
	}

	public void smoothMove(Vector3 pixels) {
		move(pixels * Time.deltaTime);
	}

	public void smoothMove(float x, float y) {
		move(new Vector3(x * Time.deltaTime, y * Time.deltaTime));
	}

	public GameObject createPivotOnTopLeftCorner() {
		var parent = new GameObject("Parent of " + gameObject.name);
		copyTransformTo(parent);

		// translate the parent 2 times the sprite height.
		// implicitly translate the sprite in the opposite direction by the same amount.
		parent.transform.Translate(0f, worldHeight, 0f);
		gameObject.transform.parent = parent.transform;
		
		return parent;		
	}

	public GameObject createPivotOnCenter() {
		var parent = new GameObject("Parent of " + gameObject.name);
		copyTransformTo(parent);

		// translate the parent 2 times the sprite height.
		// implicitly translate the sprite in the opposite direction by the same amount.
		parent.transform.Translate(worldWidth / 2f, worldHeight / 2f, 0f);
		gameObject.transform.parent = parent.transform;
		
		return parent;		
	}
	
	public GameObject createPivotOnBottomRightCorner() {
		var parent = new GameObject("Parent of " + gameObject.name);
		copyTransformTo(parent);

		// translate the parent 2 times the sprite width.
		// implicitly translate the sprite in the opposite direction by the same amount.
		parent.transform.Translate(worldWidth, 0f, 0f);
		gameObject.transform.parent = parent.transform;

		return parent;
	}

	public GameObject createPivotOnBottomLeftCorner() {
		var parent = new GameObject("Parent of " + gameObject.name);
		copyTransformTo(parent);
		gameObject.transform.parent = parent.transform;
		
		return parent;
	}
	
	private float worldHeight {
		// This is multiplied by two because orthographicSize is half the screen height
		get { return (float) height / Camera.main.pixelHeight * Camera.main.orthographicSize * 2.0f; }
	}
	
	private float worldWidth {
		// This is multiplied by two because orthographicSize is half the screen height
		get { return (float) width / Camera.main.pixelWidth * Camera.main.orthographicSize * 2.0f; }
	}

	private Vector3 snapToPixel(Vector3 pos) {
		Vector3 newpos;
		float pixelRatio = (Camera.main.orthographicSize * 2) / Camera.main.pixelHeight;
		
		newpos.x = Mathf.Round(pos.x / pixelRatio) * pixelRatio;
		newpos.y = Mathf.Round(pos.y / pixelRatio) * pixelRatio;
		newpos.z = pos.z;
		return newpos;
	}
	
	private void copyTransformTo(GameObject obj) {
		obj.transform.parent = gameObject.transform.parent;
		obj.transform.localRotation = gameObject.transform.localRotation;
		obj.transform.localScale = gameObject.transform.localScale;
		obj.transform.localPosition = gameObject.transform.localPosition;
	}

}
