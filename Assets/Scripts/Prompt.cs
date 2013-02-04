using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class Prompt : MarshalByRefObject {
	GameObject textLabel, blackBox;
	MessageBox messageBox;
	GUIText text;
	bool enabled;
	float startTime = 0f;

	List<string> remainingMessages = new List<string>();
	Action<GameObject> onComplete = GameObject => {};
	GameObject target;
	
	const float promptTime = 1.5f;
	const float boxTime = 2.0f;
	
	public Prompt() {
	}
	
	public void Setup() {
		buildBlackBox();

		textLabel = new GameObject("prompt text");
		textLabel.active = false;
		text = textLabel.AddComponent<GUIText>();
		textLabel.transform.position = new Vector3(0f, 0.06f, -9.5f);
		Font font = (Font) Resources.Load("sierra_agi_font/sierra_agi_font", typeof(Font));
		text.font = font;

		messageBox = new MessageBox(font);
	}

	void buildBlackBox() {
		blackBox = GameObject.CreatePrimitive(PrimitiveType.Plane);
		blackBox.active = false;
		blackBox.name = "prompt background";
		blackBox.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.black);
		blackBox.transform.Rotate(new Vector3(270f, 0f, 0f));
		blackBox.transform.position = new Vector3(0, -95, -9);
		blackBox.transform.localScale = new Vector3(30f, 1f, 1.5f);
	}

	public void Update(float time) {
		if (!enabled) return;
		
		if (time > startTime + promptTime) {
			hide();
			messageBox.show();
			if (remainingMessages.Count == 0) {
				onComplete(target);
				onComplete = (obj) => {}; // only run it once
			}
		}
		if (time > startTime + promptTime + boxTime && remainingMessages.Count == 0) {
			hide();
			enabled = false;
			hideBoxes();
		}
	}

	public void Reset() {
		hide();
		hideBoxes();
		enabled = false;
	}
	
	public void Destroy() {
		GameObject.Destroy(textLabel);
	}

	public void solve(Scene scene, string action) {
		print(action, "OK");
	}

	public void progress(string action) {
		print(action, "OK");
	}

	public void hint(string action, string message) {
		hint(action, new List<string> {message});
	}

	public void hint(string action, List<string> messages) {
		remainingMessages = messages;
	}

	// Cycles through displaying a sequence of action-response pairs for each object that is touched.
	// Calls onComplete when it reaches the end of a cycle with the object for which the sequence was completed.
	public void hintWhenTouched(Action<GameObject> onComplete, TouchSensor sensor, Dictionary<GameObject, ActionResponsePair[]> interactions) {
		if (remainingMessages.Count > 0) {
			if (sensor.any()) {
				messageBox.setMessage(remainingMessages[0]);
				remainingMessages.RemoveAt(0);
				startTime = Time.time - promptTime;
			}
			return;
		}

		GameObject touchedObject = null;
		foreach(var gameObject in interactions.Keys) {
			if (sensor.insideSprite(Camera.main, gameObject.GetComponent<Sprite>(), new[] {TouchPhase.Began})) {
				touchedObject = gameObject;
			}
		}

		if (touchedObject == null) return;
		
		var message = interactions[touchedObject][0];
		var promptInput = message.action;
		var responses = message.responses;
		print(promptInput, responses[0]);

		var restOfresponses = new List<string>(responses).Skip(1).ToList();
		hint(promptInput, restOfresponses);

		if (interactions[touchedObject].Length > 1) {
			interactions[touchedObject] = interactions[touchedObject].Skip(1).ToArray();
		} else {
			target = touchedObject;
			this.onComplete = onComplete;
		}
	}

	private void print(string action, string message) {
		messageBox.setMessage(message);
		startTime = Time.time;
		hideBoxes();
		show();
		text.text = ">" + action + "_";
	}
	
	private void show() {
		enabled = true;
		blackBox.active = true;
		textLabel.active = true;
	}
	
	private void hide() {
		blackBox.active = false;
		textLabel.active = false;
	}

	void hideBoxes() {
		messageBox.hide();
	}
}

public class ActionResponsePair {
	public string action {get;set;}
	public string[] responses {get;set;}
	
	public ActionResponsePair(string action, string[] responses) {
		this.action = action;
		this.responses = responses;
	}
}
