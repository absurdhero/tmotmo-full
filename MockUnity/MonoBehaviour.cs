using System;
namespace UnityEngine
{
  public abstract class MonoBehaviour
  {
  public void Invoke(string methodName, float time) { throw new InvalidOperationException(); }

  public void InvokeRepeating(string methodName, float time, float repeatRate) {  throw new InvalidOperationException(); }

  public void CancelInvoke() { throw new InvalidOperationException(); }

  public void CancelInvoke(string methodName) { throw new InvalidOperationException(); }

  public bool IsInvoking(string methodName) { throw new InvalidOperationException(); }

  public bool IsInvoking() { throw new InvalidOperationException(); }

//  public Coroutine StartCoroutine(IEnumerator routine) { throw new InvalidOperationException(); }

//  public Coroutine StartCoroutine_Auto(IEnumerator routine) { throw new InvalidOperationException(); }

//  public Coroutine StartCoroutine(string methodName, object value) { throw new InvalidOperationException(); }

//  public Coroutine StartCoroutine(string methodName) { throw new InvalidOperationException(); }

  public void StopCoroutine(string methodName) { throw new InvalidOperationException(); }

  public void StopAllCoroutines() { throw new InvalidOperationException(); }

  public static void print(object message) { throw new InvalidOperationException(); }



  // Properties

  public bool useGUILayout { get; set; }

  
  }
}
