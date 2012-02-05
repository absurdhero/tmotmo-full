using UnityEngine;
using System;

public interface GameObjectFactory<T> {
	GameObject Create (T identifier);
}

public class ResourceFactory : GameObjectFactory<string> {
	public GameObject Create(string resourcePath) {
		return (GameObject)GameObject.Instantiate(Resources.Load(resourcePath));
	}
}
