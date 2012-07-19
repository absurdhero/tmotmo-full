using UnityEngine;

public class OffsetCamera {
	GameObject cameraObject;

	/// <summary>
	/// Add an additional camera to the scene layered on top of the main one
	/// </summary>
	/// <param name='offset'>
	/// Translate camera from the origin by this amount
	/// </param>
	/// <param name='viewLayer'>
	/// Set the Camera.cullingMask to this layer number
	/// </param>
	public OffsetCamera(Vector3 offset, int viewLayer) {
		cameraObject = new GameObject("Upper Camera");
		cameraObject.AddComponent<Camera>();
		var offsetCam = cameraObject.GetComponent<Camera>();
		offsetCam.depth = 0;
		offsetCam.cullingMask = 2;
		offsetCam.clearFlags = CameraClearFlags.Depth;
		offsetCam.orthographic = true;
		offsetCam.orthographicSize = 100;
		offsetCam.transform.Translate(offset);
	}
	
	public void Destroy() {
		GameObject.Destroy(cameraObject);
	}
}
