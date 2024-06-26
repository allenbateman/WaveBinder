# WaveBinder

WaveBinder is a tool for Unity that allows real-time audio processing to create audio-reactive elements in a game. It provides an easy-to-use interface that allows the binding of custom properties to different frequency ranges and interpolates the value given the average amplitude of the sample range.

## Requirements

* Windows or Linux (x64 or ARM64)
* Unity 2022.3.0f1 or later
  
### Installation

To add the package to your Unity project, go to Window -> Package Manger -> Left top corner and click  the "+", finally select the "Add package from git URL" and paste the following link.

https://github.com/allenbateman/WaveBinder.git

![packageManager](https://github.com/allenbateman/WaveBinder/assets/57528826/e7be11cd-4296-4cf4-b116-31d24c5448c6)

Import the sample scene that fits the configuration of your project Standard/URP

![Sample Scenes](https://github.com/allenbateman/WaveBinder/assets/57528826/a82f0306-4e83-4c67-ad50-db29715d3672)

## Demos

![image21](https://github.com/allenbateman/WaveBinder/assets/57528826/70e654cb-e97b-4bd8-962c-c335aca68136)

This is the audio spectrum divided into frequency ranges used in music

## Audio Analyser 

The audio analyser component is the main and only component of the tool, it allows you to add a desired audio clip to analyse, select the number of samples (the sample resolution), and keep in mind that the more samples you choose more processing power will need so unless you need a high resolution of the audio data keep the samples low for simpler audios.

The Windowing is an algorithm to analyse the audio spectrum depending on which one you choose it will be more precise and will require more processing power, a good in-between for quality and medium processing power is Blackman-Harris. 

The sensitivity is a value multiplier on the sample values. If you add more sensitivity the lower amplitudes on the frequency spectrum will be more noticeable on the final normalized value. This sensitivity value has to be setup before runtime, setting it on runtime will affect the final output if you increase and lower it.

Finally, a display spectrum toggle renders the sample values on a texture so you can visualize the raw audio spectrum data.

![image](https://github.com/allenbateman/WaveBinder/assets/57528826/5161efa5-a37a-4347-a128-7ebf1091e5d2)


### Audio Bands

The audio bands give you the possibility to select what frequency range to analyse and return the normalised average amplitude of the selected range, it also has a smooth factor value to reduce the drastic change of values.

![AudioBand](https://github.com/allenbateman/WaveBinder/assets/57528826/225b2574-b918-48d6-ace5-342452057735)

### Property Binders

Property binders allow you to add any component and select what property of the component to be audio-driven.

Internally it uses interpolation to interpolate between two given values for that parameter, using the audio band data as the value used to interpolate.

![PropertyBinder](https://github.com/allenbateman/WaveBinder/assets/57528826/87ed1f27-aad8-4937-bb66-896f0d91d53d)


## License

Donut is licensed under the [MIT License](LICENSE.txt).
