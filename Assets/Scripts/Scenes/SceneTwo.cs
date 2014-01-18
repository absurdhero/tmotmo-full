using UnityEngine;
using System;
using System.Collections.Generic;

class SceneTwo : Scene {
    public HospitalRoom room { get; private set; }
    
    private Wiggler wiggler;
    private TouchSensor sensor;

	GameObject cover;
	GameObject zzz;
	ZzzAnimator zzzAnimator;

	Dictionary<GameObject, ActionResponsePair[]> prodResponses;

    public SceneTwo(SceneManager manager) : base(manager) {
        timeLength = 8.0f;
        room = new HospitalRoom(resourceFactory, camera);
    }

    public override void Setup(float startTime) {
        room.createBackground();
		addCover();
		addZzz();
		room.addHeartRate(startTime);
        room.addFootboard();
        room.addPerson();
        

		wiggler = new Wiggler(startTime, timeLength, cover.GetComponent<Sprite>());
        sensor = new TouchSensor(input);

		prodResponses = new Dictionary<GameObject, ActionResponsePair[]> {
			{zzz,       new [] {new ActionResponsePair("catch z", new [] {"that's not going to wake him up"})}},
			{cover,     new [] {new ActionResponsePair("prod him", new[] {"He doesn't want to wake up"}),
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
		if (cover != null) GameObject.Destroy(cover);
		removeZzz();

        // Handled by next scene
        //room.Destroy();
    }

    public override void Update () {
        wiggler.Update(Time.time);

        if (!completed) {
            if (touchedBed(sensor)) {
                room.openEyes();
            }
            
            if (room.eyesTotallyOpen) {
                removeZzz();
            }

			messagePromptCoordinator.hintWhenTouched((touched) => {
				if (touched == cover)  {
					removeCover();
					room.doubleHeartRate(Time.time);
					room.addSplitLine();
					endScene();
				}
			}, sensor, Time.time, prodResponses);
		}

		if (zzzAnimator != null && !room.eyesTotallyOpen) {
			zzzAnimator.Update(Time.time);
		}

		room.hintWhenTouched(GameObject => {}, messagePromptCoordinator, sensor);
		room.Update();
    }

	public bool touchedBed(TouchSensor touch) {
		return touch.insideSprite(Camera.main, cover.GetComponent<Sprite>(), new[] {TouchPhase.Began});
	}

	public void addCover() {
		if (cover != null) {
			cover.SetActive(true);
			return;
		}
		cover = resourceFactory.Create("HospitalRoom/Cover");
		var coverSprite = cover.GetComponent<Sprite>();
		coverSprite.setCenterToViewportCoord(0.515f, 0.34f);
		
	}
	
	public void removeCover() {
		cover.SetActive(false);
	}

	public void addZzz() {
		zzz = resourceFactory.Create("HospitalRoom/Zzz");
		zzz.GetComponent<Sprite>().setWorldPosition(50f, 60f, -1f);
		zzzAnimator = new ZzzAnimator(zzz);
	}
	
	public void removeZzz() {
		prodResponses.Remove(zzz);
		if (zzz != null) GameObject.Destroy(zzz);
	}
	

	class ZzzAnimator : Repeater {
		GameObject zzz;
		Vector3 initialPosition;
		int frame = 0;
		
		const int totalFrames = 4;
		
		
		public ZzzAnimator(GameObject zzz) : base(0.5f) {
			this.zzz = zzz;
			initialPosition = zzz.transform.position;
		}
		
		public override void OnTick() {
			frame = (frame + 1) % totalFrames;
			zzz.transform.position = initialPosition + new Vector3(frame * 1, frame * 1, 0);
		}
		
		public void Reset() {
			zzz.transform.position = initialPosition;
		}
	}
}
