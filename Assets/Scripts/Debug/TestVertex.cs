using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVertex : MonoBehaviour
{
    public Vertex vertr;
    public Cell cell;
    public OneCubeTest test;

    public void SetUp(Vertex _vert, Cell _cell,OneCubeTest _test)
    {
        vertr = _vert;
        cell = _cell;
        test = _test;
    }

    private void Update()
    {
       // GetComponent<MeshRenderer>().material.color = vertr.Active ? Color.green : Color.red;
    }

    private void OnMouseDown()
    {
        vertr.VertexValue *= -1;
        //print(cell.GetCombination());
        test.UpdateMesh();
     
    }

}
