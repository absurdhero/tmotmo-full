using UnityEngine;
using System;

public class SceneNine : Scene {
	public Confetti confetti;

	private UnityInput input;
	private TodoList todoList;

	public SceneNine(SceneManager manager, Confetti confetti) : base(manager) {
		this.confetti = confetti;
		input = new UnityInput();
		todoList = new TodoList(resourceFactory);
	}

	public override void Setup () {
		timeLength = 2.0f;
		todoList.Setup();
	}

	public override void Update () {
		todoList.Update(Time.time);
	}

	public override void Destroy () {
		confetti.Destroy();
		todoList.Destroy();
	}

	public class TodoList {
		GameObject background;
		GameObjectFactory<string> resourceFactory;
		
		public TodoList(GameObjectFactory<string> resourceFactory) {
			this.resourceFactory = resourceFactory;
		}
		
		public void Setup() {
			background = resourceFactory.Create("TodoList/GreenQuad");
		}

		public void Update(float time) {
		}
		
		public void Destroy() {
			GameObject.Destroy(background);
		}
	}
}
