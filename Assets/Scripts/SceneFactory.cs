using System;
using System.Collections.Generic;

public class SceneFactory : MarshalByRefObject {
	public List<Scene> sceneSequence;

	public SceneFactory (SceneManager sceneManager) {
		var titleScene = new TitleScene(sceneManager);
		var sceneOne   = new SceneOne(sceneManager);
		var sceneTwo   = new SceneTwo(sceneManager);
		sceneSequence = new List<Scene>{titleScene, sceneOne, sceneTwo};
	}
	
	public Scene GetFirstScene() {
		return sceneSequence[0];
	}

	public Scene GetLastScene() {
		return sceneSequence[sceneSequence.Count - 1];
	}

	public Scene GetSceneAfter(Scene scene) {
		if (scene == null) return GetFirstScene();
		
		int i = sceneSequence.IndexOf(scene);
		int nextIndex = i + 1;

		if (nextIndex == sceneSequence.Count) {
			return null;
		} else {
			return sceneSequence[nextIndex];
		}
	}	
}
