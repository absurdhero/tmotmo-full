using UnityEngine;

public class SceneNine : Scene {
	private UnityInput input;
	private TodoList todoList;


	public SceneNine(SceneManager manager) : base(manager) {
		input = new UnityInput();
		todoList = new TodoList(resourceFactory);
	}

	public override void Setup () {
		timeLength = 4.0f;
		todoList.Setup();
	}

	public override void Update () {
		todoList.Update(Time.time);
	}

	public override void Destroy () {
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
