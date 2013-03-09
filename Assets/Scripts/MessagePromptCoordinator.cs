using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class MessagePromptCoordinator : MarshalByRefObject {
	Prompt prompt;
	MessageBox messageBox;
	bool displayingAnything;
	
	float actionPrintedAt = 0f;

	public const float promptTime = 1.5f;
	public const float boxTime = 2.0f;
	
	List<string> remainingMessages = new List<string>();
	Action<GameObject> onComplete = GameObject => {};
	GameObject target;
	GameObject touchedObject;

	public MessagePromptCoordinator(Prompt prompt, MessageBox messageBox) {
		this.prompt = prompt;
		this.messageBox = messageBox;
	}

	public void Update(float time) {
		if (!displayingAnything) return;
		
		if (time > actionPrintedAt + promptTime) {
			prompt.hide();
			messageBox.show();
			if (remainingMessages.Count == 0) {
				onComplete(target);
				onComplete = (obj) => {}; // only run it once
			}
		}
		if (time > actionPrintedAt + promptTime + boxTime && remainingMessages.Count == 0) {
			prompt.hide();
			messageBox.hide();
			displayingAnything = false;
		}
	}

	public void Reset() {
		touchedObject = null;
		prompt.hide();
		messageBox.hide();
		displayingAnything = false;
	}

	public void solve(Scene scene, string action) {
		actionPrintedAt = Time.time;
		print(action, "OK");
	}

	public void progress(string action) {
		actionPrintedAt = Time.time;
		print(action, "OK");
	}

	public void hint(string action, string message) {
		hint(action, new List<string> {message});
	}

	public void hint(string action, List<string> messages) {
		remainingMessages = messages;
	}

	private void print(string action, string message) {
		messageBox.setMessage(message);
		messageBox.hide();
		displayingAnything = true;
		prompt.show();
		prompt.setText(action);
	}
	
	public void clearTouch() {
		touchedObject = null;
	}
	
	// Cycles through displaying a sequence of action-response pairs for each object that is touched.
	// Calls onComplete when it reaches the end of a cycle with the object for which the sequence was completed.
	public void hintWhenTouched(Action<GameObject> onComplete, TouchSensor sensor, float currentTime, Dictionary<GameObject, ActionResponsePair[]> interactions) {
		//if (!sensor.hasTaps()) return; // only act on touches
		
		// if there was an interaction but a dialog has not shown yet...
		if (currentTime <= actionPrintedAt + promptTime && touchedObject != null) {
			// ignore a tap if it's been at least a half second since prompting
			if (sensor.hasTaps() && currentTime > actionPrintedAt + 0.5f) {
				return;
			}
			// ignore touches of the same object before the first dialog is shown
			if (touchedObject != null && sensor.insideSprite(Camera.main, touchedObject.GetComponent<Sprite>(), TouchSensor.allPhases)) {
				return;
			}
		}
		
		// if a dialog was just shown, let it sit there for a moment
		if (currentTime <= actionPrintedAt + promptTime + 0.5f) {
			return;
		}
		
		// cycle through remaining dialogs
		if (remainingMessages.Count > 0) {
			if (sensor.hasTaps()) {
				messageBox.setMessage(remainingMessages[0]);
				remainingMessages.RemoveAt(0);
				actionPrintedAt = currentTime - promptTime;
			}
			return;
		}
		
		// start an action-dialog sequence if they touched an interactive object

		touchedObject = detectObjectInteraction(sensor, interactions);

		if (touchedObject == null) return;
		
		var message = interactions[touchedObject][0];
		var action = message.action;
		var responses = message.responses;
		actionPrintedAt = currentTime;
		print(action, responses[0]);

		var restOfresponses = new List<string>(responses).Skip(1).ToList();
		hint(action, restOfresponses);

		if (interactions[touchedObject].Length > 1) {
			interactions[touchedObject] = interactions[touchedObject].Skip(1).ToArray();
		} else {
			target = touchedObject;
			this.onComplete = onComplete;
		}
	}

	private GameObject detectObjectInteraction(TouchSensor sensor, Dictionary<GameObject, ActionResponsePair[]> interactions) {
		GameObject touched = null;
		foreach(var gameObject in interactions.Keys) {
			if (sensor.insideSprite(Camera.main, gameObject.GetComponent<Sprite>(), new[] {TouchPhase.Began})) {
				touched = gameObject;
			}
		}
		return touched;
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
