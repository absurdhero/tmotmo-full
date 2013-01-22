using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Controls progression from scene to scene and provides shared game state accessible to scenes.
/// </summary>
public class SceneManager {
	SceneFactory sceneFactory;
	public Scene currentScene { get; private set;}
	public LoopTracker loopTracker { get; private set;}
	public Prompt prompt { get; private set;}

	// click instantly through scenes instead of waiting for them to transition
	public bool debugMode = false;
	
	// start the program at the given scene
	private int skipToSceneNumber = 0;
	
	public SceneManager(LoopTracker loopTracker) : this(null, loopTracker, new Prompt()) {
	}
	
	public SceneManager (SceneFactory sceneFactory, LoopTracker loopTracker, Prompt prompt) {
		if(sceneFactory == null) {
			sceneFactory = new SceneFactory(this);
		}
		this.sceneFactory = sceneFactory;
		this.loopTracker = loopTracker;
		this.prompt = prompt;

		StartGame();
	}

	void StartGame() {
		sceneFactory.PreloadAssets();
		prompt.Setup();

		currentScene = sceneFactory.GetFirstScene();
		currentScene.Setup(Time.time);
		SkipToScene(skipToSceneNumber);
	}
	
	private void SkipToScene(int skipToSceneNumber) {
		// Skipping to a particular scene at start time -- debugging feature
		for (int i = 1; i < skipToSceneNumber; i++) {
			currentScene.Transition();
		}
	}
	
	public void NextScene() {
		if(sceneFactory.isLastScene(currentScene)) {
			GameOver();
			return;
		}

		currentScene.Destroy();

		if(sceneFactory.isFirstScene(currentScene)) {
			loopTracker.startPlaying();
		}
		
		currentScene = sceneFactory.GetSceneAfter(currentScene);

		if (currentScene.permitUnloadResources) {
			Resources.UnloadUnusedAssets();
		}

		Debug.Log("Beginning next scene (" + currentScene.GetType().Name + ")");

		loopTracker.Rewind(currentScene.rewindTime);
		loopTracker.NextLoop(currentScene.TimeLength());

		currentScene.Setup(Time.time);
	}

	public void Update () {
		currentScene.Update();
		
		if (currentScene.solved) {
			loopTracker.PlayAll();
		}

		if (loopTracker.IsLoopOver()) {
			if (currentScene.completed) {
				Debug.Log("scene complete. Transitioning...");
				prompt.Reset();
				currentScene.Transition();
			} else {
				loopTracker.Repeat();
			}
		}
		
		prompt.Update(Time.time);

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

	public float timeLeftInCurrentLoop() {
		return loopTracker.TimeLeftInCurrentLoop();
	}

	public void changeSceneLength(float length) {
		loopTracker.ChangeLoopLength(length);
	}

	void GameOver() {
		Debug.Log("game over");
		loopTracker.Stop();
		currentScene.Destroy();
		sceneFactory.Reset();
		StartGame();
	}
}
