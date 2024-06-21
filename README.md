# WaveBinder

WaveBinder is a tool for Unity that allows real-time audio processing to create audio-reactive elements in a game. It provides an easy-to-use interface that allows the binding of custom properties to different frequency ranges and interpolates the value given the average amplitude of the sample range.

## Requirements

* Windows or Linux (x64 or ARM64)
* Unity 2022.3.0f1 or later
  
### Installation

To add the package to your Unity project, go to Window -> Package Manger -> Left top corner and click  the "+", finally select the "Add package from git URL" and paste the following link.

https://github.com/allenbateman/WaveBinder.git

![packageManager](https://github.com/allenbateman/WaveBinder/assets/57528826/e7be11cd-4296-4cf4-b116-31d24c5448c6)


## Demos

![image21](https://github.com/allenbateman/WaveBinder/assets/57528826/70e654cb-e97b-4bd8-962c-c335aca68136)

This is the audio spectrum divided into frequency ranges used in music

## Audio Analyser 

The audio analyser component is the main and only component of the tool, it allows you to add a desired audio clip to analyse, select the number of samples (the sample resolution) and also to select the windowing algorithm to analyse the audio clip. 

![AudioAnalyser Component](https://github.com/allenbateman/WaveBinder/assets/57528826/2c1cdba2-573b-47a9-b8f0-4151f1454936)


### Audio Bands

The audio bands give you the possibility to select what frequency range to analyse and return the normalised average amplitude of the selected range, it also has a smooth factor value to reduce the drastic change of values.

![AudioBand](https://github.com/allenbateman/WaveBinder/assets/57528826/225b2574-b918-48d6-ace5-342452057735)

### Property Binders

Property binders allow you to add any component and select what property of the component to be audio-driven.

Internally it uses interpolation to interpolate between two given values for that parameter, using the audio band data as the value used to interpolate.

![PropertyBinder](https://github.com/allenbateman/WaveBinder/assets/57528826/87ed1f27-aad8-4937-bb66-896f0d91d53d)


## License

Donut is licensed under the [MIT License](LICENSE.txt).
