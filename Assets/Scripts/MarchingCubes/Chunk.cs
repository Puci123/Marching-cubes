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

    [SerializeField]
    private bool showGizmos;

    private Vector3Int chunkTabSzie;
    private Vertex[,,] grid;
    private Cell[,,] chunks;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
            StartCoroutine(DebugVertexValues());
        else if (Input.GetKeyDown(KeyCode.C))
            StartCoroutine(DebugIntersectionccions());
    }

    IEnumerator DebugVertexValues()
    {
        foreach (Vertex vertex in grid)
        {
            print(vertex.VertexValue);
            yield return null;
        }
    }

    IEnumerator DebugIntersectionccions()
    {
        foreach (Cell cell in chunks)
        {
            foreach (Vertex vert in cell.verticesInresectons)
            {
                print(vert.WordPos);
            }

            yield return null;
        }
    }

    private void SetUpGrid()
    {
        chunkTabSzie.x = Mathf.RoundToInt(chunkSize.x / cellSize) + 1;
        chunkTabSzie.y = Mathf.RoundToInt(chunkSize.y / cellSize) + 1;
        chunkTabSzie.z = Mathf.RoundToInt(chunkSize.z / cellSize) + 1;

        grid = new Vertex[chunkTabSzie.x, chunkTabSzie.y,chunkTabSzie.z];
        chunks = new Cell[chunkTabSzie.x - 1, chunkTabSzie.y - 1, chunkTabSzie.z - 1];

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
                    grid[x, y, z] = new Vertex(temp,GetVertexValue(temp),isoValue);
                  
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
                    vertcies[0] = grid[x, y, z];
                    vertcies[1] = grid[x + 1, y, z];
                    vertcies[2] = grid[x +1, y, z + 1];
                    vertcies[3] = grid[x, y, z + 1];
                    //UpSqer
                    vertcies[4] = grid[x, y + 1, z];
                    vertcies[5] = grid[x + 1, y + 1, z];
                    vertcies[6] = grid[x + 1, y + 1, z + 1];
                    vertcies[7] = grid[x, y + 1, z + 1];

                    chunks[x, y, z] = new Cell(vertcies,isoValue);
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

        foreach (Cell cell in chunks)
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

   
    private void OnValidate()
    {
        if(chunkSize == Vector3.zero)
        {
            Debug.LogError("Chunk szie cnat be 0");
        }
        if(cellSize <= 0)
        {
            Debug.LogError("Cell size must be greater tan 0");
        }
    }
        
    public Vertex GetVertexFromWorldPoint(Vector3 position)
    {
        float PercentX = Mathf.Clamp01(position.x / chunkSize.x);
        float PercentY = Mathf.Clamp01(position.y / chunkSize.y);
        float PercentZ = Mathf.Clamp01(position.z / chunkSize.z);

        int x = Mathf.RoundToInt((chunkTabSzie.x - 1) * PercentX);
        int y = Mathf.RoundToInt((chunkTabSzie.y - 1) * PercentY);
        int z = Mathf.RoundToInt((chunkTabSzie.z - 1) * PercentZ);

        return grid[x, y, z];

    }

    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            if (grid != null && grid.Length > 0)
            {
                foreach (Cell chunk in chunks)
                {
                    Gizmos.color = chunk.cellColor;
                  //  Gizmos.DrawCube(chunk.Center(), Vector3.one * cellSize * 0.8f);
                }
            }

            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(transform.position,new Vector3(chunkSize.x, chunkSize.y, chunkSize.z));
        }
    }
}
