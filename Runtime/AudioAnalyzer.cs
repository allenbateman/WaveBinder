using System.Collections.Generic;
using UnityEngine;
using UnityEditor.PackageManager.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace WaveBinder.Runtime
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioAnalyzer : MonoBehaviour
    {
        #region Editor fields
        AudioSource _audioSource;
        public AudioClip _audioClip;
        public List<AudioBand> _bandList = new List<AudioBand>() { new AudioBand(0,20000)};
       
        [PowerOfTwoSlider]
        public int _nSamples = 128;
        public FFTWindow _fftWindow;
        public enum _Channels
        {
            Stereo, Left, Right
        }
        public _Channels _channel = new _Channels();
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

        //amplitude
        [HideInInspector]
        public float _amplitude, _amplitudeBuffer;
        float _amplitudeHighest = 0.01f;
        private bool _NoAudioClip;
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            if (_audioClip != null)
            {
                _audioSource.clip = _audioClip;
                _audioSource.Play();    
            }
            if(_audioSource.clip != null && _audioClip == null)
            {
                _audioClip = _audioSource.clip;
            }
            if(!_audioClip && !_audioSource.clip)
            {
                _NoAudioClip = true;
                Debug.LogWarning("There is no audio clip set in audio analyser");
            }
        }
        void Start()
        {
            if (_NoAudioClip)
            {
                return;
            }
            UpdateAudioBands();
        }

        void Update()
        {
            if(_NoAudioClip)
            {
                return;
            }

            
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
                    if (_bandList[propertyBinder.AudioBand]._Smoothing)
                    {
                        propertyBinder.Level = _bandList[propertyBinder.AudioBand]._normalizedAmpBuffer;
                    }
                    else
                    {
                        propertyBinder.Level = _bandList[propertyBinder.AudioBand]._normalizedAmp;
                    }

                }
            }
        }

        public void GenerateFrequencyBands()
        {
            _spectrum = GetSpectrumData();
            if (_spectrum == null)
            { 
                Debug.Log("_spectrum null");
                return;
            }

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
        void UpdateAudioBands()
        {
            foreach (var band in _bandList)
            {
                band.Init();
                band.MapFrequencyToSamples(_nSamples);
            }
        }
    }
}