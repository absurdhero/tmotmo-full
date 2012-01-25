using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour {
	GameObject title;
	GameObject subtitle;
	GameObject news;
	GameObject buyMusic;
	GameObject background;
	
	private Cycler cycle_title;
	
	// Use this for initialization
	void Start () {
	  	// Stop reorientation weirdness 
		// http://answers.unity3d.com/questions/14655/unity-iphone-black-rect-when-i-turn-the-iphone
		TouchScreenKeyboard.autorotateToPortrait = false; 
		TouchScreenKeyboard.autorotateToPortraitUpsideDown = false; 
		TouchScreenKeyboard.autorotateToLandscapeRight = false; 
		TouchScreenKeyboard.autorotateToLandscapeLeft = false;
		
		title = (GameObject)Instantiate(Resources.Load("TitleScreen/Title"));
		subtitle = (GameObject)Instantiate(Resources.Load("TitleScreen/Subtitle"));
		news = (GameObject)Instantiate(Resources.Load("TitleScreen/News"));
		buyMusic = (GameObject)Instantiate(Resources.Load("TitleScreen/Buy Music"));
		background = (GameObject)Instantiate(Resources.Load("TitleScreen/BackgroundQuad"));
		
		Layout();
	}
	
	void Layout() {
		Camera cam = Camera.main;
		Sprite title_sprite = title.GetComponent<Sprite>();
		
		// Programmer needs swizzling, badly.
		var layoutpos = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.65f, 0.0f));
		layoutpos -= title_sprite.Center();
		title.transform.position = new Vector3(layoutpos.x, layoutpos.y, title.transform.position.z);
		// Anchor the subtitle an absolute distance from wherever the title ended up
		subtitle.transform.position = title.transform.position + new Vector3(5, -0.5f, 0);
		
		// place buttons in the bottom corners
		ScreenXYToWorld(news, 4, 4);
		ScreenXYToWorld(buyMusic, (int) cam.GetScreenWidth() - news.GetComponent<Sprite>().PixelWidth() - 4, 4);
		
		// animate title
		cycle_title = new Cycler(0.4f, 5);
		cycle_title.AddSprite(title.GetComponent<Sprite>());
		cycle_title.AddSprite(subtitle.GetComponent<Sprite>());
	}
	
	// Update is called once per frame
	void Update () {
		cycle_title.Update(Time.time);
	}
	
	private void ScreenXYToWorld(GameObject obj, int x, int y) {
		var layoutpos = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 0.0f));
		obj.transform.position = new Vector3(layoutpos.x, layoutpos.y, obj.transform.position.z);
	}
}
