using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu()]
public class PerlinNoise2D : NoiseKenel
{
    public float amplitude;
    public float freqency;
    public float seed;

    private float temp = 0.01f;

    public override float GetPointValue(Vector3 pos, Vector3 gridSize,float isoValue)
    {

        float percentX = pos.x / gridSize.x;
        float percetZ = pos.z / gridSize.z;
        float noiseValue = Mathf.PerlinNoise(percentX * freqency + seed, percetZ * freqency + seed);

        float height = noiseValue * amplitude;

        if (pos.y > height) return Mathf.Clamp(noiseValue, isoValue + temp,1);
        else return Mathf.Clamp(noiseValue, 0, isoValue - temp);


    }

    public override void Initalize()
    {
        
    }
}
