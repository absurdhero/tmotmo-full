using UnityEngine;
using System;

public interface GameObjectFactory<T> {
	GameObject Create (object obj, T identifier);
	GameObject Create (T identifier);
}

public class ResourceFactory : GameObjectFactory<string> {
	public GameObject Create(object obj, string name) {
		string path = resourcePath(obj, name);
		return Create(path);
	}

	public GameObject Create(string resourcePath) {
		return (GameObject)GameObject.Instantiate(Resources.Load(resourcePath));
	}
	
	protected string resourcePath(object obj, string name) {
		var resourcePrefix = obj.GetType().ToString();
		return resourcePrefix +"/" + name;
	}
}
