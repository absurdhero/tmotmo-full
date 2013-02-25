using System;
using System.Text;
using UnityEngine;

public class MessageBox : MarshalByRefObject {
	const int lineWidth = 20;

	GameObject textLabel;
	GUIText text;
	Sprite messageBackground, leftBorder, rightBorder, topBorder, bottomBorder;

	const float borderThickness = 8f;
	const float halfThickness = borderThickness / 2f;
	const float paddingX = 4f, paddingY = 4f;

	Texture2D messageBorder;

	public MessageBox(Font font) {
		buildMessageBackground();

		textLabel = new GameObject("message text");
		textLabel.active = false;
		text = textLabel.AddComponent<GUIText>();
		textLabel.transform.position = new Vector3(0.5f, 0.5f, -9.5f);
		text.alignment = TextAlignment.Center;
		text.anchor = TextAnchor.MiddleCenter;
		text.font = font;
		text.material.SetColor("_Color", Color.black);

		messageBorder = (Texture2D) Resources.Load("messageBorder");
		leftBorder = Sprite.create(messageBorder);
		rightBorder = Sprite.create(messageBorder);
		topBorder = Sprite.create(messageBorder);
		bottomBorder = Sprite.create(messageBorder);

		// these are positive because they are relative to the camera's placement
		// the world z coordinate will be negative
		leftBorder.setDepth(9.5f);
		rightBorder.setDepth(9.5f);
		topBorder.setDepth(9.5f);
		bottomBorder.setDepth(9.5f);

		hide();
	}
	
	void buildMessageBackground() {
		messageBackground = Sprite.create((Texture2D) Resources.Load("1px"));

		messageBackground.visible(false);
		messageBackground.name = "message background";
		messageBackground.transform.position = new Vector3(0f, 0f, -9f);
	}

	public void setMessage(String message) {
		text.text = wrap(message);

		var textRect = text.GetScreenRect(Camera.main);
		expandBackgroundToSizeOf(textRect);

		leftBorder.setScreenPosition(textRect.xMin - halfThickness - paddingX, textRect.yMin - halfThickness - paddingY);
		rightBorder.setScreenPosition(textRect.xMax + halfThickness + paddingX, textRect.yMin - halfThickness - paddingY);

		var verticalBorderScale = new Vector3(1f, (textRect.height + borderThickness + paddingY * 2 + 2f) / messageBorder.height, 1f);
		leftBorder.transform.localScale =  verticalBorderScale;
		rightBorder.transform.localScale = verticalBorderScale;

		topBorder.setScreenPosition(textRect.xMin - halfThickness - paddingX, textRect.yMax + halfThickness + paddingY);
		bottomBorder.setScreenPosition(textRect.xMin - halfThickness - paddingX, textRect.yMin - halfThickness - paddingY);

		var horizontalBorderScale = new Vector3((textRect.width + borderThickness + paddingX * 2 + 2f) / messageBorder.width, 1f, 1f);
		topBorder.transform.localScale =  horizontalBorderScale;
		bottomBorder.transform.localScale = horizontalBorderScale;

	}

	void expandBackgroundToSizeOf(Rect textRect) {
		messageBackground.transform.localScale = new Vector3((textRect.width + borderThickness * 2 + paddingX * 2 + 2f) / 2f, (textRect.height + borderThickness * 2 + paddingY * 2 + 2f) / 2f, 1f);
		messageBackground.setScreenPosition(textRect.x - borderThickness - paddingX, textRect.y - borderThickness - paddingY);
	}

	static public String wrap(String text) {
		StringBuilder output = new StringBuilder(text.Length + 1);
		int spaceLeft = lineWidth;
		foreach(String word in text.Split(' ')) {
			if (word.Length + 1 > spaceLeft) {
				output.Append('\n');
				output.Append(word);
				spaceLeft = lineWidth - word.Length;
			} else {
				spaceLeft -= word.Length + 1;
				output.Append(' ');
				output.Append(word);
			}
		}
		output.Remove(0, 1); // remove space that was inserted at the beginning of the text
		return output.ToString();
	}

	public void show() {
		textLabel.active = true;
		messageBackground.visible(true);
		leftBorder.visible(true);
		rightBorder.visible(true);
		topBorder.visible(true);
		bottomBorder.visible(true);
	}

	public void hide() {
		textLabel.active = false;
		messageBackground.visible(false);
		leftBorder.visible(false);
		rightBorder.visible(false);
		topBorder.visible(false);
		bottomBorder.visible(false);
	}

}
