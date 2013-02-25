using UnityEngine;

class Continued : Scene {
	private Sprite text;
	private Color originalBGColor;
	private TouchSensor touch;

	public Continued(SceneManager manager) : base(manager) {
		timeLength = 16;
	}
	
	public override void Setup(float startTime) {
		text = Sprite.create("ToBeContinued");
		text.setScreenPosition(0, 0);
		originalBGColor = Camera.main.backgroundColor;
		Camera.main.backgroundColor = Color.black;
		touch = new TouchSensor(new UnityInput());
	}
	
	public override void Update() {
		if (touch.hasTaps())
			endScene();

		sceneManager.loopTracker.PlayAllButVocals();
	}
	
	public override void Destroy() {
		Camera.main.backgroundColor = originalBGColor;
		Sprite.Destroy(text);
	}
}
