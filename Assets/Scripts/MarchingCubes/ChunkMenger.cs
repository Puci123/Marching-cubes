using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkMenger : MonoBehaviour
{
    public Vector2Int chunksNumber;
    public Vector3 chunkSize;
    public Material terainMaterial;

    public float cellSize;
    public float isoValue = 0.5f;
   
    public bool aplyColider = true;
    public bool showGizmos = true;
    public static bool interpolateVertecies = true;


    public NoiseKenel noiseKenel;

    private Transform map;
    private Vector3 chunkScale = Vector3.one * 10;
    private List<Transform> chunks = new List<Transform>();

    public void GenerterWolrd()
    {
  
        foreach (Transform child in chunks)
        {
            DestroyImmediate(child.gameObject);
        }

        chunks.Clear();

        CreateTerain();
    }

    private void CreateTerain()
    {

        Vector3 halfMapSize = new Vector3(chunkScale.x * chunkSize.x * chunksNumber.x, 0, chunkScale.z * chunkSize.z * chunksNumber.y);
        halfMapSize.x = halfMapSize.x / 2 - (chunkSize.x * chunkScale.x / 2);
        halfMapSize.z = halfMapSize.z / 2 - (chunkSize.z * chunkScale.z / 2);

        Vector3 spawnPos = transform.position - halfMapSize;


        for (int z = 0; z < chunksNumber.y; z++)
        {
            for (int x = 0; x < chunksNumber.x; x++)
            {
                Vector3 offset = new Vector3(x * chunkScale.x * chunkSize.x, 0, z * chunkSize.z * chunkScale.z);

                GameObject temp = new GameObject("Chunk " + x + ":" + z);

                temp.transform.localScale = chunkScale;
                temp.AddComponent<MeshFilter>();
                temp.AddComponent<MeshRenderer>().material = terainMaterial;
                temp.AddComponent<MeshCollider>();
                temp.transform.position = spawnPos + offset;
                temp.AddComponent<Chunk>().SetUpChunk(chunkSize, cellSize, isoValue, aplyColider, noiseKenel);
                temp.transform.parent = map;

                chunks.Add(temp.transform);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.black;

            Vector3 halfMapSize = new Vector3(chunkScale.x * chunkSize.x * chunksNumber.x, 0, chunkScale.z * chunkSize.z * chunksNumber.y);

            halfMapSize.x = halfMapSize.x / 2 - (chunkSize.x * chunkScale.x / 2);
            halfMapSize.z = halfMapSize.z / 2 - (chunkSize.z * chunkScale.z / 2);

            Vector3 spawnPos = transform.position - halfMapSize;


            for (int z = 0; z < chunksNumber.y; z++)
            {
                for (int x = 0; x < chunksNumber.x; x++)
                {
                    Vector3 offset = new Vector3(x * chunkScale.x * chunkSize.x, 0, z * chunkSize.z * chunkScale.z);
                    Vector3 temp = new Vector3(chunkScale.x * chunkSize.x, chunkScale.y * chunkSize.y, chunkScale.z * chunkSize.z);
                    Vector3 pos = spawnPos + offset;

                    Gizmos.DrawWireCube(pos, temp);
                }
            }
        }
    }
}
