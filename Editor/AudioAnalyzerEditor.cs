using UnityEditor;
using UnityEngine;
using WaveBinder.Runtime;


namespace WaveBinder.Editor
{
    [CustomEditor(typeof(AudioAnalyzer))]
    public class AudioAnalyzerEditor : UnityEditor.Editor
    {
        PropertyBinderEditor _propertyBinderEditor;
        AudioBandEditor      _audioBandEditor;

        SerializedProperty _audioClip;
        SerializedProperty _nSamples;
        SerializedProperty _fftWindow;
        SerializedProperty _channel;
        SerializedProperty _bandList;

        bool _DispalySpectrum;
        Texture2D _spectrumTex;

        private void OnEnable()
        {
            var finder = new PropertyFinder(serializedObject);


            //_value = serializedObject.FindProperty("value"); 
            //propertyFinder utility saves some code lines
            _audioClip  = finder["_audioClip"];
            _nSamples   = finder["_nSamples"];
            _fftWindow  = finder["_fftWindow"];
            _channel    = finder["_channel"];
            _bandList   = finder["_bandList"];

            // Link the SerializedProperty to the variable 
            _propertyBinderEditor = new PropertyBinderEditor(finder["_propertyBinders"],_bandList);
            _audioBandEditor = new AudioBandEditor(_bandList);
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            // Input settings
            EditorGUILayout.PropertyField(_audioClip);
            EditorGUILayout.PropertyField(_fftWindow);
            EditorGUILayout.PropertyField(_nSamples);
            //EditorGUILayout.PropertyField(_channel);
            //EditorGUILayout.PropertyField(_bandList);


            _DispalySpectrum = EditorGUILayout.Toggle("Display Spectrum", _DispalySpectrum);
            if(_DispalySpectrum)
            {
                RenderAudioSpectrum();
                //draw texture here
                if (_spectrumTex != null)
                {
                    GUILayout.Label(_spectrumTex);
                }
            }


            serializedObject.ApplyModifiedProperties();

            if (targets.Length == 1)
            {
                _audioBandEditor.ShowGUI();
                _propertyBinderEditor.ShowGUI(); 
            }
        }

        void RenderAudioSpectrum()
        {
            AudioAnalyzer analyzer = (AudioAnalyzer)target;

            if (target == null) return;

            float[] spectrumData = analyzer.GetSpectrumData();
            if(spectrumData != null && spectrumData.Length > 0)
            {
                int width = spectrumData.Length;
                int height = 255; 
                if(_spectrumTex == null || _spectrumTex.width != width || _spectrumTex.height != height)
                {
                    _spectrumTex = new Texture2D(width,height, TextureFormat.ARGB32, false);
                }

                //init color array
                Color[] colors = new Color[width * height];
                // Set color based on amplitude of each sample
                for (int x = 0; x < width; x++)
                {
                    float amplitude = spectrumData[x] * height * 10f; // Map amplitude to texture height
                    for (int y = 0; y < height; y++)
                    {
                        if(amplitude < y + 5 && amplitude > y - 5) 
                        {

                            colors[y * width + x] = Color.white;
                        }
                        else
                        {
                            colors[y * width + x] = Color.black;
                        }

                        //if (y < amplitude)
                        //{
                        //    colors[y * width + x] = Color.white;
                        //}
                        //else
                        //{
                        //    colors[y * width + x] = Color.black;
                        //}
                    }
                }
                _spectrumTex.SetPixels(colors);
                _spectrumTex.Apply();
            }
        }
    }
}