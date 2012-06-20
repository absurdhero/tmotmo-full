using System;
using UnityEngine;

public class Confetti {
	GameObject[] confetti;
	float[] dropSpeeds;
	Vector3 initialConfettiPosition = new Vector3(0f, 25f, 0f);
		
	public bool pouring { get; private set; }
	public bool finishedPouring { get; private set; }
	
	float startedPouring;
	Metronome pouringAnimationFrequency;
	
	public Confetti() {
	}
		
	public void Pour(float time) {
		createConfettiPieces();
		setRandomFallSpeeds();

		pouring = true;
		startedPouring = time;
		pouringAnimationFrequency = new Metronome(time, 0.125f);
	}
		
	private const int verticalFloor = 10;

	public void Update(float time) {
		float timeSinceStart = time - startedPouring;
		if (timeSinceStart > 4) {
			finishedPouring = true;
			return;
		}

		if (pouring) {
			animateFallingPieces(time);
		}
	}

	void animateFallingPieces(float time) {
		if (pouringAnimationFrequency.isNextTick(time)) {
			for (int i = 0; i < confetti.Length; i++) {
				var sprite = confetti[i].GetComponent<Sprite>();
				if (sprite.getScreenPosition().y > verticalFloor)
					sprite.move(Vector3.down * dropSpeeds[i]);
			}
		}
	}
	
	public void Deactivate() {
		if (confetti == null) return;
		foreach (var piece in confetti) {
			piece.active = false;
		}
	}

	public void Destroy() {
		if (confetti == null) return;
		foreach (var piece in confetti) {
			GameObject.Destroy(piece);
		}
	}
	
	void createConfettiPieces() {
		var confettiGrid = new Grid(16f, 16f) {
				HorizontalOffset = 8f	
			};
			
		confetti = new GameObject[100];
		for (int i = 0; i < 10; i++) {
			for (int j = 0; j < 10; j++) {
				var n = i * 10 + j;
				confetti[n] = createConfettiPiece();
				var sprite = confetti[n].GetComponent<Sprite>();
				sprite.imageMaterial.SetUVToGridCell(confettiGrid, i, j % 16);
				// line up pieces horizontally in 10 columns 16 pixels apart
				// but vertically, slant them a bit so they line up with the slanted head
				sprite.move(new Vector3(16 * i, 4 * (10 - i)));
			}
		}			
	}
	
	/* Assign a speed to each piece.
	 * Set the random number generator to a constant seed
	 * and come up with a speed that could vary roughly by
	 * a factor of two for a good visual effect.
	 */
	void setRandomFallSpeeds() {
		dropSpeeds = new float[100];
		UnityEngine.Random.seed = 1;
		for(int i = 0; i < dropSpeeds.Length; i++) {
			dropSpeeds[i] = 6f + (8f * UnityEngine.Random.value);
		}
	}

	GameObject createConfettiPiece() {
		var piece = new GameObject("glitch confetti");
		piece.transform.position = initialConfettiPosition;
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
