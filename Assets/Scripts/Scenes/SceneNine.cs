using UnityEngine;
using System;

public class SceneNine : Scene {
	public Confetti confetti;

	private UnityInput input;
	GameObject background;
	private TodoList todoList;

	public SceneNine(SceneManager manager, Confetti confetti) : base(manager) {
		this.confetti = confetti;
		input = new UnityInput();
		background = resourceFactory.Create("TodoList/GreenQuad");
		todoList = new TodoList(resourceFactory, confetti);
	}

	public override void Setup () {
		timeLength = 2.0f;
		todoList.Setup();
	}

	public override void Update () {
		todoList.Update(Time.time);

		if (todoList.piecesAreSpreadOut()) {
			endScene();
		}
	}

	public override void Destroy () {
		confetti.Destroy();
		GameObject.Destroy(background);
		todoList.Destroy();
	}

	public class TodoList {
		GameObjectFactory<string> resourceFactory;
		Confetti confetti;

		const int VERTICAL_STOP_POSITION = 35;
		const int SCROLL_SPEED = 20;

		Metronome metronome;
		GameObject intactTodoList;
		Sprite intactTodoListSprite { get { return intactTodoList.GetComponent<Sprite>(); } }
		GameObject tornListLeft, tornListRight, tornListBottom;
		Sprite tornListLeftSprite { get { return tornListLeft.GetComponent<Sprite>(); } }
		Sprite tornListRightSprite { get { return tornListRight.GetComponent<Sprite>(); } }
		Sprite tornListBottomSprite { get { return tornListBottom.GetComponent<Sprite>(); } }
		
		Dragger leftDrag, rightDrag, bottomDrag;
		
		bool tearable = false;
		
		public TodoList(GameObjectFactory<string> resourceFactory, Confetti confetti) {
			this.resourceFactory = resourceFactory;
			this.confetti = confetti;
		}
		
		public void Setup() {
			intactTodoList = resourceFactory.Create("TodoList/todo");
			intactTodoListSprite.setScreenPosition(200, -intactTodoListSprite.height);
			metronome = new Metronome(Time.time, 0.125f);
		}

		public void Update(float time) {
			if (tearable) {
				tornListLeftSprite.move(leftDrag.movementIfDragged());
				tornListRightSprite.move(rightDrag.movementIfDragged());
				tornListBottomSprite.move(bottomDrag.movementIfDragged());
				return;
			}
			if (intactTodoListSprite.getScreenPosition().y >= VERTICAL_STOP_POSITION) {
				replaceListWithTornPieces();
				return;
			}
			
			if (metronome.isNextTick(time)) {
				intactTodoListSprite.move(0, SCROLL_SPEED);
			}
		}
		
		public void Destroy() {
			GameObject.Destroy(intactTodoList);
			GameObject.Destroy(tornListLeft);
			GameObject.Destroy(tornListRight);
			GameObject.Destroy(tornListBottom);
		}

		public void replaceListWithTornPieces() {
			tearable = true;
			
			tornListLeft = resourceFactory.Create("TodoList/tornListLeft");
			tornListRight = resourceFactory.Create("TodoList/tornListRight");
			tornListBottom = resourceFactory.Create("TodoList/tornListBottom");
			
			tornListLeft.transform.position = intactTodoList.transform.position;
			tornListRight.transform.position = intactTodoList.transform.position;
			tornListBottom.transform.position = intactTodoList.transform.position;
			
			leftDrag = new Dragger(tornListLeftSprite);
			rightDrag = new Dragger(tornListRightSprite);
			bottomDrag = new Dragger(tornListBottomSprite);

			tornListLeftSprite.move(-10, 85);
			tornListRightSprite.move(104, 110);
			tornListBottomSprite.move(10, 0);
			
			intactTodoList.active = false;
			
			confetti.Deactivate();
		}

		public bool piecesAreSpreadOut() {
			if (!tearable) return false;
			
			if (leftDrag.totalDragDistance().magnitude > 50
				&& rightDrag.totalDragDistance().magnitude > 50
				&& bottomDrag.totalDragDistance().magnitude > 50)
				return true;
			return false;
		}
	}
}
