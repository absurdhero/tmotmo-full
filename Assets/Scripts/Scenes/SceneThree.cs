using UnityEngine;
using System;

class SceneThree : Scene {
	public HospitalRoom room { get; private set; }
	
	public const int MAX_SPLIT = 40;
	
	UnityInput input;
	
	public SceneThree(SceneManager manager, HospitalRoom room) : base(manager) {
		this.room = room;
		input = new UnityInput();
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
		
		if (Application.isEditor && input.GetMouseButtonUp(0)) {
			room.separateHalves(MAX_SPLIT);
		}

	}
	
	private bool PinchingGuy() {
		return input.touchCount == 2
			&& (room.guyBoundsPlus(50).Contains(input.GetTouch(0).position)
			    || room.guyBoundsPlus(50).Contains(input.GetTouch(1).position));
	}
	
	private Vector2 PinchDistance() {
		if (input.touchCount >= 2
			&& input.hasMoved(0) && input.hasMoved(1)) {
			return (input.GetTouch(1).position - input.GetTouch(0).position);
		}
		return Vector2.zero;
	}
}
