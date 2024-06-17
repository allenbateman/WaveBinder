# WaveBinder

WaveBinder is a tool for Unity that allows real-time audio processing to create audio-reactive elements in a game. It provides an easy to use interface that allows to bind custom properties to different frequency range and interpolate the value given the amplitude of the samples.

## Requirements

* Windows or Linux (x64 or ARM64)
* Unity 2022.3.0f1 or later
  
### Installation

https://github.com/allenbateman/WaveBinder.git

![packageManager](https://github.com/allenbateman/WaveBinder/assets/57528826/e7be11cd-4296-4cf4-b116-31d24c5448c6)


## Demos

## Audio Analyser 


![AudioAnalyser Component](https://github.com/allenbateman/WaveBinder/assets/57528826/2c1cdba2-573b-47a9-b8f0-4151f1454936)


### Audio Bands

The audio bands gives you the posibility to select what frequency range to analyse and returns the normalised average amplitude of the selected range, it also has a smooth factor value to reduce the drastic change of values.

![AudioBand](https://github.com/allenbateman/WaveBinder/assets/57528826/225b2574-b918-48d6-ace5-342452057735)

### Property Binders

Property binders allows you to add any component and select what porperty of the component to be audio driven.

Internally it uses interpolation to interpolate between to given values for that parameter, using the audio band data as the value used to interpolate.

![PropertyBinder](https://github.com/allenbateman/WaveBinder/assets/57528826/87ed1f27-aad8-4937-bb66-896f0d91d53d)


## License

Donut is licensed under the [MIT License](LICENSE.txt).
