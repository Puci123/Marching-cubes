using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class PerlinNoise3D : NoiseKenel
{

    public float freqency = 1f;

    public override float GetPointValue(Vector3 pos, Vector3 gridSize, float isoValue)
    {
        double x = pos.x;
        double y = pos.y;
        double z = pos.z;


        return (float)NoiseS3D.Noise(x,y,z);
    }
}
