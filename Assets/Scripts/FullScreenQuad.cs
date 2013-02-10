using UnityEngine;

public class FullScreenQuad : Sprite {
	const float DEPTH = 20;

	public static new FullScreenQuad create(object obj, params string[] textureNames) {
		return createFor<FullScreenQuad>(obj, textureNames);
	}

	public static new FullScreenQuad create(params string[] texturePaths) {
		return createFor<FullScreenQuad>(texturePaths);
	}
	
	public static new FullScreenQuad create(params Texture2D[] textures) {
		return createFor<FullScreenQuad>(textures);
	}
	
	override protected Mesh createMesh(Camera camera) {
		Mesh mesh = new Mesh();
		Vector3 tr = camera.ViewportToWorldPoint(new Vector3(1, 1, DEPTH));
		Vector3 tl = camera.ViewportToWorldPoint(new Vector3(0, 1, DEPTH));
		Vector3 br = camera.ViewportToWorldPoint(new Vector3(1, 0, DEPTH));
		Vector3 bl = camera.ViewportToWorldPoint(new Vector3(0, 0, DEPTH));
		mesh.vertices = new Vector3[] {tr, tl, br, bl};
		mesh.triangles = new int[] {0, 2, 1, 1, 2, 3};
		return mesh;
	}
}
