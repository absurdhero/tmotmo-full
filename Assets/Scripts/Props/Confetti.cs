using System;
using UnityEngine;

public class Confetti {
	GameObject[] confetti;
	float[] dropSpeeds;
	Vector3 initialConfettiPosition = new Vector3(0f, 25f, 0f);
	
	public const int gridLength = 10;
	public const int numberOfPieces = 100;
	
	Grid confettiGrid = new Grid(16f, 16f) {
				HorizontalOffset = 8f	
	};
	
	private const int verticalFloor = 10;
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
	
	
	public void ensureConfettiWasPoured() {
		if (finishedPouring) return;
		
		// fast forward quickly to the end of the 4 second pouring sequence
		var time = Time.time - 4f;
		if (!pouring) Pour(time);
		for(float t = time; t <= time + 4f; t += pouringAnimationFrequency.interval) {
			Update(t);
		}
	}
	
	public void expandSideways(float horizontalScrollSpeed, int frameCount) {
		const int delay = 2;
		int columnsToMove = Mathf.Min(frameCount-delay, gridLength / 2);

		for (int column = 0; column < columnsToMove; column++) {
			for (int row = 0; row < gridLength; row++) {
				var piece = confetti[column * 10 + row];
				int direction = 1;
				if (row < 5) direction = -1;
				piece.GetComponent<Sprite>().move(direction * horizontalScrollSpeed, 0f);
			}
		}
	}

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
		var texture =  (Texture2D) Resources.Load("TodoList/glitch");

		confetti = new GameObject[numberOfPieces];
		for (int i = 0; i < gridLength; i++) {
			for (int j = 0; j < gridLength; j++) {
				var n = i * 10 + j;
				confetti[n] = createConfettiPiece(texture);
				var sprite = confetti[n].GetComponent<Sprite>();
				sprite.imageMaterial.SetUVToGridCell(confettiGrid, i, j % 16);
				// line up pieces horizontally in 10 columns 16 pixels apart
				// but vertically, slant them a bit so they line up with the slanted head
				sprite.move(new Vector3(12 * i + 20, 4 * (10 - i)));
			}
		}			
	}
	
	/* Assign a speed to each piece.
	 * Set the random number generator to a constant seed
	 * and come up with a speed that could vary roughly by
	 * a factor of two for a good visual effect.
	 */
	void setRandomFallSpeeds() {
		dropSpeeds = new float[numberOfPieces];
		UnityEngine.Random.seed = 1;
		for(int i = 0; i < dropSpeeds.Length; i++) {
			dropSpeeds[i] = 6f + (8f * UnityEngine.Random.value);
		}
	}

	GameObject createConfettiPiece(Texture2D texture) {
		var piece = new GameObject("glitch confetti");
		piece.transform.position = initialConfettiPosition;
		var sprite = piece.AddComponent<Sprite>();
		sprite.height = 16;
		sprite.width = 16;
		sprite.textures = new Texture2D[] {
				texture
			};
		sprite.Awake();
		sprite.Start();	
			
		return piece;
	}

	public void hidePiece(int pieceIndex) {
		confetti[pieceIndex].GetComponent<Sprite>().visible(false);
	}
}
