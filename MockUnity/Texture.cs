namespace UnityEngine
{
public interface Texture
{
  // Methods
  int GetNativeTextureID();

  // Properties
//  static int masterTextureLimit { get; set; }
//  static AnisotropicFiltering anisotropicFiltering { get; set; }
  int width { get; set; }
  int height { get; set; }
//  FilterMode filterMode { get; set; }
  int anisoLevel { get; set; }
//  TextureWrapMode wrapMode { get; set; }
  float mipMapBias { get; set; }
  Vector2 texelSize { get; }
}
}
