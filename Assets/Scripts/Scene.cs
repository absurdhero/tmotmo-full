using System;
using System.Collections;
using UnityEngine;

public interface Scene
{
	float rewindTime { get; }
	float TimeLength();
	bool solved { get; }
	bool completed { get; }
	bool permitUnloadResources { get; }

	void LoadAssets();
	void Setup(float startTime);
	void Update();
	void Destroy();
	void Transition();
	void endScene();
}

