namespace UnityEngine {
    public abstract class MeshRenderer {
        public Material material;
        public bool receiveShadows;
        public bool castShadows;

        public Material sharedMaterial { get { return material; } }
    }
}
