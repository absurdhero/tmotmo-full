using UnityEngine;

public class Layout : MonoBehaviour {
	SceneManager sceneManager;
	Sounds sounds;

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
		sceneManager = new SceneManager(loopTracker);
	}

	// Update is called once per frame
	void Update () {
		sceneManager.Update();
	}
}