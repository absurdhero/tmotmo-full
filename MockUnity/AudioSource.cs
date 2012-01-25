using System;
namespace UnityEngine
{
public abstract class AudioSource : Behaviour
{

  // Methods
  public void Play(ulong delay) { throw new InvalidOperationException(); }
  public void Play() { throw new InvalidOperationException(); }
  public void Stop() { throw new InvalidOperationException(); }
  public void Pause() { throw new InvalidOperationException(); }
  public void PlayOneShot(AudioClip clip, float volumeScale) { throw new InvalidOperationException(); }
  public void PlayOneShot(AudioClip clip) { throw new InvalidOperationException(); }
  public float[] GetOutputData(int numSamples, int channel) { throw new InvalidOperationException(); }
  public void GetOutputData(float[] samples, int channel) { throw new InvalidOperationException(); }
  //public float[] GetSpectrumData(int numSamples, int channel, FFTWindow window);
  //public void GetSpectrumData(float[] samples, int channel, FFTWindow window);


  // Properties
  public float volume { get; set; }
  public float pitch { get; set; }
  public float time { get; set; }
  public int timeSamples { get; set; }
  public AudioClip clip { get; set; }
  public bool isPlaying { get { throw new InvalidOperationException(); } }
  public bool loop { get; set; }
  public bool ignoreListenerVolume { get; set; }
  public bool playOnAwake { get; set; }
 //public  AudioVelocityUpdateMode velocityUpdateMode { get; set; }
  public float panLevel { get; set; }
  public bool bypassEffects { get; set; }
  public float dopplerLevel { get; set; }
  public float spread { get; set; }
  public int priority { get; set; }
  public bool mute { get; set; }
  public float minDistance { get; set; }
  public float maxDistance { get; set; }
  public float pan { get; set; }
  //public AudioRolloffMode rolloffMode { get; set; }
  public float minVolume { get; set; }
  public float maxVolume { get; set; }
  public float rolloffFactor { get; set; }

}
}

