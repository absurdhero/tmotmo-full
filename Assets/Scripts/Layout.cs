using UnityEngine;

/// <summary>
/// This is the entry point for the game.
/// Layout is added to an object in the Main Scene so Unity loads it at startup.
/// </summary>
public class Layout : MonoBehaviour {
	SceneManager sceneManager;
	Sounds sounds;
	public MessageBox messageBox {get; private set;}

	void Start () {
	  	// Stop reorientation weirdness 
		// http://answers.unity3d.com/questions/14655/unity-iphone-black-rect-when-i-turn-the-iphone
		TouchScreenKeyboard.autorotateToPortrait = false; 
		TouchScreenKeyboard.autorotateToPortraitUpsideDown = false; 
		TouchScreenKeyboard.autorotateToLandscapeRight = false; 
		TouchScreenKeyboard.autorotateToLandscapeLeft = false;

		sounds = new Sounds(gameObject);
		sounds.Start();

		var loopTracker = new LoopTracker(sounds);

		
		var textLabel = new GameObject("prompt text");
		textLabel.active = false;
		var text = textLabel.AddComponent<GUIText>();
		textLabel.transform.position = new Vector3(0f, 0.06f, -9.5f);
		var font = (Font) Resources.Load("sierra_agi_font/sierra_agi_font", typeof(Font));
		text.font = font;

		messageBox = new MessageBox(font);
		var prompt = new Prompt(textLabel, text);

		sceneManager = new SceneManager(loopTracker, new MessagePromptCoordinator(prompt, messageBox));
	}

	// Update is called once per frame
	void Update () {
		sceneManager.Update();
	}
}