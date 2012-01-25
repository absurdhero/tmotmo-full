using System;

namespace UnityEngine
{
	public class Application
	{
		public static int loadedLevel;
		public static string loadedLevelName;
		public static bool isLoadingLevel;
		public static int levelCount;
		public static int streamedBytes;
		public static bool isPlaying;
		public static bool isEditor;
		public static bool isWebPlayer;
		//public static RuntimePlatform platform;
		public static bool runInBackground;
		public static bool isPlayer;
		public static string dataPath;
		public static string streamingAssetsPath;
		public static string persistentDataPath;
		public static string temporaryCachePath;
		public static string srcValue;
		public static string absoluteURL;
		public static string absoluteUrl;
		public static string unityVersion;
		public static bool webSecurityEnabled;
		public static string webSecurityHostUrl;
		public static int targetFrameRate;
		//public static SystemLanguage systemLanguage;
		//public static ThreadPriority backgroundLoadingPriority;
		//public static NetworkReachability internetReachability;
		public static bool genuine;
		public static bool genuineCheckAvailable;
		public static void Quit () { throw new InvalidOperationException(); }
		public static void CancelQuit () { throw new InvalidOperationException(); }
		public static void LoadLevel (int index) { throw new InvalidOperationException(); }
		public static void LoadLevel (string name) { throw new InvalidOperationException(); }
		public static AsyncOperation LoadLevelAsync (int index) { throw new InvalidOperationException(); }
		public static AsyncOperation LoadLevelAsync (string levelName) { throw new InvalidOperationException(); }
		public static AsyncOperation LoadLevelAdditiveAsync (int index) { throw new InvalidOperationException(); }
		public static AsyncOperation LoadLevelAdditiveAsync (string levelName) { throw new InvalidOperationException(); }
		public static void LoadLevelAdditive (int index) { throw new InvalidOperationException(); }
		public static void LoadLevelAdditive (string name) { throw new InvalidOperationException(); }
		public static float GetStreamProgressForLevel (int levelIndex) { throw new InvalidOperationException(); }
		public static float GetStreamProgressForLevel (string levelName) { throw new InvalidOperationException(); }
		public static bool CanStreamedLevelBeLoaded (int levelIndex) { throw new InvalidOperationException(); }
		public static bool CanStreamedLevelBeLoaded (string levelName) { throw new InvalidOperationException(); }
		public static void CaptureScreenshot (string filename, int superSize) { throw new InvalidOperationException(); }
		public static void CaptureScreenshot (string filename) { throw new InvalidOperationException(); }
		public static void DontDestroyOnLoad (Object mono) { throw new InvalidOperationException(); }
		public static void ExternalCall (string functionName, params object[] args) { throw new InvalidOperationException(); }
		public static void ExternalEval (string script) { throw new InvalidOperationException(); }
		public static void OpenURL (string url) { throw new InvalidOperationException(); }
		public static void CommitSuicide (int mode) { throw new InvalidOperationException(); }
		/*
		public static void RegisterLogCallback (Application.LogCallback handler) { throw new InvalidOperationException(); }
		public static void RegisterLogCallbackThreaded (Application.LogCallback handler) { throw new InvalidOperationException(); }
		public static AsyncOperation RequestUserAuthorization (UserAuthorization mode) { throw new InvalidOperationException(); }
		public static bool HasUserAuthorization (UserAuthorization mode) { throw new InvalidOperationException(); }
		 */
	}
}