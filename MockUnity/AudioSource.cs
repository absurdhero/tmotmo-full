using System;
namespace UnityEngine
{
public interface AudioSource

{

  // Methods
  void Play(ulong delay);
  void Play();
  void Stop();
  void Pause();
  void PlayOneShot(AudioClip clip, float volumeScale);
  void PlayOneShot(AudioClip clip);
  float[] GetOutputData(int numSamples, int channel);
  void GetOutputData(float[] samples, int channel);
  //float[] GetSpectrumData(int numSamples, int channel, FFTWindow window);
  //void GetSpectrumData(float[] samples, int channel, FFTWindow window);


  // Properties
  float volume { get; set; }
  float pitch { get; set; }
  float time { get; set; }
  int timeSamples { get; set; }
  AudioClip clip { get; set; }
  bool isPlaying { get; }
  bool loop { get; set; }
  bool ignoreListenerVolume { get; set; }
  bool playOnAwake { get; set; }
 // AudioVelocityUpdateMode velocityUpdateMode { get; set; }
  float panLevel { get; set; }
  bool bypassEffects { get; set; }
  float dopplerLevel { get; set; }
  float spread { get; set; }
  int priority { get; set; }
  bool mute { get; set; }
  float minDistance { get; set; }
  float maxDistance { get; set; }
  float pan { get; set; }
  //AudioRolloffMode rolloffMode { get; set; }
  float minVolume { get; set; }
  float maxVolume { get; set; }
  float rolloffFactor { get; set; }

}
}

