using UnityEngine;
using System.Collections.Generic;

public interface AbstractTouchSensor
{
	bool insideSprite(Camera camera, Sprite sprite);

	bool changeInsideSprite(Camera camera, Sprite sprite);

	bool insideSprite(Camera camera, Sprite sprite, ICollection<TouchPhase> phases);

	bool hasTaps();

}

