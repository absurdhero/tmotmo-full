namespace UnityEngine {
	public abstract class Renderer : Component {
	}
	
    public abstract class MeshRenderer : Renderer {
        public Material material;
        public bool receiveShadows;
        public bool castShadows;

        public Material sharedMaterial { get { return material; } }
    }
}
