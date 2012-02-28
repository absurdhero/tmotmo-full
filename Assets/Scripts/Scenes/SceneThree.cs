using UnityEngine;
using System;

class SceneThree : Scene {
	public HospitalRoom room { get; private set; }
	
	public const int MAX_SPLIT = 40;
	
	public SceneThree(SceneManager manager, HospitalRoom room) : base(manager) {
		this.room = room;
	}

	public override void Setup() {
		timeLength = 8.0f;
		rewindLoop(8.0f);

		room.addSplitLine();
		room.removeCover();
		room.openEyes();
		room.openEyes();
		room.removeZzz();
	}
	
	public override void Destroy() {
	}

	public override void Update () {
		room.Update();
		
		if(room.guySplitDistance == MAX_SPLIT) {
			endScene();
			return;
		}

		if(PinchingGuy()) {
			var distance = PinchDistance();
			var horizontal_distance = Math.Min(Math.Abs(distance.x), MAX_SPLIT);
			room.separateHalves(horizontal_distance);
		}
		
		if (Application.isEditor && Input.GetMouseButtonUp(0)) {
			room.separateHalves(MAX_SPLIT);
		}

	}
	
	private bool PinchingGuy() {
		return Input.touchCount == 2
			&& (room.guyBounds(50).Contains(Input.GetTouch(0).position)
			    || room.guyBounds(50).Contains(Input.GetTouch(1).position));
	}
	
	private Vector2 PinchDistance() {
		if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved) {
			return Input.GetTouch(1).position - Input.GetTouch(0).position;
		}
		return Vector2.zero;
	}
	
}
