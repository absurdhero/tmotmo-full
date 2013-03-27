using UnityEngine;

class Continued : Scene {
	private Color originalBGColor;
	private TouchSensor touch;
	private MessageBox messageBox;

	public Continued(SceneManager manager) : base(manager) {
		timeLength = 16;
	}
	
	public override void Setup(float startTime) {
		originalBGColor = Camera.main.backgroundColor;
		Camera.main.backgroundColor = Color.black;
		touch = new TouchSensor(new UnityInput());
		var layout = GameObject.Find("Layout").GetComponent<Layout>();
		messageBox = layout.messageBox;
		messageBox.setMessage("Insert Disk 2");
		messageBox.show();
	}
	
	public override void Update() {
		if (touch.hasTaps())
			endScene();

		sceneManager.loopTracker.PlayAllButVocals();
	}
	
	public override void Destroy() {
		messageBox.hide();
		Camera.main.backgroundColor = originalBGColor;
	}
}
