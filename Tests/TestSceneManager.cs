using System;
using System.Reflection;

using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using UnityEngine;
using NMock;

namespace Tests
{
	[TestFixture]
	public class TestSceneManager
	{
	    private Mock<AbstractLoopTracker> loopTracker;
		private Mock<AbstractSceneFactory> sceneFactory;
		private MockFactory factory;

		[SetUp]
		public void SetUp ()
		{
			factory = new MockFactory();
			loopTracker = factory.CreateMock<AbstractLoopTracker>();
			sceneFactory = factory.CreateMock<AbstractSceneFactory>();
		}

		[Test]
		public void firstSceneEndingTriggersLoopStart()
		{
			var firstScene = factory.CreateMock<Scene>(MockStyle.Stub);
			var lastScene = factory.CreateMock<Scene>(MockStyle.Stub);
			var messagePromptCoordinator = new MessagePromptCoordinator(null, null);

			firstScene.Stub.Out.Method(_ => _.TimeLength()).WillReturn(0.0f);

			// emulate behavior of sceneFactory and return mock Scenes
			sceneFactory.Expects.AtLeastOne.Method(_ => _.PreloadAssets());
			sceneFactory.Expects.AtLeastOne.Method(_ => _.GetFirstScene()).WillReturn(firstScene.MockObject);
			sceneFactory.Expects.AtLeastOne.MethodWith(_ => _.isLastScene(firstScene.MockObject)).WillReturn(false);
			loopTracker.Expects.One.Method(_ => _.startPlaying());
			loopTracker.Expects.One.Method(_ => _.Rewind(0f)).WithAnyArguments();
			loopTracker.Expects.One.Method(_ => _.NextLoop(0f)).WithAnyArguments();
			loopTracker.Expects.One.Method(_ => _.Stop());
			sceneFactory.Expects.One.MethodWith(_ => _.GetSceneAfter(firstScene.MockObject)).WillReturn(lastScene.MockObject);
			sceneFactory.Expects.AtLeastOne.MethodWith(_ => _.isLastScene(lastScene.MockObject)).WillReturn(true);
			sceneFactory.Expects.AtLeastOne.Method(_ => _.Reset());

			var sceneManager = new SceneManager(sceneFactory.MockObject, loopTracker.MockObject, messagePromptCoordinator);
			sceneManager.NextScene();
			sceneManager.NextScene();
		}
	}
}
