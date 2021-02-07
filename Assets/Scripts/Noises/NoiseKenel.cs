using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NoiseKenel : ScriptableObject
{
    public float XOffset = 0;
    public float YOffset = 0;
    public float Zoffset = 0;

    //public abstract float GetPointValue(Vector3 pos,Vector3 gridSize, string seed);
    public abstract float GetPointValue(Vector3 pos, Vector3 gridSize);
}
