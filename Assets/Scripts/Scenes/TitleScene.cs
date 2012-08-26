using UnityEngine;
using System;

public interface Factory<TReturn, TId>
{
	TReturn create(TId id);
}

public class ImageFactory : Factory<GameObject, string>
{
	public GameObject create(string id) 
	{
		return (GameObject)GameObject.Instantiate(Resources.Load(id));
	}
}

public class TitleScene : Scene {
	public GameObject title;
	public GameObject subtitle;
	public GameObject news;
	public GameObject buyMusic;
	public GameObject startButton;
	public GameObject background;
	
	private Cycler cycle_title, cycle_start;
	
	private UnityInput input;
	
	public TitleScene(SceneManager manager) : base(manager) {
		input = new UnityInput();
	}

	public override void Setup(float startTime) {
		background = resourceFactory.Create(this, "BackgroundQuad");
		title = resourceFactory.Create(this, "Title");
		subtitle = resourceFactory.Create(this, "Subtitle");
		news = resourceFactory.Create(this, "News");
		buyMusic = resourceFactory.Create(this, "Buy Music");
		startButton = resourceFactory.Create(this, "TapToStart");

		Camera cam = Camera.main;
		Sprite titleSprite = title.GetComponent<Sprite>();

		var layoutpos = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.65f, 0.0f));
		layoutpos -= titleSprite.Center();
		// Programmer needs swizzling, badly.
		title.transform.position = new Vector3(layoutpos.x, layoutpos.y, title.transform.position.z);
		// Anchor the subtitle an absolute distance from wherever the title ended up
		subtitle.transform.position = title.transform.position + titleSprite.Center() + new Vector3(15f, -20f, -1f);
		
		// add blinking start text below title but don't display it yet
		startButton.GetComponent<Sprite>().setCenterToViewportCoord(0.5f, 0.4f);
		startButton.active = false;
		
		// place buttons in the bottom corners
		MoveToScreenXY(news, 4, 4);
		MoveToScreenXY(buyMusic, (int) cam.GetScreenWidth() - buyMusic.GetComponent<Sprite>().PixelWidth() - 4, 4);
		
		// animate title
		cycle_title = new Cycler(0.4f, 5);
		cycle_title.AddSprite(title.GetComponent<Sprite>());
		cycle_title.AddSprite(subtitle.GetComponent<Sprite>());
	}
	
	public override void Update () {
		var touch = new TouchSensor(input);
		
		if (touch.insideSprite(Camera.main, buyMusic.GetComponent<Sprite>())) {
			Application.OpenURL("http://itunes.apple.com/us/album/same-not-same-ep/id533347009");
			ConsumeTouches();
		}
		else if (touch.insideSprite(Camera.main, news.GetComponent<Sprite>())) {
			Application.OpenURL("http://themakingofthemakingof.com");
			ConsumeTouches();
		}
		else if (touch.any()) {
			endScene();
		}

		if (cycle_title.Complete()) {
			animateStartButton();
		}

		cycle_title.Update(Time.time);
	}

	public override void Destroy() {
		GameObject.Destroy(title);
		GameObject.Destroy(subtitle);
		GameObject.Destroy(news);
		GameObject.Destroy(buyMusic);
		GameObject.Destroy(startButton);
		GameObject.Destroy(background);
	}

	private void animateStartButton() {
		if (cycle_start == null) {
			cycle_start = new Cycler(0.4f, 2);
			cycle_start.AddSprite(startButton);
			startButton.active = true;
		}
		cycle_start.Update(Time.time);
	}
}

