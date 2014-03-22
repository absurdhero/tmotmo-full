using UnityEngine;
using System;

public class Prompt : MarshalByRefObject {
	GameObject textLabel, blackBox;
	GUIText text;

	public Prompt(GameObject textLabel, GUIText text) {
		this.textLabel = textLabel;
		this.text = text;
	}
	
	public Prompt build() {
		blackBox = GameObject.CreatePrimitive(PrimitiveType.Plane);
		blackBox.SetActive(false);
		blackBox.name = "prompt background";
		blackBox.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.black);
		blackBox.transform.Rotate(new Vector3(270f, 0f, 0f));
		blackBox.transform.position = new Vector3(0, -95, -9);
		blackBox.transform.localScale = new Vector3(30f, 1f, 1.5f);
		return this;
	}
	
	public void Destroy() {
		GameObject.Destroy(textLabel);
	}

	public void setText(string action) {
		text.text = ">" + action + "_";
	}
	
	public void show() {
		blackBox.SetActive(true);
		textLabel.SetActive(true);
	}
	
	public void hide() {
		blackBox.SetActive(false);
		textLabel.SetActive(false);
	}
}
