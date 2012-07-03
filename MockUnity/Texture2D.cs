namespace UnityEngine
{

public interface Texture2D : Texture
{

  // Methods
  void SetPixel(int x, int y, Color color);
  Color GetPixel(int x, int y);
  Color GetPixelBilinear(float u, float v);
  void SetPixels(Color[] colors);
  void SetPixels(Color[] colors, int miplevel);
  void SetPixels(int x, int y, int blockWidth, int blockHeight, Color[] colors, int miplevel);
  void SetPixels(int x, int y, int blockWidth, int blockHeight, Color[] colors);
  bool LoadImage(byte[] data);
  Color[] GetPixels();
  Color[] GetPixels(int miplevel);
  Color[] GetPixels(int x, int y, int blockWidth, int blockHeight, int miplevel);
  Color[] GetPixels(int x, int y, int blockWidth, int blockHeight);
  void Apply(bool updateMipmaps, bool makeNoLongerReadable);
  void Apply(bool updateMipmaps);
  void Apply();
  //bool Resize(int width, int height, TextureFormat format, bool hasMipMap);
  bool Resize(int width, int height);
  void Compress(bool highQuality);
  Rect[] PackTextures(Texture2D[] textures, int padding, int maximumAtlasSize, bool makeNoLongerReadable);
  Rect[] PackTextures(Texture2D[] textures, int padding, int maximumAtlasSize);
  Rect[] PackTextures(Texture2D[] textures, int padding);
  void ReadPixels(Rect source, int destX, int destY, bool recalculateMipMaps);
  void ReadPixels(Rect source, int destX, int destY);
  byte[] EncodeToPNG();

  // Properties
  int mipmapCount { get; }
  string name { get; }
  //TextureFormat format { get; }
}

}