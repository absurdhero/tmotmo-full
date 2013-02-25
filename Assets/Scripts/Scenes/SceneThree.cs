using UnityEngine;
using System;
using System.Collections.Generic;

class SceneThree : Scene {
	public HospitalRoom room { get; private set; }
	Wiggler wiggler, reverseWiggler;
	
	public const int MAX_SPLIT = 30;
	
	UnityInput input;

	TouchSensor sensor;
	
	Dictionary<GameObject, ActionResponsePair[]> prodResponses;
	
	public SceneThree(SceneManager manager, HospitalRoom room) : base(manager) {
		timeLength = 8.0f;
		rewindLoop(8.0f);

		this.room = room;
		input = new UnityInput();
	}

	public override void Setup(float startTime) {
		room.addSplitLine();
		room.removeCover();
		room.openEyes();
		room.openEyes();
		room.removeZzz();

		var guyLeftPivot = room.guyLeft.createPivotOnBottomRightCorner();
		var guyRightPivot = room.guyRight.createPivotOnBottomLeftCorner();
		wiggler = new Wiggler(startTime, timeLength, new[] {guyLeftPivot});
		reverseWiggler = new ReverseWiggler(startTime, timeLength, new[] {guyRightPivot});

		sensor = new TouchSensor(new UnityInput());
		
		prodResponses = new Dictionary<GameObject, ActionResponsePair[]> {
				{room.guyLeft.gameObject, new [] {new ActionResponsePair("prod Same",   new[] {"it's already awake."}),
				new ActionResponsePair("prod Same",   new[] {"listen to the lyrics."})}},
				{room.guyRight.gameObject, new [] {new ActionResponsePair("prod Not Same", new[] {"it's already awake."}),
				new ActionResponsePair("prod Not Same", new[] {"listen to the lyrics."})}},
		};
	}
	
	public override void Destroy() {
		wiggler.Destroy();
		reverseWiggler.Destroy();
	}

	public override void Update () {
		room.Update();
		wiggler.Update(Time.time);
		reverseWiggler.Update(Time.time);

		if (solved) return;

	    messagePromptCoordinator.hintWhenTouched(GameObject => {}, sensor, Time.time, prodResponses);

		if(room.guySplitDistance == MAX_SPLIT) {
			messagePromptCoordinator.solve(this, "pull guy in half");
			endScene();
		}
		else if(PinchingGuy()) {
			var distance = PinchDistance();
			var horizontal_distance = Math.Min(Math.Abs(distance.x), MAX_SPLIT);
			room.separateHalves(horizontal_distance);
			messagePromptCoordinator.progress("pull guy apart");
		}
	}
	
	private bool PinchingGuy() {
		return input.touchCount == 2
			&& (room.guyBoundsPlus(50).Contains(input.GetTouch(0).position)
			    || room.guyBoundsPlus(50).Contains(input.GetTouch(1).position));
	}
	
	private Vector2 PinchDistance() {
		if (input.touchCount >= 2
			&& (input.hasMoved(0) || input.hasMoved(1))) {
			return (input.GetTouch(1).position - input.GetTouch(0).position);
		}
		return Vector2.zero;
	}
}
