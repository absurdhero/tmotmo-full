using UnityEngine;

public class Prompt {
	GameObject textLabel, blackBox;
	GUIText text;
	Sprite okBox, nopeBox;
	bool correct, enabled, solveScene;
	float startTime = 0f;
	
	Scene sceneToSolve;
	
	const float promptTime = 1.5f;
	const float boxTime = 2.0f;
	
	public Prompt() {
	}
	
	public void Setup() {
		blackBox = GameObject.CreatePrimitive(PrimitiveType.Plane);
		blackBox.active = false;
		blackBox.name = "prompt background";
		blackBox.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.black);
		blackBox.transform.Rotate(new Vector3(270f, 0f, 0f));
		blackBox.transform.position = new Vector3(0, -95, -9);
		blackBox.transform.localScale = new Vector3(30f, 1f, 1.5f);
		
		textLabel = new GameObject("prompt text");
		textLabel.active = false;
		text = textLabel.AddComponent<GUIText>();
		textLabel.transform.position = new Vector3(0f, 0.06f, -9.5f);
		Font font = (Font) Resources.Load("sierra_agi_font/sierra_agi_font", typeof(Font));
		text.font = font;
		
		okBox = Sprite.create("ok box");
		okBox.visible(false);
		okBox.setCenterToViewportCoord(0.5f, 0.5f);
		nopeBox = Sprite.create("nope box");
		nopeBox.visible(false);
		nopeBox.setCenterToViewportCoord(0.5f, 0.5f);
	}
	
	public void Update(float time) {
		if (!enabled) return;
		
		if (time > startTime + promptTime) {
			hide();
			if (correct) {
				okBox.visible(true);
				if (solveScene) {
					sceneToSolve.endScene();
					solveScene = false;
				}
			} else {
				nopeBox.visible(true);
			}
		}
		if (time > startTime + promptTime + boxTime) {
			hide();
			enabled = false;
			hideBoxes();
		}
	}

	public void Reset() {
		hide();
		hideBoxes();
		solveScene = false;
		enabled = false;
	}
	
	public void Destroy() {
		GameObject.Destroy(textLabel);
	}

	public void solve(Scene scene, string message) {
		solveScene = true;
		correct = true;
		sceneToSolve = scene;
		print(message);
	}

	public void progress(string message) {
		correct = true;
		print(message);
	}

	public void hint(string message) {
		correct = false;
		print(message);
	}
	
	private void print(string message) {
		startTime = Time.time;
		hideBoxes();
		show();
		text.text = ">" + message + "_";
	}
	
	private void show() {
		enabled = true;
		blackBox.active = true;
		textLabel.active = true;
	}
	
	private void hide() {
		blackBox.active = false;
		textLabel.active = false;
	}

	void hideBoxes() {
		okBox.visible(false);
		nopeBox.visible(false);
	}
}

