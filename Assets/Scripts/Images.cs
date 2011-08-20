using UnityEngine;
using System.Collections;

// This class holds all images in a convenient central location
public class Images : MonoBehaviour {
	public static Images it;
  
  public Texture2D transparent;

  public Texture2D p1_Circle1;
  public Texture2D p1_Circle2;
  public Texture2D p1_Circle3;
  public Texture2D p1_Circle4;
  public Texture2D p1_Circle5;
  public Texture2D p1_Triangle1;
  public Texture2D p1_Triangle2;
  public Texture2D p1_Triangle3;

  public Texture2D p2_Z;
  public Texture2D p2_Face1;
  public Texture2D p2_Face2;
  public Texture2D p2_Face3;

  public Texture2D p3_FullBed;
  public Texture2D p3_NoBed;
  public Texture2D p3_LeftBed;
  public Texture2D p3_RightBed;

  public Texture2D p4_BubbleLeft;
  public Texture2D p4_BubbleRight;
  public Texture2D p4_FaceClosed;
  public Texture2D p4_FaceLeftOpen;
  public Texture2D p4_FaceRightOpen;

	public Texture2D img1;
	public Texture2D img2;
  public Texture2D bg1;
	
	// Use this for initialization
	void Start () {
		it = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
