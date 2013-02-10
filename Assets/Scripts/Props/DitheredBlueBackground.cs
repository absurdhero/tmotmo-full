using UnityEngine;


class DitheredBlueBackground {
	FullScreenQuad background;
	GameObject bottomDither;
	GameObjectFactory<string> resourceFactory;
	
	public DitheredBlueBackground(GameObjectFactory<string> resourceFactory) {
		this.resourceFactory = resourceFactory;
	}
	
	public void LoadAssets() {
		background = FullScreenQuad.create("SceneTwelve/bgletgo");
		bottomDither = resourceFactory.Create("SceneTwelve/BottomDither");
		background.visible(false);
		bottomDither.active = false;
	}

	public void Show() {
		background.visible(true);
		bottomDither.active = true;
	}

	public void Destroy () {
		FullScreenQuad.Destroy(background);
		GameObject.Destroy(bottomDither);
	}
}