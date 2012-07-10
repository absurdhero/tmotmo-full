using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneFactory : MarshalByRefObject {
	SceneManager sceneManager;
	
	Type LAST_SCENE = typeof(SceneThirteen);

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
		if (scene.GetType() == LAST_SCENE) return true;
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
		case "SceneThree":
			return new SceneFour(sceneManager, ((SceneThree) scene).room);
		case "SceneFour":
			return new SceneFive(sceneManager);
		case "SceneFive":
			return new SceneSix(sceneManager);
		case "SceneSix":
			return new SceneSeven(sceneManager);
		case "SceneSeven":
			return new SceneEight(sceneManager);
		case "SceneEight":
			return new SceneNine(sceneManager, ((SceneEight) scene).confetti);
		case "SceneNine":
			return new SceneTen(sceneManager);
		case "SceneTen":
			return new SceneEleven(sceneManager);
		case "SceneEleven":
			return new SceneTwelve(sceneManager);
		case "SceneTwelve":
			return new SceneThirteen(sceneManager);
		}
		
		throw new InvalidOperationException();
	}	
}
