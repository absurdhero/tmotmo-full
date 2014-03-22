using UnityEngine;
using System;

public class SceneNine : AbstractScene {
	public Confetti confetti;
	ScrollLetters spreadConfetti;
	Sprite todoList;

	FullScreenQuad background;

	public SceneNine(SceneManager manager, Confetti confetti) : base(manager) {
		timeLength = 2.0f;
		this.confetti = confetti;
	}

	public override void Setup (float startTime) {
		endScene();

		background = FullScreenQuad.create("TodoList/todo_bg");

		spreadConfetti = new ScrollLetters(confetti);
		spreadConfetti.Setup(startTime);

		todoList = Sprite.create("TodoList/todo");
		todoList.setCenterToViewportCoord(0.5f, 0.5f);
		todoList.setDepth(-1);
	}

	public override void Update () {
		spreadConfetti.Update(Time.time);
	}

	public override void Destroy () {
		confetti.Destroy();
		FullScreenQuad.Destroy(background);
		spreadConfetti.Destroy();
		Sprite.Destroy(todoList);
	}

	public class ScrollLetters {
		Confetti confetti;
		ScrollingLetters scrollingLetters;

		const int SCROLL_SPEED = 20;

		Metronome metronome;
		
		public ScrollLetters(Confetti confetti) {
			this.confetti = confetti;
		}
		
		public void Setup(float startTime) {
			metronome = new Metronome(startTime, 0.125f);
			confetti.ensureConfettiWasPoured();
			scrollingLetters = new ScrollingLetters();
			scrollingLetters.Setup();
		}

		public void Update(float time) {
			
			if (metronome.isNextTick(time)) {
				hidePiecesForTick(metronome.nextTick);
				scrollingLetters.Update(time);
			}
		}
		
		private void hidePiecesForTick(int currentTick) {
			for(int i = 0; i < Confetti.gridLength; i++) {
				int pieceNum = i * Confetti.gridLength + currentTick;
				if (pieceNum >= Confetti.numberOfPieces) return;
				confetti.hidePiece(pieceNum);
			}
		}
		
		public void Destroy() {
			scrollingLetters.Destroy();
		}
	}
}
