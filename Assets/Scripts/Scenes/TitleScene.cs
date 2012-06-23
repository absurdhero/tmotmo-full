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
	public GameObject background;
	
	private Cycler cycle_title;
	
	private UnityInput input;
	
	public TitleScene(SceneManager manager) : base(manager) {
		input = new UnityInput();
	}
	
	public override void Setup() {
		background = resourceFactory.Create(this, "BackgroundQuad");
		title = resourceFactory.Create(this, "Title");
		subtitle = resourceFactory.Create(this, "Subtitle");
		news = resourceFactory.Create(this, "News");
		buyMusic = resourceFactory.Create(this, "Buy Music");

		Camera cam = Camera.main;
		Sprite titleSprite = title.GetComponent<Sprite>();

		var layoutpos = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.65f, 0.0f));
		layoutpos -= titleSprite.Center();
		// Programmer needs swizzling, badly.
		title.transform.position = new Vector3(layoutpos.x, layoutpos.y, title.transform.position.z);
		// Anchor the subtitle an absolute distance from wherever the title ended up
		subtitle.transform.position = title.transform.position + titleSprite.Center() + new Vector3(15f, -20f, -1f);
		
		// place buttons in the bottom corners
		MoveToScreenXY(news, 4, 4);
		MoveToScreenXY(buyMusic, (int) cam.GetScreenWidth() - buyMusic.GetComponent<Sprite>().PixelWidth() - 4, 4);
		
		// animate title
		cycle_title = new Cycler(0.4f, 5);
		cycle_title.AddSprite(title.GetComponent<Sprite>());
		cycle_title.AddSprite(subtitle.GetComponent<Sprite>());
	}
	
	public override void Update () {
		bool touchedBuy = false;
		bool touchedNews = false;
		bool touched = input.touchCount > 0;
		
		for (int i = 0; i < input.touchCount; i++) {
			Debug.Log("TOUCHED");
			var touch = input.GetTouch(i);
			touchedBuy |= buyMusic.GetComponent<Sprite>().Contains(touch.position);
			touchedNews |= news.GetComponent<Sprite>().Contains(touch.position);
	    }
		
		if (touchedBuy) {
			Application.OpenURL("http://apple.com/itunes");
			ConsumeTouches();
		}
		else if (touchedNews) {
			Application.OpenURL("http://themakingofthemakingof.com");
			ConsumeTouches();
		}
		else if (touched) {
			endScene();
		}

		cycle_title.Update(Time.time);
	}

	public override void Destroy() {
		GameObject.Destroy(title);
		GameObject.Destroy(subtitle);
		GameObject.Destroy(news);
		GameObject.Destroy(buyMusic);
		GameObject.Destroy(background);
	}
}

