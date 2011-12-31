namespace UnityEngine {
    public abstract class GameObject {
        abstract public T AddComponent<T> ();
        abstract public T GetComponent<T> ();

        public Transform transform;
    }
}
