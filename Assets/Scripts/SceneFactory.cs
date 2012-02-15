using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneFactory : MarshalByRefObject {
	SceneManager sceneManager;

	public SceneFactory (SceneManager sceneManager) {
		this.sceneManager = sceneManager;
	}
	
	public Scene GetFirstScene() {
		return new TitleScene(sceneManager);
	}

	public bool isFirstScene(Scene scene) {
		if (scene.GetType() == typeof(TitleScene)) return true;
		return false;
	}

	public bool isLastScene(Scene scene) {
		if (scene.GetType() == typeof(SceneFour)) return true;
		return false;
	}

	public Scene GetSceneAfter(Scene scene) {
		if (scene == null) return GetFirstScene();

		switch (scene.GetType().ToString()) {
		case "TitleScene":
			return new SceneOne(sceneManager);
		case "SceneOne":
			return new SceneTwo(sceneManager);
		case "SceneTwo":
			return new SceneThree(sceneManager, ((SceneTwo) scene).room);
		}
		
		throw new InvalidOperationException();
	}	
}