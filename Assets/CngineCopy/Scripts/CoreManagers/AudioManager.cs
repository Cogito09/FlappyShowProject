using ProjectOne;
using Unity.Collections;
using UnityEngine;

namespace Cngine
{
    public class AudioManager : MonoBehaviour, ISpawnCallbackReceiver
    {
        [SerializeField] private UnityEngine.AudioSource _mainAudioSource;
        private AudioClip _currentAudioClip;
        public bool IsMuted;

        public void SetupAudioClip(AudioClip AudioClip)
        {
            if (AudioClip == null)
            {
                Debug.Log("track cfg is null , cant load track");
                return;
            }

            _currentAudioClip = AudioClip;
            _mainAudioSource.clip = _currentAudioClip;
        }

        public void StartAudio(double _timeBeforeTrackStart)
        { 
            if (_mainAudioSource.clip == null)
            {
                Debug.Log("Audio clip is not loaded , cant start audio");
            }

            if (IsMuted)
            {
                Log.Info("Is mutted, not launching audio.");
                return;
            }

            _mainAudioSource.PlayDelayed((float)_timeBeforeTrackStart);
        }

        public NativeArray<float> GetAudioSpectrumInNativeArray()
        {
            var spectrum = _mainAudioSource.GetSpectrumData(64, 0, FFTWindow.BlackmanHarris);
            return new NativeArray<float>(spectrum, Allocator.Temp);
        }
                    
        public float[] GetAudioSpectrum(int magrnitudeOf2arrayLength)
        {
            float[] spectrum = new float[magrnitudeOf2arrayLength];
            _mainAudioSource.GetSpectrumData(spectrum, 0, FFTWindow.Triangle);
            return spectrum;
        }

        public float[] GetWaveform(int magrnitudeOf2arrayLength)
        {
            float[] waveform = new float[magrnitudeOf2arrayLength];
            _mainAudioSource.GetOutputData(waveform, 0);
            return waveform;
        }

        public float[] bufferedWaveform;
        public float[] GetWaveformBuffered(int magrnitudeOf2arrayLength)
        {
            float[] currentFrameWaveform = GetWaveform(magrnitudeOf2arrayLength);
            if (bufferedWaveform == null || bufferedWaveform.Length != currentFrameWaveform.Length)
            {
                bufferedWaveform = new float[64];
                bufferedWaveform = currentFrameWaveform;
                return bufferedWaveform;
            }

            for (int i = 0; i < currentFrameWaveform.Length; i++)
            {
                if (currentFrameWaveform[i] > bufferedWaveform[i])
                {
                    bufferedWaveform[i] = currentFrameWaveform[i];
                    continue;
                }

                bufferedWaveform[i] -= bufferedWaveform[i] * 0.01f;
            }

            return bufferedWaveform;
        }
        
        public void StopAudio()
        {   
            _mainAudioSource.Stop();
        }

        public void OnInstantiated()
        {
            Log.Info($"AudioManager Instantiated");
            GameMasterBase.AudioManager = this;
        }
    }
}