using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PerlinNoise3D))]
public class NoiseKernelEditor : Editor
{
    public override void OnInspectorGUI()
    {
        NoiseKenel noise = (PerlinNoise3D)target;
        base.OnInspectorGUI();

        if (GUILayout.Button("Random seed"))
        {
            noise.RandomSeed();
        }


    }

}
