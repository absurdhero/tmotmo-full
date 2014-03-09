using UnityEngine;

public interface AbstractInput {
	int touchCount {
		get;
	}
	
	Vector3 mousePosition {
		get;
	}
	
	Touch GetTouch(int index);
	
	bool hasMoved(int touchIndex);
	
	bool GetMouseButtonUp(int index);
	
	bool GetMouseButton(int index);
}

