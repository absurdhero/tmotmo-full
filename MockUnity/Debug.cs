using System;

namespace UnityEngine
{
	public sealed class Debug
	{
        private static void Internal_Log (int level, string msg, Object obj) {
            if (level == 0)
                Console.Out.WriteLine(msg);
            else
                Console.Error.WriteLine(msg);
        }

		public static void Log (object message)
		{
			Debug.Internal_Log (0, (message == null) ? "Null" : message.ToString (), null);
		}
		public static void Log (object message, Object context)
		{
			Debug.Internal_Log (0, (message == null) ? "Null" : message.ToString (), context);
		}
		public static void LogError (object message)
		{
			Debug.Internal_Log (2, message.ToString (), null);
		}
		public static void LogError (object message, Object context)
		{
			Debug.Internal_Log (2, message.ToString (), context);
		}
		public static void LogWarning (object message)
		{
			Debug.Internal_Log (1, message.ToString (), null);
		}
		public static void LogWarning (object message, Object context)
		{
			Debug.Internal_Log (1, message.ToString (), context);
		}
	}
}
