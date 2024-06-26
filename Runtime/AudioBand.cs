using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using WaveBinder.Runtime;

[Serializable]
public class AudioBand
{
    const int MIN_FREQUENCY = 20;
    const int MAX_FREQUENCY = 20000;
    // Detrmine the range of frequencies the audio band takes
    [SerializeField,Range(20,20000)]
    public int _minRangeFrequency = 20;
    [SerializeField, Range(20, 20000)]
    public int _maxRangeFrequency = 20;
    //TODO make private 
    private int _minRangeSample = 0;
    private int _maxRangeSample = 0;

    [SerializeField,Range(0.0001f,1f)]
    private float _smoothFactor = .5f;

    public float _amplitude { get; private set; }
    public float _amplitudeBuffer { get; private set; }
    private float _amplitudeDecrease = 0; // speed with which the buffer decreases
    private float _amplitudeHighest = 0;

    public float _normalisedAmp { get; private set; }
    public float _normalisedAmpBuffer { get; private set; }  


    //TODO Apply toggle to enable/Disable smoothing
    public bool _Smoothing { get; private set; }
    public AudioBand(int minRange, int maxRange)
    {
        this._minRangeFrequency = minRange;
        this._maxRangeFrequency = maxRange;

        _amplitude = 1;
        _amplitudeBuffer = 1;
        _amplitudeDecrease = 1;
        _amplitudeHighest = 1;
        _normalisedAmp = 1;
        _normalisedAmpBuffer = 1;
        _Smoothing = true;
    }

    public void Init()
    {
        _amplitude = 1;
        _amplitudeBuffer = 1;
        _amplitudeDecrease = 1;
        _amplitudeHighest = 1;
        _normalisedAmp = 1;
        _normalisedAmpBuffer = 1;
        _Smoothing = true;
    }

    public void MapFrequencyToSamples(int nSamples)
    {

        var result = LinearInterpolation.MapRange(_minRangeFrequency, _maxRangeFrequency, MIN_FREQUENCY, MAX_FREQUENCY, 0, nSamples);

        _minRangeSample = result.Item1;
        _maxRangeSample = result.Item2;

        //Debug.Log($"Range {_minRangeSample}, {_maxRangeSample}");
    }

    public void Update()
    {
        if (_amplitude > _amplitudeBuffer)
        {
            _amplitudeBuffer = _amplitude;
            _amplitudeDecrease = _smoothFactor;
        }
        if (_amplitudeBuffer > _amplitude)
        {
            _amplitudeBuffer -= _amplitudeDecrease;
            if(_amplitudeBuffer <= 0 )
            {
                _amplitudeBuffer = 0.001f;
            }
            //increase the decrease speed by 20%
            _amplitudeDecrease *= 1.2f; 
        }
        if(_amplitude > _amplitudeHighest)
        {
            _amplitudeHighest = _amplitude;
        }

        _normalisedAmp = (_amplitude / _amplitudeHighest);
        _normalisedAmpBuffer = (_amplitudeBuffer / _amplitudeHighest);
    }
    public void SetFrequencyRange(int min,int max)
    {
        //check range
        if(min > max)
        {
            min = max;
        }

        _minRangeFrequency = min;
        _maxRangeFrequency = max;
    }
    public int GetMinRangeSample()
    {
        return _minRangeSample;
    }
    public int GetMaxRangeSample()
    {
        return _maxRangeSample;
    }
    public float GetAmplitude()
    { 
        return _amplitude; 
    }
    public void SetAudioBand(float  frequency)
    {
        _amplitude = frequency;
    }
    public void SetAmplitude(float amp)
    {
        _amplitude = amp;
    }

    public float GetNormalisedAmp()
    {
        return _normalisedAmp;
    }

    public float GetNormalisedAmpBuff()
    {
        return _normalisedAmpBuffer;
    }

}
