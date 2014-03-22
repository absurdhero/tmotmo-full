using UnityEngine;
using System;

public class TitleScene : AbstractScene {
	public Sprite title;
	public Sprite subtitle;
	public Sprite news;
	public Sprite buyMusic;
	public Sprite startButton;
	public Sprite background;
	
	private Cycler cycle_title, cycle_start;
	
	public TitleScene(SceneManager manager) : base(manager) {
	}

	public override void Setup(float startTime) {
		background = FullScreenQuad.create(this, "bg");
		title = Sprite.create(this, new[] {"tmo1", "tmo2", "tmo3", "tmo4", "tmo5", "tmo6"});
		subtitle = Sprite.create(this, new[] {"p1", "p2", "p3", "p4", "p5", "p6"});
		news = Sprite.create(this, new[] {"news1", "news2"});
		buyMusic = Sprite.create(this, new[] {"itunes1", "itunes2"});
		startButton = Sprite.create(this, new[] {"tap1", "tap2", "tap3"});
		
		Camera cam = Camera.main;

		var layoutpos = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.65f, 0.0f));
		layoutpos -= title.Center();
		// Programmer needs swizzling, badly.
		title.transform.position = new Vector3(layoutpos.x, layoutpos.y, title.transform.position.z);
		// Anchor the subtitle an absolute distance from wherever the title ended up
		subtitle.transform.position = title.transform.position + title.Center() + new Vector3(15f, -20f, -1f);
		
		// add blinking start text below title but don't display it yet
		startButton.setCenterToViewportCoord(0.5f, 0.4f);
		startButton.visible(false);
		
		// place buttons in the bottom corners
		news.setScreenPosition(4, 4);
		buyMusic.setScreenPosition((int) Screen.width - buyMusic.PixelWidth() - 4, 4);
		
		// animate title
		cycle_title = new Cycler(0.4f, 5);
		cycle_title.AddSprite(title);
		cycle_title.AddSprite(subtitle);
	}
	
	public override void Update () {
		var touch = new TouchSensor(input, gameObjectFinder);
		
		if (touch.insideSprite(Camera.main, buyMusic)) {
			Application.OpenURL("http://itunes.apple.com/us/album/same-not-same-ep/id533347009");
		}
		else if (touch.insideSprite(Camera.main, news)) {
			Application.OpenURL("http://themakingofthemakingof.com");
		}
		else if (touch.hasTaps()) {
			endScene();
		}

		if (cycle_title.Complete()) {
			animateStartButton();
		}

		cycle_title.Update(Time.time);
	}

	public override void Destroy() {
		Sprite.Destroy(title);
		Sprite.Destroy(subtitle);
		Sprite.Destroy(news);
		Sprite.Destroy(buyMusic);
		Sprite.Destroy(startButton);
		Sprite.Destroy(background);
	}

	private void animateStartButton() {
		if (cycle_start == null) {
			cycle_start = new Cycler(0.4f, 2);
			cycle_start.AddSprite(startButton);
			startButton.visible(true);
		}
		cycle_start.Update(Time.time);
	}
}

