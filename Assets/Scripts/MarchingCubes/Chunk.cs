using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Vector3 chunkSize;
    public float cellSize;
    public float isoValue = 0.5f;
    public bool aplyColider = true;
    public NoiseKenel noiseKenel;

    private Vector3Int chunkTabSzie;
    private Vertex[,,] vertGrid;
    private Cell[,,] cellGrid;

    private MeshCollider col;
    private Mesh mesh;
    private MeshFilter meshFilter;

    private void Start()
    {
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        col = GetComponent<MeshCollider>();

        SetUpGrid();
        GenerateGrid();
        ConectVetecies();
        MarchCube();

        if (aplyColider)
            col.sharedMesh = mesh;

    }

    private void SetUpGrid()
    {
        chunkTabSzie.x = Mathf.RoundToInt(chunkSize.x / cellSize) + 1;
        chunkTabSzie.y = Mathf.RoundToInt(chunkSize.y / cellSize) + 1;
        chunkTabSzie.z = Mathf.RoundToInt(chunkSize.z / cellSize) + 1;

        vertGrid = new Vertex[chunkTabSzie.x, chunkTabSzie.y,chunkTabSzie.z];
        cellGrid = new Cell[chunkTabSzie.x - 1, chunkTabSzie.y - 1, chunkTabSzie.z - 1];

    }

    private void GenerateGrid()
    {
        noiseKenel.Initalize();

        for (int z = 0; z < chunkTabSzie.z; z++)
        {
            for (int y = 0; y < chunkTabSzie.y; y++)
            {
                for (int x = 0; x < chunkTabSzie.x; x++)
                {
                    Vector3 temp = new Vector3(x * cellSize + transform.position.x - chunkSize.x / 2,           //x
                                               y * cellSize + transform.position.y - chunkSize.x / 2,          //y
                                               z * cellSize + transform.position.z - chunkSize.x / 2);        //z
                    vertGrid[x, y, z] = new Vertex(temp,GetVertexValue(temp),isoValue);
                  
                }
            }
        }

        print("Gred genreted");
    }

    private void ConectVetecies()
    {
        for (int z = 0; z < chunkTabSzie.z - 1; z++)
        {
            for (int y = 0; y < chunkTabSzie.y - 1; y++)
            {
                for (int x = 0; x < chunkTabSzie.x - 1; x++)
                {

                    Vertex[] vertcies = new Vertex[8];

                    //DownSqer
                    vertcies[0] = vertGrid[x, y, z];
                    vertcies[1] = vertGrid[x + 1, y, z];
                    vertcies[2] = vertGrid[x +1, y, z + 1];
                    vertcies[3] = vertGrid[x, y, z + 1];
                    //UpSqer
                    vertcies[4] = vertGrid[x, y + 1, z];
                    vertcies[5] = vertGrid[x + 1, y + 1, z];
                    vertcies[6] = vertGrid[x + 1, y + 1, z + 1];
                    vertcies[7] = vertGrid[x, y + 1, z + 1];

                    cellGrid[x, y, z] = new Cell(vertcies,isoValue);
                }
            }
        }

        print("Vertecies Conected");

    }

    public void MarchCube()
    {
        mesh.Clear();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        List<Vector3> vertices = new List<Vector3>();

        List<int> triangle = new List<int>();
        int i = 0;

        foreach (Cell cell in cellGrid)
        {

            foreach (Cell.Triangl tri in cell.GetTriangle())
            {
                vertices.Add(tri.A);
                vertices.Add(tri.B);
                vertices.Add(tri.C);

                triangle.Add(i);
                triangle.Add(i + 1);
                triangle.Add(i + 2);

                i += 3;
            }
        }



        mesh.name = "TereinChunk";

        print(vertices.Count);
        print(triangle.Count);
        print(vertices.Count * 3);

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangle.ToArray();
        mesh.RecalculateNormals();
        meshFilter.sharedMesh = mesh;
      

        print("Cubes Marched");
    }

    private float GetVertexValue(Vector3 vertPos)
    {
        return noiseKenel.GetPointValue(vertPos, chunkSize, isoValue);
    }
    public Vertex GetVertexFromWorldPoint(Vector3 position)
    {
        float PercentX = Mathf.Clamp01(position.x / chunkSize.x);
        float PercentY = Mathf.Clamp01(position.y / chunkSize.y);
        float PercentZ = Mathf.Clamp01(position.z / chunkSize.z);

        int x = Mathf.RoundToInt((chunkTabSzie.x - 1) * PercentX);
        int y = Mathf.RoundToInt((chunkTabSzie.y - 1) * PercentY);
        int z = Mathf.RoundToInt((chunkTabSzie.z - 1) * PercentZ);

        return vertGrid[x, y, z];

    }


}
