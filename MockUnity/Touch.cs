namespace UnityEngine
{
public interface Touch
{
  // Properties
  int fingerId { get; }
  Vector2 position { get; }
  Vector2 deltaPosition { get; }
  float deltaTime { get; }
  int tapCount { get; }
  TouchPhase phase { get; }
}
}