using UnityEngine;

class Continued : Scene {
	private Sprite text;
	private Color originalBGColor;
	private UnityInput input;

	public Continued(SceneManager manager) : base(manager) {
		timeLength = 8;
		rewindLoop(8);
	}
	
	public override void Setup(float startTime) {
		text = Sprite.create("ToBeContinued");
		text.setScreenPosition(0, 0);
		originalBGColor = Camera.main.backgroundColor;
		Camera.main.backgroundColor = Color.black;
		input = new UnityInput();
	}
	
	public override void Update() {
		if (input.touchCount > 0 && input.GetTouch(0).phase == TouchPhase.Began)
			endScene();

		sceneManager.loopTracker.PlayAllButVocals();
	}
	
	public override void Destroy() {
		Camera.main.backgroundColor = originalBGColor;
		Sprite.Destroy(text);
	}
}
