using UnityEngine;

class Continued : Scene {
	private Sprite text;
	private Color originalBGColor;

	public Continued(SceneManager manager) : base(manager) {
		timeLength = 8;
		this.rewindLoop(8);
	}
	
	public override void Setup(float startTime) {
		text = Sprite.create("ToBeContinued");
		text.setScreenPosition(0, 0);
		originalBGColor = Camera.main.backgroundColor;
		Camera.main.backgroundColor = Color.black;
	}
	
	public override void Update() {
	}
	
	public override void Destroy() {
		Camera.main.backgroundColor = originalBGColor;
		Sprite.Destroy(text);
	}
}
