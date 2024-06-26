using UnityEngine;
using UnityEditor;
using UnityEditor.Rendering;
using WaveBinder.Runtime;
namespace WaveBinder.Editor
{
    public class AudioBandEditor
    {

        SerializedProperty _bandList;
        AudioAnalyzer _analyzer;
        public AudioBandEditor(AudioAnalyzer analyser,SerializedProperty audioBands)
        {
            _bandList = audioBands;
            _analyzer = analyser;
        }


        public void ShowGUI()
        {
            EditorGUILayout.Space();

            for (int i = 0; i < _bandList.arraySize; i++)
            {
                ShowAudioBandEditor(i);
            }

            CoreEditorUtils.DrawSplitter();
            EditorGUILayout.Space();

            var rect = EditorGUILayout.GetControlRect();
            rect.x += (rect.width - 200) * 0.5f;
            rect.width = 200;

            if (GUI.Button(rect, "Add audio band"))
            {
                OnNewAddAudioBand();
            }
        }

        void ShowAudioBandEditor(int i)
        {
            var audioBand = _bandList.GetArrayElementAtIndex(i);
            var finder = new RelativePropertyFinder(audioBand);

            _bandList.serializedObject.Update();

            //header 
            CoreEditorUtils.DrawSplitter();
            CoreEditorUtils.DrawHeader("Audio band " + i.ToString());

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(finder["_minRangeFrequency"]);
            if (EditorGUI.EndChangeCheck())
            {
                _analyzer.UpdateBandRange(i);
            }
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(finder["_maxRangeFrequency"]);
            if (EditorGUI.EndChangeCheck())
            {
                _analyzer.UpdateBandRange(i);
            }
            EditorGUILayout.PropertyField(finder["_smoothFactor"]);

            _bandList.serializedObject.ApplyModifiedProperties();

            var rect = EditorGUILayout.GetControlRect();
            rect.x += (rect.width - 100) * 0.5f;
            rect.width = 100;

            if (GUI.Button(rect, "Remove"))
            {
                OnRemoveAudioBand(i);
            }
        }
        void OnNewAddAudioBand()
        {
            _bandList.serializedObject.Update();

            var i = _bandList.arraySize;
            _bandList.InsertArrayElementAtIndex(i);

            SerializedProperty _band = _bandList.GetArrayElementAtIndex(i);
            var finder = new RelativePropertyFinder(_band);

            // Get the properties of the AudioBand object
            SerializedProperty minFreqProp = finder["_minRangeFrequency"];
            SerializedProperty maxFreqProp = finder["_maxRangeFrequency"];
            SerializedProperty smoothFactor = finder["_smoothFactor"];

            // initialize the properties
            minFreqProp.intValue = 20;
            maxFreqProp.intValue = 20000;
            smoothFactor.floatValue = 1f;

            _bandList.serializedObject.ApplyModifiedProperties();
        }
        void OnRemoveAudioBand(int i)
        {
            _bandList.serializedObject.Update();
            _bandList.DeleteArrayElementAtIndex(i);
            _bandList.serializedObject.ApplyModifiedProperties();
        }
    }
}
