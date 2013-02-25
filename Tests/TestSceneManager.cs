using System;
using System.Reflection;

using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using UnityEngine;

namespace Tests
{
	[TestFixture]
	public class TestSceneManager
	{
	    private LoopTracker loopTracker;
		private SceneFactory sceneFactory;
    	private MockRepository mocks;

		[SetUp]
		public void SetUp ()
		{
			mocks = new MockRepository();
			loopTracker = mocks.DynamicMock<LoopTracker>();
			sceneFactory = mocks.DynamicMock<SceneFactory>();
		}

		[Test]
		public void firstSceneEndingTriggersLoopStart()
		{
			var firstScene = mocks.DynamicMock<Scene>();
			var lastScene = mocks.DynamicMock<Scene>();
			var messagePromptCoordinator = mocks.Stub<MessagePromptCoordinator>();
			using (mocks.Record ()) {
				// emulate behavior of sceneFactory and return mock Scenes
				Expect.Call(sceneFactory.GetFirstScene()).Return(firstScene);
				Expect.Call(sceneFactory.isLastScene(firstScene)).Return(false);
				Expect.Call(sceneFactory.isFirstScene(firstScene)).Return(true);
				Expect.Call(delegate{loopTracker.startPlaying();});
				Expect.Call(sceneFactory.GetSceneAfter(firstScene)).Return(lastScene);
				Expect.Call(sceneFactory.isLastScene(lastScene)).Return(true);
			}
			using (mocks.Playback ()) {
				var sceneManager = new SceneManager(sceneFactory, loopTracker, messagePromptCoordinator);
				sceneManager.NextScene();
				sceneManager.NextScene();
			}

		}
	}
}
