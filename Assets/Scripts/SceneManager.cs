using UnityEngine;
using System.Collections.Generic;

public class SceneManager {
	SceneFactory sceneFactory;
	public Scene currentScene { get; private set;}
	LoopTracker loopTracker;

	// click instantly through scenes instead of waiting for them to transition
	public bool debugMode = false;
	
	// start the program at the given scene
	private int skipToSceneNumber = 0;
	
	public SceneManager(LoopTracker loopTracker) : this(null, loopTracker) {
	}
	
	public SceneManager (SceneFactory sceneFactory, LoopTracker loopTracker) {
		if(sceneFactory == null) {
			sceneFactory = new SceneFactory(this);
		}
		this.sceneFactory = sceneFactory;
		this.loopTracker = loopTracker;
		
		currentScene = sceneFactory.GetFirstScene();
		currentScene.Setup();
		
		SkipToScene(skipToSceneNumber);
	}
	
	private void SkipToScene(int skipToSceneNumber) {
		// Skipping to a particular scene at start time -- debugging feature
		for (int i = 1; i < skipToSceneNumber; i++) {
			currentScene.Transition();
		}
	}
	
	public void NextScene() {
		currentScene.Destroy();
		
		if(sceneFactory.isLastScene(currentScene)) {
			GameOver();
			return;
		}

		if(sceneFactory.isFirstScene(currentScene)) {
			loopTracker.startPlaying();
		}
		
		currentScene = sceneFactory.GetSceneAfter(currentScene);		
		Debug.Log("Beginning next scene (" + currentScene.GetType().Name + ")");
		currentScene.Setup();
		
		loopTracker.NextLoop(currentScene.TimeLength());
	}

	public void Update () {
		currentScene.Update();		
		
		if (loopTracker.IsLoopOver()) {
			if (currentScene.completed) {
				Debug.Log("scene complete. Transitioning...");
				currentScene.Transition();
			} else {
				loopTracker.Repeat();
			}
		}

		// Debugging ability to right-click through scenes
		if(Application.isEditor && Input.GetMouseButtonUp(1)) {
			if (debugMode) {
				Debug.Log("Clicked through scene");
				if (sceneFactory.isLastScene(currentScene)) GameOver();
				else NextScene();
			} else {
				Debug.Log("Causing scene to complete");
				currentScene.endScene();
			}
		}
	}
	
	void GameOver() {
		Debug.Log("game over");
		currentScene.Destroy();
		currentScene = sceneFactory.GetFirstScene();
		currentScene.Setup();
	}
}