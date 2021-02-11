using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneCubeTest : MonoBehaviour
{
    [Range(0.5f,5)]
    public float edgeSize = 1f;
    public  Transform testVetex;

    public MeshFilter meshFilter;
    private Mesh mesh;

    private Cell cell;

    private void Start()
    {
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        AddVertecies();
    }

    public void AddVertecies()
    {
        Vertex[] temp = new Vertex[8];

        //bottom sqer
        temp[0] = new Vertex(transform.position.x,            transform.position.y,            transform.position.z,0.5f);
        temp[1] = new Vertex(transform.position.x + edgeSize, transform.position.y,            transform.position.z, 0.5f);
        temp[2] = new Vertex(transform.position.x + edgeSize, transform.position.y,            transform.position.z + edgeSize, 0.5f);
        temp[3] = new Vertex(transform.position.x,            transform.position.y,            transform.position.z + edgeSize, 0.5f);
        //top sqer
        temp[4] = new Vertex(transform.position.x,            transform.position.y + edgeSize, transform.position.z, 0.5f);
        temp[5] = new Vertex(transform.position.x + edgeSize, transform.position.y + edgeSize, transform.position.z, 0.5f);
        temp[6] = new Vertex(transform.position.x + edgeSize, transform.position.y + edgeSize, transform.position.z + edgeSize, 0.5f);
        temp[7] = new Vertex(transform.position.x,            transform.position.y + edgeSize, transform.position.z + edgeSize, 0.5f);


        cell = new Cell(temp,1f);


        //Instantatie vertex for debug
        for (int i = 0; i < temp.Length; i++)
        {
            Instantiate(testVetex, temp[i].WordPos, Quaternion.identity, transform).
                GetComponent<TestVertex>().SetUp(temp[i], cell,this);
        }

    }

    public void UpdateMesh()
    {

        mesh.Clear();
        int cubeIndex = cell.GetCombination();
        //print(cubeIndex);

        if (cubeIndex == 0)
            return;

        mesh.vertices = cell.GetVertxPos().ToArray();
        mesh.triangles = cell.GetVertexOrder(cubeIndex).ToArray();
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
        
    }

    private void OnDrawGizmos()
    {
        if (cell != null)
        {
            for (int i = 0; i < cell.maiVerecies.Length ; i++)
            {
              //  Gizmos.color = cell.maiVerecies[i].Active ? Color.green : Color.red;
                Gizmos.DrawCube(cell.maiVerecies[i].WordPos, Vector3.one / 4);
            }
            
            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(transform.position + Vector3.one * (edgeSize / 2), Vector3.one * (edgeSize + 1/4));
        }
    }
        
}
