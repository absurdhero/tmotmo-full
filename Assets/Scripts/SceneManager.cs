using UnityEngine;
using System.Collections.Generic;

public class SceneManager {
	SceneFactory sceneFactory;
	public Scene currentScene { get; private set;}
	LoopTracker loopTracker;

	// click instantly through scenes instead of waiting for them to transition
	public bool debugMode = false;
	
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
	}
	
	public void NextScene() {
		Debug.Log("Beginning next scene (" + currentScene.GetType().Name + ")");
		currentScene.Destroy();
		currentScene.completed = false;
		
		if(currentScene == sceneFactory.GetLastScene()) {
			GameOver();
			return;
		}

		if(currentScene == sceneFactory.GetFirstScene()) {
			loopTracker.startPlaying();
		}
		
		currentScene = sceneFactory.GetSceneAfter(currentScene);		
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
				if (currentScene == sceneFactory.GetLastScene()) GameOver();
				else NextScene();
			} else {
				Debug.Log("Causing scene to complete");
				currentScene.completed = true;
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