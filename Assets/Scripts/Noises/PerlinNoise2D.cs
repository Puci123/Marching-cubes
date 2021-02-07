using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class PerlinNoise2D : NoiseKenel
{
    public float amplitude;
    public float freqency;

    public override float GetPointValue(Vector3 pos, Vector3 gridSize)
    {

        float percentX = pos.x / gridSize.x;
        float percetZ = pos.z / gridSize.z;

        float height = Mathf.PerlinNoise(percentX * freqency  + XOffset, percetZ * freqency + Zoffset) * amplitude;

        if (pos.y < height + YOffset) return 1;
        else return -1;
            
    }
}
