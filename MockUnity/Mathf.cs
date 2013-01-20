using System;

namespace UnityEngine
{
	public static class Mathf
	{
		public const float PI = 3.14159274f;

		public static float Sin(float f)
		{
			return (float) Math.Sin(f);
		}
		public static float Cos(float f)
		{
			return (float) Math.Cos(f);
		}
		public static float Tan(float f)
		{
			return (float) Math.Tan(f);
		}
		public static float Asin(float f)
		{
			return (float) Math.Asin(f);
		}
		public static float Acos(float f)
		{
			return (float) Math.Acos(f);
		}
		public static float Atan(float f)
		{
			return (float) Math.Atan(f);
		}
		public static float Atan2(float y, float x)
		{
			return (float) Math.Atan2(y, x);
		}
		public static float Sqrt(float f)
		{
			return (float) Math.Sqrt(f);
		}
		public static float Abs(float f)
		{
			return Math.Abs(f);
		}
		public static int Abs(int value)
		{
			return Math.Abs(value);
		}
		public static float Min(float a, float b)
		{
			return Math.Min(a, b);
		}
		public static float Min(params float[] values)
		{
			throw new InvalidOperationException();
		}
		public static int Min(int a, int b)
		{
			return Math.Min(a, b);
		}
		public static int Min(params int[] values)
		{
			throw new InvalidOperationException();
		}
		public static float Max(float a, float b)
		{
			return Math.Max(a, b);
		}
		public static float Max(params float[] values)
		{
			throw new InvalidOperationException();
		}
		public static int Max(int a, int b)
		{
			return Math.Max(a, b);
		}
		public static int Max(params int[] values)
		{
			throw new InvalidOperationException();
		}
		public static float Pow(float f, float p)
		{
			return (float) Math.Pow(f, p);
		}
		public static float Exp(float power)
		{
			return (float) Math.Exp(power);
		}
		public static float Log(float f, float p)
		{
			return (float) Math.Log(f, p);
		}
		public static float Log(float f)
		{
			return (float) Math.Log(f);
		}
		public static float Log10(float f)
		{
			return (float) Math.Log10(f);
		}
		public static float Ceil(float f)
		{
			return (float) Math.Ceiling(f);
		}
		public static float Floor(float f)
		{
			return (float) Math.Floor(f);
		}
		public static float Round(float f)
		{
			return (float) Math.Round((double)f);
		}
		public static int CeilToInt(float f)
		{
			return (int) Math.Ceiling(f);
		}
		public static int FloorToInt(float f)
		{
			return (int) Math.Floor(f);
		}
		public static int RoundToInt(float f)
		{
			return (int) Math.Round(f);
		}
		public static float Sign(float f)
		{
			return Math.Sign(f);
		}
		public static float Clamp(float value, float min, float max)
		{
			throw new InvalidOperationException();
		}
		public static int Clamp(int value, int min, int max)
		{
			throw new InvalidOperationException();
		}
		public static float Clamp01(float value)
		{
			throw new InvalidOperationException();
		}
		public static float Lerp(float from, float to, float t)
		{
			throw new InvalidOperationException();
		}
		public static float LerpAngle(float a, float b, float t)
		{
			throw new InvalidOperationException();
		}
		public static float MoveTowards(float current, float target, float maxDelta)
		{
			throw new InvalidOperationException();
		}
		public static float MoveTowardsAngle(float current, float target, float maxDelta)
		{
			throw new InvalidOperationException();
		}
		public static float SmoothStep(float from, float to, float t)
		{
			throw new InvalidOperationException();
		}
		public static float Gamma(float value, float absmax, float gamma)
		{
			throw new InvalidOperationException();
		}
		public static bool Approximately(float a, float b)
		{
			throw new InvalidOperationException();
		}
		public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed)
		{
			throw new InvalidOperationException();
		}
		public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime)
		{
			throw new InvalidOperationException();
		}
		public static float SmoothDamp(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
		{
			throw new InvalidOperationException();
		}
		public static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed)
		{
			throw new InvalidOperationException();
		}
		public static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime)
		{
			throw new InvalidOperationException();
		}
		public static float SmoothDampAngle(float current, float target, ref float currentVelocity, float smoothTime, float maxSpeed, float deltaTime)
		{
			throw new InvalidOperationException();
		}
		public static float Repeat(float t, float length)
		{
			throw new InvalidOperationException();
		}
		public static float PingPong(float t, float length)
		{
			throw new InvalidOperationException();
		}
		public static float InverseLerp(float from, float to, float value)
		{
			throw new InvalidOperationException();
		}
	}
}

