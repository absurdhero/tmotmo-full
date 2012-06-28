using System;
using UnityEngine;

public class Confetti {
	GameObject[] confetti;
	float[] dropSpeeds;
	Vector3 initialConfettiPosition = new Vector3(0f, 25f, 0f);
	
	const int gridLength = 10;
	const int numberOfPieces = 100;
	
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
	
	public void followTodoList(float verticalScrollSpeed, int frameCount) {
		const int delay = 2;
		int rowsToMove = Mathf.Min(frameCount-delay, gridLength);

//		// get your ducks in a row
//		for (int column = 0; column < gridLength; column++) {
//			var piece = confetti[column * 10 + rowsToMove-1];
//			var sprite = piece.GetComponent<Sprite>();
//			sprite.setScreenPosition(sprite.getScreenPosition().x, 0);
//		}
		
		for (int row = 0; row < rowsToMove; row++) {
			for (int column = 0; column < gridLength; column++) {
				var piece = confetti[column * 10 + row];
				piece.GetComponent<Sprite>().move(0f, verticalScrollSpeed);
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
		confetti = new GameObject[numberOfPieces];
		for (int i = 0; i < gridLength; i++) {
			for (int j = 0; j < gridLength; j++) {
				var n = i * 10 + j;
				confetti[n] = createConfettiPiece();
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
