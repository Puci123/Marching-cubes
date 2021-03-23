using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NoiseKenel : ScriptableObject
{
   
    public abstract float GetPointValue(Vector3 pos, Vector3 gridSize,float isoValue);
    public abstract void RandomSeed();
    public abstract void Initalize();
}
