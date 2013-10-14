using System;
using UnityEngine;

public class ScrollingLetters {
	Sprite[] letters, leftLetters, rightLetters;
	Texture2D letterAtlas;
	
	const int gridHeight = 12;
	const int gridWidth = 5;
	const int rowLength = 24;
	const int numRows = 6;
	const int numColumns = 9;
	const int columnLength = 13;
	const int numberOfBottomPieces = rowLength * numRows;
	const int numberOfPiecesPerSide = columnLength * numColumns;
	const int letterHeight = 16;
	const int letterWidth = 16;
	
	const int finalHeightOfBottomRows = numRows * letterHeight;
	
	Grid letterGrid = new Grid(16f, 16f);
	
	private const int ceiling = 100;
	public bool pouring { get; private set; }
	public bool finishedPouring { get; private set; }
	
	Metronome scrollingFrequency;
	
	public ScrollingLetters() {
	}

	public void Setup() {
		letters = new Sprite[numberOfBottomPieces];
		leftLetters = new Sprite[numberOfPiecesPerSide];
		rightLetters = new Sprite[numberOfPiecesPerSide];
		letterAtlas = (Texture2D) Resources.Load("TodoList/ksLetters");

		createLetterRow(0);
		scrollingFrequency = new Metronome(Time.time, 0.125f);
	}

	public void Update(float time) {
		if (scrollingFrequency.isNextTick(time)) {
			scrollLetters(time);
			if (scrollingFrequency.nextTick > 5) {
				scrollSides(time);
			}
		}		

	}

	void scrollLetters(float time) {
		var row = scrollingFrequency.nextTick + 1;
		if (row < numRows) {
			createLetterRow(row);
			for (int i = 0; i < letters.Length; i++) {
				if (letters[i] == null) break;
				letters[i].move(Vector3.up * letterHeight);
			}
		}
	}
	
	void scrollSides(float time) {
		var column = scrollingFrequency.nextTick - numRows;
		if (column < numColumns) {
			createLeftAndRightColumns(column);
			for (int i = 0; i < leftLetters.Length; i++) {
				if (leftLetters[i] == null) break;
				leftLetters[i].move(Vector3.right * letterWidth);
			}
			for (int i = 0; i < rightLetters.Length; i++) {
				if (rightLetters[i] == null) break;
				rightLetters[i].move(Vector3.left * letterWidth);
			}
		}
	}
	
	public void Deactivate() {
		if (letters == null) return;
		foreach (var letter in letters) {
			letter.gameObject.SetActive(false);
		}
	}

	public void Destroy() {
		if (letters == null) return;
		foreach (var letter in letters) {
			if (letter == null) continue;
			Sprite.Destroy(letter);
		}
		foreach (var letter in leftLetters) {
			if (letter == null) continue;
			Sprite.Destroy(letter);
		}
		foreach (var letter in rightLetters) {
			if (letter == null) continue;
			Sprite.Destroy(letter);
		}
	}
	
	void createLeftAndRightColumns(int i) {
		for (int row = 0; row < columnLength; row++) {
			var n = row + i * columnLength;
			leftLetters[n] = createLetter(letterAtlas);
			setRandomLetterTexture(leftLetters[n]);
			leftLetters[n].move(new Vector3(0, finalHeightOfBottomRows + letterHeight * row));
		}

		for (int row = 0; row < columnLength; row++) {
			var n = row + i * columnLength;
			rightLetters[n] = createLetter(letterAtlas);
			setRandomLetterTexture(rightLetters[n]);
			rightLetters[n].move(new Vector3(Camera.main.pixelWidth-letterHeight, finalHeightOfBottomRows + letterHeight * row));
		}
	}

	void createLetterRow(int i) {
		for (int column = 0; column < rowLength; column++) {
			var n = column + i * rowLength;
			letters[n] = createLetter(letterAtlas);
			setRandomLetterTexture(letters[n]);
			letters[n].move(new Vector3(20 * column, 0));
		}
	}

	void setRandomLetterTexture(Sprite letterSprite) {
		//letters[n].imageMaterial.SetUVToGridCell(letterGrid, 0, 0);
		int textureNumber = UnityEngine.Random.Range(0, gridWidth * gridHeight);
		letterSprite.imageMaterial.SetUVToGridCell(letterGrid, textureNumber / gridWidth % gridWidth, textureNumber % gridHeight);
	}

	Sprite createLetter(Texture2D texture) {
		GameObject piece = new GameObject("glitch letters");
		Sprite sprite = piece.AddComponent<Sprite>();
		sprite.height = letterHeight;
		sprite.width = letterHeight;
		sprite.setScreenPosition(0, 0);
		sprite.textures = new Texture2D[] {
				texture
			};
		sprite.Awake();
		sprite.Start();	

		return sprite;
	}
}
