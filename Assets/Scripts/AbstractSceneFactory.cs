/// <summary>
/// Instantiates scenes and defines their order
/// </summary>
public interface AbstractSceneFactory {
	Scene GetFirstScene();
	bool isLastScene(Scene scene);
	Scene GetSceneAfter(Scene scene);
	
	void PreloadAssets();
	void Reset();
}

