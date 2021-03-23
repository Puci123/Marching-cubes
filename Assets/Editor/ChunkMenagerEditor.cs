using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChunkMenger))]
public class ChunkMenagerEditor : Editor
{
    private static bool showNoiseOptions = true;

    public override void OnInspectorGUI()
    {
        ChunkMenger menger = (ChunkMenger)target;
        base.OnInspectorGUI();  

        if(menger.noiseKenel != null)
        {
            showNoiseOptions = EditorGUILayout.Foldout(showNoiseOptions, "Noise Options");
            if (showNoiseOptions)
            {
                var editor = Editor.CreateEditor(menger.noiseKenel);
                editor.OnInspectorGUI();
            }

            if (GUILayout.Button("Generte new tereein"))
            {
                menger.GenerterWolrd();
            }
        }
    }
}
