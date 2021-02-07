using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Brush : ScriptableObject
{
    [Range(0.1f,5f)]
    public float brushSize = 1f;
    [Range(0,1)]
    public float strenght = 1f;

    public Transform brushIndcator;

    void HAHA()
    {
        float test = Mathf.PerlinNoise(1, 1);
    }
}
