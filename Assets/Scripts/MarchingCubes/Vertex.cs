using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex 
{
    private Vector3 wordPos;

    private float vertexValue;
    private float isoValue;

    public Vertex(Vector3 _wordldPos,float _value,float _isoValue)
    {
        WordPos = _wordldPos;
        VertexValue = _value;
        isoValue = _isoValue;
    }

    public Vertex(Vector3 _wrodlPos, float _isoValue) : this(_wrodlPos, -1, _isoValue) {}
    public Vertex(float x, float y, float z, float _isoValu) : this(new Vector3(x, y, z), -1, _isoValu) {}
    public Vertex(float x, float y, float z, float value, float _isoValu) : this(new Vector3(x, y, z), value, _isoValu) { }


    public float VertexValue
    {
        get { return vertexValue; }
        set {
                vertexValue = Mathf.Clamp(value, -1, 1);   
                
            ;}
    }

    public Vector3 WordPos
    {
        get { return wordPos; }
        set { wordPos = value; }
    }
}
