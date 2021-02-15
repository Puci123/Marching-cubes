using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PerlinNoise3D : NoiseKenel
{
    private FastNoiseLite noise = new FastNoiseLite();
    public float weight = 1f;
    public float freqency = 1f;
    public float lacunarity = 1f;
    public int octaves = 3;
    public int seed = 0;


    public override void Initalize()
    {
        noise.SetNoiseType(FastNoiseLite.NoiseType.Perlin);
        noise.SetFrequency(freqency);
        noise.SetFractalLacunarity(lacunarity);
        noise.SetFractalOctaves(octaves);
        noise.SetSeed(seed);
        noise.SetFractalGain(0);
        noise.SetFractalType(FastNoiseLite.FractalType.PingPong);
    }

    public override float GetPointValue(Vector3 pos, Vector3 gridSize, float isoValue)
    {
        float x = pos.x / gridSize.x;
        float y = pos.y / gridSize.y;
        float z = pos.z / gridSize.z;

        float value = Mathf.Abs(noise.GetNoise(x, y, z)) * weight;
        return pos.y - value;
    }
}
