using UnityEngine;
using System.Collections.Generic;

public class SceneTen : Scene {
	Metronome stompSpeed;
	GameObject amp;
	GameObject shoe;
	GameObject wireShadow;
	GameObject glassCracks;
	GameObject wires;
	FullScreenQuad background;
	
	Sprite ampSprite { get { return amp.GetComponent<Sprite>(); }}
	Sprite shoeSprite { get { return shoe.GetComponent<Sprite>(); }}
	Sprite glassSprite { get { return glassCracks.GetComponent<Sprite>(); }}
	Sprite wiresSprite { get { return wires.GetComponent<Sprite>(); }}
	Sprite wireShadowSprite { get { return wireShadow.GetComponent<Sprite>(); }}
	
	Animatable pedalStomped, pedalUnStomped, stompFoot, retractFoot;
	Animatable[] animatables;
	
	const int firstCrackFrame = 3;

	public SceneTen(SceneManager manager) : base(manager) {
		timeLength = 1.75f;
		permitUnloadResources = false;
	}
	
	public override void LoadAssets() {
		background = FullScreenQuad.create(this, "pedal_stomp_bg");
		shoe = resourceFactory.Create("SceneTen/Shoe");
		amp = resourceFactory.Create(this, "Amp");
		wires = resourceFactory.Create(this, "Wires");
		wireShadow = resourceFactory.Create(this, "WireShadow");

		background.visible(false);
		shoe.active = false;
		amp.active = false;
		wires.active = false;
		wireShadow.active = false;


		// double the scale on all of these because the art is half-size
		shoe.transform.localScale = Vector3.one * 2;
		shoe.GetComponent<Sprite>().setScreenPosition(0, 164);
		amp.GetComponent<Sprite>().setScreenPosition(130, 50);
		amp.transform.localScale = Vector3.one * 2;
		wires.GetComponent<Sprite>().setScreenPosition(30, 30);
		wires.transform.localScale = Vector3.one * 2;			
		wireShadow.GetComponent<Sprite>().setScreenPosition(30, 30);
		wireShadow.transform.localScale = Vector3.one * 2;
		
		pedalStomped = new PedalStomped(new List<Sprite> {ampSprite, wiresSprite, wireShadowSprite});
		pedalUnStomped = new PedalUnStomped(new List<Sprite> {ampSprite, wiresSprite, wireShadowSprite});
		stompFoot = new StompFoot(new List<Sprite> {shoeSprite});
		retractFoot = new RetractFoot(new List<Sprite> {shoeSprite});
		animatables = new[] {pedalStomped, pedalUnStomped, stompFoot, retractFoot};
	}

	public override void Setup (float startTime) {
		endScene();
		
		background.visible(true);
		shoe.active = true;
		amp.active = true;
		wires.active = true;
		wireShadow.active = true;

		stompSpeed = new Metronome(startTime, 0.1f);
	}

	public override void Update () {
		float time = Time.time;
		if (stompSpeed.isNextTick(time)) {
			OnTick(stompSpeed.nextTick);
		}
	}

	public override void Destroy () {
		GameObject.Destroy(amp);
		GameObject.Destroy(shoe);
		GameObject.Destroy(wireShadow);
		GameObject.Destroy(glassCracks);
		GameObject.Destroy(wires);
		FullScreenQuad.Destroy(background);
	}
			
	public void beginCracks() {
		glassCracks = resourceFactory.Create(this, "GlassCracks");
		glassCracks.transform.localScale = Vector3.one * 2;
		glassSprite.setScreenPosition(0, 0);
		glassSprite.setDepth(0f);
	}
	
	private void OnTick(int currentTick) {
		foreach(var animatable in animatables) {
			animatable.animate(currentTick);
		}

		if (currentTick == firstCrackFrame) {
			beginCracks();
		}
		
		if (currentTick > firstCrackFrame) {
			glassSprite.DrawNextFrame();
		}
	}

	class PedalStomped : Mover {
		static readonly Vector3 stompDisplacement = new Vector3(10, -10);

		public PedalStomped(ICollection<Sprite> sprites) : base(sprites, stompDisplacement, 4, 4) {}
	}

	class PedalUnStomped : Mover {
		static readonly Vector3 unstompDisplacement = new Vector3(-10, 10);

		public PedalUnStomped(ICollection<Sprite> sprites) : base(sprites, unstompDisplacement, 6, 6) {}
	}
	
	class StompFoot : Mover {
		static readonly Vector3 descent = new Vector3(30f, -30f);

		public StompFoot(ICollection<Sprite> sprites) : base(sprites, descent, 0, 4) {}
	}

	class RetractFoot : Mover {
		static readonly Vector3 ascent = new Vector3(-30f, 30f);

		public RetractFoot(ICollection<Sprite> sprites) : base(sprites, ascent, 4, 8) {}
	}
}
