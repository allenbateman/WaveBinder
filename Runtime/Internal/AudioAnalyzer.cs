using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace WaveBinder
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioAnalyzer : MonoBehaviour
    {
        #region Editor fields
        AudioSource _audioSource;
        public AudioClip _audioClip;
        public List<AudioBand> _bandList = new List<AudioBand>() { new AudioBand(0,20000)};
        [PowerOfTwoSlider]
        public int _nSamples = 512;
        public FFTWindow _fftWindow;
        private int _audioSamplingRate;
        private float _frequencyResolution;
        private float[] _spectrum;

        [SerializeReference] PropertyBinder[] _propertyBinders = null;
        public PropertyBinder[] propertyBinders
        {
            get => (PropertyBinder[])_propertyBinders.Clone();
            set => _propertyBinders = value;
        }

        #endregion

        #region Editor funcitons
        public float[] GetSpectrumData()
        {
            if (_audioSource == null || _audioSource.clip == null) return null;
            _spectrum = new float[_nSamples];
            _audioSource.GetSpectrumData(_spectrum, (int)_channel, _fftWindow);
            return _spectrum;
        }
        #endregion

        #region Old AudioAuanlyzer
        public enum _Channels
        {
            Stereo, Left, Right
        }

        public _Channels _channel = new _Channels();
        // number of samples taken form the auido range
        // 0->0 & 512->20k
        public int nSamples = 512;

        //[HideInInspector]
        public float[] _audioBand, _audioBandBuffer;

        //how fast the buffer empties
        public float _bufferDecay;

        //amplitude
        [HideInInspector]
        public float _amplitude, _amplitudeBuffer;
        float _amplitudeHighest = 0.01f;


        //profiler 
        // this variable will smooth the visualization
        public float _audioProfiler;

        private void OnEnable()
        {

        }
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            if (_audioClip != null)
            {
                _audioSource.clip = _audioClip;
                _audioSource.Play();    
            }
            _audioSamplingRate = AudioSettings.outputSampleRate;
        }
        void Start()
        {

            UpdateAudioBands();
            AudioProfiler(_audioProfiler);
        }

        void Update()
        {
            GetSpectrumData();            
            GenerateFrequencyBands();

            foreach(var band in _bandList)
            {
                band.Update();
            }

            GetAmplitud();
            
            if (_propertyBinders != null)
            {
                foreach (var propertyBinder in _propertyBinders)
                {
                    propertyBinder.Level = _bandList[propertyBinder.AudioBand]._normalizedAmpBuffer;//NAN Value
                }
            }
        }

        void GenerateFrequencyBands()
        {            
            //this var stores the average of the samples for that freq band
            for (int i = 0; i < _bandList.Count; i++)
            {
                int minSampleIndex = _bandList[i].GetMinRangeSample();
                int maxSampleIndex = _bandList[i].GetMaxRangeSample();

                float average = 0;

                //switch over the channel selected to fill up the samples
                for (int j = minSampleIndex; j < maxSampleIndex; j++)
                {
                    average += _spectrum[j];                    
                }
                average /= (maxSampleIndex - minSampleIndex);
                //multiply the average to work with higher values in high frequencies
                _bandList[i].SetAmplitude(average*10);
            }
        }
        //get average amplitude of the whole spectrum
        void GetAmplitud()
        {
            float currentAmplitude = 0f;
            float currentAmplitudeBuffer = 0f;

            foreach(var band in _bandList)
            {
                currentAmplitude += band._normalizedAmp;
                currentAmplitudeBuffer += band._normalizedAmpBuffer;
            }
            if (currentAmplitude > _amplitudeHighest)
            {
                _amplitudeHighest = currentAmplitude;
            }

            _amplitude = currentAmplitude / _amplitudeHighest;
            _amplitudeBuffer = currentAmplitudeBuffer / _amplitudeHighest;
        }
        // Pre-read the highest values so at the start of the audio file already it is initialized to use
        void AudioProfiler(float audioProfiler)
        {
            foreach(var band in _bandList)
            {
                band.AudioProfiler(audioProfiler);
            }
        }
        void UpdateAudioBands()
        {
            _frequencyResolution = (float)_audioSamplingRate / nSamples;
            Debug.Log("Frequency resolution: " + _frequencyResolution);

            foreach (var band in _bandList)
            {
                band.MapFrequencyToSamples(_frequencyResolution, nSamples);
            }
        }
        #endregion
    }
}