using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Instantiates scenes and defines their order
/// </summary>
public class SceneFactory : MarshalByRefObject {
	SceneManager sceneManager;
	
	Type LAST_SCENE = typeof(Continued);

	bool scenesInstantiated = false;
	Scene[] scenes;

	public SceneFactory (SceneManager sceneManager) {
		this.sceneManager = sceneManager;
		scenes = new Scene[16];
	}

	private void ensureScenesInstantiated() {
		if (scenesInstantiated) return;

		scenes[0] = new TitleScene(sceneManager);
		scenes[1] = new SceneOne(sceneManager);
		scenes[2] = new SceneTwo(sceneManager);
		var sceneTwo = new SceneTwo(sceneManager);
		scenes[2] = sceneTwo;
		var sceneThree = new SceneThree(sceneManager, sceneTwo.room);
		scenes[3] = sceneThree;
		var sceneFour = new SceneFour(sceneManager, sceneThree.room);
		scenes[4] = sceneFour;
		scenes[5] = new SceneFive(sceneManager);
		scenes[6] = new SceneSix(sceneManager);
		scenes[7] = new SceneSeven(sceneManager);
		var sceneEight = new SceneEight(sceneManager);
		scenes[8] = sceneEight;
		scenes[9] = new SceneNine(sceneManager, sceneEight.confetti);
		scenes[10] = new SceneTen(sceneManager);
		scenes[11] = new SceneEleven(sceneManager);
		var sceneTwelve = new SceneTwelve(sceneManager);
		scenes[12] = sceneTwelve;
		var sceneThirteen = new SceneThirteen(sceneManager, sceneTwelve.fallingGuyProp);
		scenes[13] = sceneThirteen;
		scenes[14] = new SceneFourteen(sceneManager, sceneThirteen.fallingGuyProp);
		scenes[15] = new Continued(sceneManager);

		scenesInstantiated = true;
	}
	
	public Scene GetFirstScene() {
		ensureScenesInstantiated();
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
		ensureScenesInstantiated();
		if (scene == null) return GetFirstScene();

		int sceneIndex = Array.FindIndex<Scene>(scenes, s => s.GetType() == scene.GetType());
		if (sceneIndex > scenes.Length)
			throw new InvalidOperationException();
		return scenes[sceneIndex + 1];
	}

	public void PreloadAssets() {
		ensureScenesInstantiated();
		foreach(var scene in scenes) {
			scene.LoadAssets();
		}
	}
	
	public void Reset() {
		scenesInstantiated = false;
		scenes = new Scene[16];
	}
}
