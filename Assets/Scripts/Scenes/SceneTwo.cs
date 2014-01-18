using UnityEngine;
using System;
using System.Collections.Generic;

class SceneTwo : Scene {
    public HospitalRoom room { get; private set; }
    
    private Wiggler wiggler;
    private TouchSensor sensor;
	Dictionary<GameObject, ActionResponsePair[]> prodResponses;

    public SceneTwo(SceneManager manager) : base(manager) {
        timeLength = 8.0f;
        room = new HospitalRoom(resourceFactory, camera);
    }

    public override void Setup(float startTime) {
        room.createBackground();
        room.addZzz();
        room.addHeartRate(startTime);
        room.addFootboard();
        room.addCover();
        room.addPerson();
        
        wiggler = new Wiggler(startTime, timeLength, room.cover.GetComponent<Sprite>());
        sensor = new TouchSensor(input);

		prodResponses = new Dictionary<GameObject, ActionResponsePair[]> {
			{room.zzz,       new [] {new ActionResponsePair("catch z", new [] {"that's not going to wake him up"})}},
			{room.cover,     new [] {new ActionResponsePair("prod him", new[] {"He doesn't want to wake up"}),
					new ActionResponsePair("prod him until he wakes up", new [] {"OK"}),
					new ActionResponsePair("expose him to the cold",
					                       new [] {
						"you remove the blankets, security and otherwise.",
						"there are now two distinct halves.",
						"are they the same person?"}),
				}},
		};
    }

    public override void Destroy() {
        wiggler.Destroy();
        // Handled by next scene
        //room.Destroy();
    }

    public override void Update () {
        wiggler.Update(Time.time);

        if (!completed) {
            if (room.touchedBed(sensor)) {
                room.openEyes();
            }
            
            if (room.eyesTotallyOpen) {
                room.removeZzz();
            }

			messagePromptCoordinator.hintWhenTouched((touched) => {
				if (touched == room.cover)  {
					room.removeCover();
					room.doubleHeartRate(Time.time);
					room.addSplitLine();
					endScene();
				}
			}, sensor, Time.time, prodResponses);
		}
        
		room.hintWhenTouched(GameObject => {}, messagePromptCoordinator, sensor);
		room.Update();
    }

}
