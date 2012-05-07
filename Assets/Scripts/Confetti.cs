using System;
using UnityEngine;

public class Confetti {
	GameObjectFactory<string> resourceFactory;
	GameObject[] confetti;
		
	public bool pouring { get; private set; }
	public bool finishedPouring { get; private set; }

	public Confetti(GameObjectFactory<string> resourceFactory) {
		this.resourceFactory = resourceFactory;
	}
		
	public void Pour() {
		Create();
		pouring = true;
	}
		
	public void Update(float time) {
		finishedPouring = true;
	}
		
	public void Destroy() {
		foreach (var piece in confetti) {
			GameObject.Destroy(piece);
		}
	}
	
	void Create() {
		var confettiGrid = new Grid(16f, 16f) {
				HorizontalOffset = 8f	
			};
			
		confetti = new GameObject[100];
		for (int i = 0; i < 9; i++) {
			for (int j = 0; j < 9; j++) {
				var n = i * 10 + j;
				confetti[n] = createConfettiPiece();
				var sprite = confetti[n].GetComponent<Sprite>();
				sprite.imageMaterial.SetUVToGridCell(confettiGrid, i, j % 16);
					
				var screenPosition = sprite.getScreenPosition() + new Vector3(16 * i, 16 * (j % 16));
				sprite.setScreenPosition(screenPosition.x, screenPosition.y);
			}
		}
			
	}

	GameObject createConfettiPiece() {
		var piece = new GameObject("glitch confetti");
		var sprite = piece.AddComponent<Sprite>();
		sprite.height = 16;
		sprite.width = 16;
		sprite.textures = new Texture2D[] {
				(Texture2D)Resources.Load("TodoList/glitch")
			};
		sprite.Awake();
		sprite.Start();
			
			
		return piece;
	}

}
