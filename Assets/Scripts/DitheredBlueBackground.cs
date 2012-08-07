using UnityEngine;


class DitheredBlueBackground {
	GameObject background, bottomDither;
	GameObjectFactory<string> resourceFactory;
	
	public DitheredBlueBackground(GameObjectFactory<string> resourceFactory) {
		this.resourceFactory = resourceFactory;
	}
	
	public void LoadAssets() {
		background = resourceFactory.Create("SceneTwelve/TealBackground");
		bottomDither = resourceFactory.Create("SceneTwelve/BottomDither");
		background.active = false;
		bottomDither.active = false;
	}

	public void Show() {
		background.active = true;
		bottomDither.active = true;
	}

	public void Destroy () {
		GameObject.Destroy(background);
		GameObject.Destroy(bottomDither);
	}
}