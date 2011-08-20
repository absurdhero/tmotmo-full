using System;

namespace UnityEngine
{
public static class Input
{

  // Methods
  public static float GetAxis(string axisName) { throw new InvalidOperationException(); }
  public static float GetAxisRaw(string axisName) { throw new InvalidOperationException(); }
  public static bool GetButton(string buttonName) { throw new InvalidOperationException(); }
  public static bool GetButtonDown(string buttonName) { throw new InvalidOperationException(); }
  public static bool GetButtonUp(string buttonName) { throw new InvalidOperationException(); }
  public static bool GetKey(string name) { throw new InvalidOperationException(); }
//  public static bool GetKey(KeyCode key) { throw new InvalidOperationException(); }
  public static bool GetKeyDown(string name) { throw new InvalidOperationException(); }
//  public static bool GetKeyDown(KeyCode key) { throw new InvalidOperationException(); }
  public static bool GetKeyUp(string name) { throw new InvalidOperationException(); }
//  public static bool GetKeyUp(KeyCode key) { throw new InvalidOperationException(); }
  public static string[] GetJoystickNames() { throw new InvalidOperationException(); }
  public static bool GetMouseButton(int button) { throw new InvalidOperationException(); }
  public static bool GetMouseButtonDown(int button) { throw new InvalidOperationException(); }
  public static bool GetMouseButtonUp(int button) { throw new InvalidOperationException(); }
  public static void ResetInputAxes() { throw new InvalidOperationException(); }
//  public static AccelerationEvent GetAccelerationEvent(int index) { throw new InvalidOperationException(); }
  public static Touch GetTouch(int index) { throw new InvalidOperationException(); }
//  public static Quaternion GetRotation(int deviceID) { throw new InvalidOperationException(); }
  public static Vector3 GetPosition(int deviceID) { throw new InvalidOperationException(); }

  // Properties
  public static Vector3 mousePosition { get { throw new InvalidOperationException(); } }
  public static bool anyKey { get { throw new InvalidOperationException(); } }
  public static bool anyKeyDown { get { throw new InvalidOperationException(); } }
  public static string inputString { get { throw new InvalidOperationException(); } }
  public static Vector3 acceleration { get { throw new InvalidOperationException(); } }
//  public static AccelerationEvent[] accelerationEvents { get { throw new InvalidOperationException(); } }
  public static int accelerationEventCount { get { throw new InvalidOperationException(); } }
  public static Touch[] touches { get { return new Touch[0]; } }
  public static int touchCount { get { throw new InvalidOperationException(); } }
  public static bool multiTouchEnabled { get { throw new InvalidOperationException(); } } // set; }
//  public static DeviceOrientation deviceOrientation { get { throw new InvalidOperationException(); } }
}
}
